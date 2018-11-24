using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitMonitor.Objects
{
    class Logout
    {

        private static RestClient client = new RestClient("https://api.github.com/");
        private static string clientID = "bdd7da6d9e28bbf58c58";
        private static string clientSecret = "0dd8440e44b8f757d81302321529eb5bdc23047b";

        public static void DeleteToken()
        {
            client.Authenticator = new HttpBasicAuthenticator(clientID, clientSecret);

            var request = new RestRequest("/applications/"+clientID+"/tokens/"+LoginPage.userPassword, Method.DELETE);

            IRestResponse response = client.Execute(request);
        }
    }
}
