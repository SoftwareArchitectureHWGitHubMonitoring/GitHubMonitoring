using GitMonitor.Objects;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GitMonitor
{
    /// <summary>
    /// Interaction logic for OrganizationsWindow.xaml
    /// </summary>
    public partial class OrganizationsPage : Page
    {
        public OrganizationsPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient("https://api.github.com/");
            client.Authenticator = new HttpBasicAuthenticator(LoginPage.userName, LoginPage.userPassword);
            var request = new RestRequest("user/orgs", Method.GET);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            dynamic magic = JsonConvert.DeserializeObject(content);
            Newtonsoft.Json.Linq.JArray result = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(content);

            List<Organization> list = new List<Organization>();

            for (int i = 0; i < result.Count; i++)
            {
                Organization o = new Organization(magic[i].id, magic[i].login, magic[i].description);
                list.Add(o);
                textbox.Text += (magic[i].login) + "\n";

                var requestDetails = new RestRequest("orgs/" + magic[i].login + "/members", Method.GET);

                IRestResponse responseDetails = client.Execute(requestDetails);
                var contentDetails = responseDetails.Content; 
                dynamic magicDetails = JsonConvert.DeserializeObject(contentDetails);
                Newtonsoft.Json.Linq.JArray resultDetails = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(contentDetails);
                for(int j = 0; j < resultDetails.Count; j++)
                {
                    textbox.Text += ("\t"+magicDetails[j].login) + "\n";
                }

            }
            organizationsGrid.ItemsSource = list;
         
        }

    }
}
