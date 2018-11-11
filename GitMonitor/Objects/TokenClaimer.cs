using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMonitor.Objects
{
    class TokenClaimer
    {

        private static TokenClaimer instance = null;

        private TokenClaimer() {}

        public static TokenClaimer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TokenClaimer();
                }
                return instance;
            }
        }

        private string clientID = "bdd7da6d9e28bbf58c58";
        private string clientSecret = "0dd8440e44b8f757d81302321529eb5bdc23047b";

        public string claimToken(string userName, string userPassword)
        {
            var client = new RestClient("https://api.github.com/");
            client.Authenticator = new HttpBasicAuthenticator(userName, userPassword);
            var request = new RestRequest("/authorizations/clients/" + clientID, Method.PUT);
            request.AddJsonBody(new { client_secret = clientSecret });

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            dynamic contentObject = JsonConvert.DeserializeObject(content);

            Console.WriteLine("ID: " + contentObject.id);
            Console.WriteLine("Token: " + contentObject.token);

            return contentObject.token;
        }
    }
}
