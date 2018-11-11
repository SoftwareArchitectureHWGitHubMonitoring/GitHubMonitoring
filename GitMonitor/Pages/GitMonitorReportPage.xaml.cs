using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GitMonitor
{
    /// <summary>
    /// Interaction logic for GitMonitorReportPage.xaml
    /// </summary>
    public partial class GitMonitorReportPage : Page
    {
        public GitMonitorReportPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var userName = "barabali";
            var userPassword = "asd";

            var client = new RestClient("https://api.github.com/");
            client.Authenticator = new HttpBasicAuthenticator(userName, userPassword);
            var request = new RestRequest("/user", Method.GET);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            Console.WriteLine(content);
            textbox.Text = content;

            var request2 = new RestRequest("/users/barabali/repos?page=1", Method.GET);

            // execute the request
            IRestResponse response2 = client.Execute(request2);
            var content2 = response2.Content; // raw content as string
            Console.WriteLine(content2);

            // View Expense Report
            // NavigationWindow window = new NavigationWindow();
            // window.Source = new Uri("GitMonitorReportPage.xaml", UriKind.Relative);
            //window.Show();
        }
    }
}
