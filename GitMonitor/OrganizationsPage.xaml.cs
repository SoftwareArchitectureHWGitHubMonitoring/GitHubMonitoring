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
            Newtonsoft.Json.Linq.JArray result = (Newtonsoft.Json.Linq.JArray) JsonConvert.DeserializeObject(content);
            for(int i=0;i<result.Count;i++)
            {
               textbox.Text+=(magic[i].login)+"\n";
            }
        }

    }
}
