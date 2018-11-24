using GitMonitor.Objects;
using LibGit2Sharp;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GitMonitor.Services
{
    class RepositoryService
    {
        static RestClient client = new RestClient("https://api.github.com/");
        static String directory = ConfigManager.get("cloneDirectory");

        public RepositoryService()
        {
           
        }

        public static void cloneOneRepo(String orgname, String reponame)
        {

            //deleteRepoFromDrive(reponame);

            var co = new CloneOptions();
            co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = LoginPage.userName, Password = LoginPage.userPassword };

            try
            {
                Repository.Clone("https://github.com/" + orgname + "/" + reponame + ".git", directory + reponame);
            }  catch(Exception e)
            {
                MessageBox.Show("Error during cloning, message: "+e.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private static void deleteRepoFromDrive(String reponame)
        {
            if (System.IO.Directory.Exists(@"G:\\Work\\Clones\\"+reponame))
            {
                try
                {
                    System.IO.Directory.Delete(@"G:\\Work\\Clones\\" + reponame, true);
                }

                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            /*System.IO.DirectoryInfo di = new DirectoryInfo("G:\\Work\\Clones\\"+reponame);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }*/
        }

        public static List<MyRepository> GetRepositories(String orgname)
        {
            client.Authenticator = new HttpBasicAuthenticator(LoginPage.userName, LoginPage.userPassword);
            var request = new RestRequest("/orgs/" + orgname + "/repos", Method.GET);
            IRestResponse responseDetails = client.Execute(request);

            if (!responseDetails.IsSuccessful)
            {
                MessageBox.Show("Failed to get or no data", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            var contentDetails = responseDetails.Content;
            dynamic magic = JsonConvert.DeserializeObject(contentDetails);

            Console.WriteLine(contentDetails);

            Newtonsoft.Json.Linq.JArray resultDetails = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(contentDetails);
            List<MyRepository> list = new List<MyRepository>();
            for (int j = 0; j < resultDetails.Count; j++)
            {
                MyRepository o = new MyRepository(magic[j].id, magic[j].name, magic[j].owner.login);
                Console.WriteLine(magic[j].name);
                list.Add(o);
            }
            return list;

        }
    }
}
