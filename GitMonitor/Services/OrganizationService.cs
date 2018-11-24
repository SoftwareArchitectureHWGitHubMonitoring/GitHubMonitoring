using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GitMonitor.Services
{
    class OrganizationService
    {
        static RestClient client = new RestClient("https://api.github.com/");


        public static dynamic getOrganizations()
        {
            client.Authenticator = new HttpBasicAuthenticator(LoginPage.userName, LoginPage.userPassword);

            var request = new RestRequest("/user/orgs", Method.GET);

            // execute the request
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
