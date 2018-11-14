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

        RestClient client = new RestClient("https://api.github.com/");

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
        private string[] scopesArray = { "admin:org","repo" };

        public string[] claimToken(string userName, string userPassword)
        {
            client.Authenticator = new HttpBasicAuthenticator(userName, userPassword);
            var request = new RestRequest("/authorizations/clients/" + clientID, Method.PUT);
            request.AddJsonBody(new { client_secret = clientSecret, scopes = scopesArray });

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            Console.WriteLine(content);

            if (response.IsSuccessful)
            {
            dynamic contentObject = JsonConvert.DeserializeObject(content);
                Console.WriteLine("ID: " + contentObject.id);
                Console.WriteLine("Token: " + contentObject.token);

                string[] data = { contentObject.token, contentObject.id };
                return data;
            }
            else
            {
                return null;
            }
        }

        public int DeleteToken()
        {
            //First get id required for delete
            client.Authenticator = new HttpBasicAuthenticator(LoginPage.userName, LoginPage.userPassword);
            var request = new RestRequest("/authorizations/clients/" + clientID, Method.PUT);
            request.AddJsonBody(new { client_secret = clientSecret});

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            dynamic contentObject = JsonConvert.DeserializeObject(content);

            Console.WriteLine(contentObject);
            string authID = contentObject.id;
            return 1;
            // ERROR Only Basic authentication works, no token...
            var requestDelete = new RestRequest("/authorizations/"+authID, Method.DELETE);
            IRestResponse responseDelete = client.Execute(requestDelete);
            Console.WriteLine(responseDelete);

            return 0;
        }
    }
}
