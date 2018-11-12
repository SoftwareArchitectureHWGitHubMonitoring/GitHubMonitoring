using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitMonitor.Objects
{
    class CosineSimilarityCalculator
    {

        private class Tuple
        {

            private List<string> TupleElements;

            public List<string> Elements
            {
                get => TupleElements;

                set
                {

                    TupleElements = new List<string>();

                    foreach (string s in value)
                    {
                        TupleElements.Add(s.Replace(" ", ""));
                    }
                }
            }

            // The constructor makes and fills up the String array
            public Tuple(string[] _TupleElements)
            {
                // Remove the unnecessary spaces
                for (int i = 0; i < _TupleElements.Length; ++i)
                {
                    _TupleElements[i] = _TupleElements[i].Replace(" ", "");
                }

                this.TupleElements = new List<string>(_TupleElements);
            }
        }

        private class TokenizedDocument
        {

            // These list contains the heads, the tails, and the inner shingles
            private List<Tuple> head, tail, inner;

            public List<Tuple> HeadList {
                get => head;
            }

            public List<Tuple> TailList
            {
                get => tail;
            }

            public List<Tuple> InnerList
            {
                get => inner;
            }

            // The constructor initialize the lists and based on the wordGranularity it fills up the lists
            public TokenizedDocument(string document, int shingleSize)
            {
                head = new List<Tuple>();
                tail = new List<Tuple>();
                inner = new List<Tuple>();

                ProcessDocumentWords(document, shingleSize);
            }

            // Make the non w-shingle document
            private void ProcessDocumentWords(string document, int shingleSize)
            {
                // This helps with the calculation of the heads and tails
                // Every word will be converted to the following form: [ ]{shingleSize-1}<sentence>[ ]{shingleSize-1}
                List<string> wrapper = new List<string>(shingleSize);

                // Fill up the wrapper list
                int upperIndex = shingleSize - 1;

                for (int i = 0; i < upperIndex; ++i)
                {
                    wrapper.Add(" ");
                }

                // Replace the special characters and make the whole document lower case 
                // The _ character will be the tokenizer character (sentence ending)
                document = Regex.Replace(
                    Regex.Replace(Regex.Replace( document.ToLower(), "[.]+[ ]*|[!][ ]*|[?][ ]*", "_"), "[;]|[,]|[:]|[-]|[(]", " "), "[\r][\n]", " "
                );

                // Tokenize the document by the _ character
                string[] tokens = document.Split('_');

                // Loop through the tokens aka. sentences
                foreach (string sentence in tokens)
                {
                    // Remove the unnecessary spaces
                    string current = Regex.Replace(sentence, "^ ", "").Replace("  ", " ");

                    // This will holds the elements
                    List<string> elements = new List<string>();

                    // Tokenize the sentence into words
                    string[] words = current.Split(' ');

                    // Calculate the length of the words
                    int length = words.Length;

                    // Add the beginning wrapper to the elements
                    elements.AddRange(wrapper);

                    // Add the words to the elements
                    foreach (string word in words)
                    {
                        elements.Add(word);
                    }

                    // Add the ending wrapper to the elements
                    elements.AddRange(wrapper);

                    // Loop through the element from the first wrapper to the last word before the ending wrapper 
                    for (int i = 0; i < length + upperIndex; ++i)
                    {
                        string[] currentTupleList = new string[shingleSize];
                        int index = 0;

                        // Make the tuple list
                        for (int j = i; j < i + shingleSize; ++j)
                        {
                            currentTupleList[index++] = elements[j];
                        }

                        // Put the properly made tuple into it's type (head, tail, inner)
                        if (currentTupleList[0].Equals(" "))
                        {
                            if (!elements[i + shingleSize].Equals(" "))
                                head.Add(new Tuple(currentTupleList));
                        }
                        else if (currentTupleList[currentTupleList.Length - 1].Equals(" "))
                        {
                            tail.Add(new Tuple(currentTupleList));
                        }
                        else
                        {
                            inner.Add(new Tuple(currentTupleList));
                        }

                    }
                }
            }
        }

        private class OccurrenceVector
        {
            // This HashMap stores the occurrences of the Tuple (more precisely the Tuple as a List)
            private Dictionary<List<string>, int> occurences;

            // Initialize the HashMap and call the occurrence calculator function
            public OccurrenceVector(TokenizedDocument tok)
            {
                occurences = new Dictionary<List<string>, int>();

                calculateOccurences(tok);
            }

            public Dictionary<List<string>, int> OccurrencesV
            {
                get => occurences;
            }


            // This function calculates the occurrences of a TokenizedDocument
            private void calculateOccurences(TokenizedDocument tok)
            {
                // Get the list of tuples from the TokenizedDocument
                List<Tuple> innerList = tok.InnerList;
                List<Tuple> headList = tok.HeadList;
                List<Tuple> tailList = tok.TailList;


                // Loop through the Tuples and calculate the occurrences
                foreach (Tuple t in innerList)
                {
                    List<string> current = t.Elements;

                    if (!occurences.ContainsKey(current))
                    {
                        occurences.Add(current, 1);
                    }
                    else
                    {
                        occurences[current] = occurences[current] + 1;   
                    }
                }

                foreach (Tuple t in headList)
                {
                    List<string> current = t.Elements;
                    if (!occurences.ContainsKey(current))
                    {
                        occurences.Add(current, 1);
                    }
                    else
                    {
                        occurences[current] = occurences[current] + 1;
                    }
                }

                foreach (Tuple t in tailList)
                {
                    List<string> current = t.Elements;
                    if (!occurences.ContainsKey(current))
                    {
                        occurences.Add(current, 1);
                    }
                    else
                    {
                        occurences[current] = occurences[current] + 1;
                    }
                }
            }

            // Calculate this OccurrenceVector scalar product with the parameterly given Occurrencevector
            public double calculateScalarProduct(OccurrenceVector vecB)
            {
                int result = 0;

                // Get the other HashMap
                Dictionary<List<string>, int> vecBOccurences = vecB.OccurrencesV;

                // Make two sets from the keys
                Dictionary<List<string>, int>.KeyCollection keysA = occurences.Keys;
                Dictionary<List<string>, int>.KeyCollection keysB = vecBOccurences.Keys;

                // Calculate the sum product of the intersection valies
                foreach (List<string> current in keysA)
                {
                    foreach (var other in keysB)
                    {
                        if (other.SequenceEqual(current))
                        {
                            result += occurences[current] * vecBOccurences[other];
                            break;
                        }
                    }
                }

                return result;
            }

            // Getter for the HashMap
            private Dictionary<List<String>, int> getOccurences()
            {
                return occurences;
            }
        }

        public void CalculateCosineSimilarity(string text1, string text2, int shingleSize = 3)
        {
            TokenizedDocument tokens1 = new TokenizedDocument(text1, shingleSize);
            TokenizedDocument tokens2 = new TokenizedDocument(text2, shingleSize);

            OccurrenceVector vectors1 = new OccurrenceVector(tokens1);
            OccurrenceVector vectors2 = new OccurrenceVector(tokens2);

            double result = vectors1.calculateScalarProduct(vectors2) / 
                Math.Sqrt(vectors1.calculateScalarProduct(vectors1) * vectors2.calculateScalarProduct(vectors2));

            Console.WriteLine(result);
        }


    }
}
