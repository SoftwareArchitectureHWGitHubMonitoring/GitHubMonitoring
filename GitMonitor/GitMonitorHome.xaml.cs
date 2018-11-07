using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net.Http;
using System.Windows;

namespace GitMonitor
{
    /// <summary>
    /// Interaction logic for GitMonitorHome.xaml
    /// </summary>
    public partial class GitMonitorHome : System.Windows.Controls.Page
    {
        private static readonly HttpClient client = new HttpClient();

        public GitMonitorHome()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var userName = "";
            var userPassword = "";

            var client = new RestClient("https://api.github.com/");
            client.Authenticator = new HttpBasicAuthenticator(userName, userPassword);
            var request = new RestRequest("/user", Method.GET);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            Console.WriteLine(content);

            var request2 = new RestRequest("/users/burjandedes/repos?page=1", Method.GET);

            // execute the request
            IRestResponse response2 = client.Execute(request2);
            var content2 = response2.Content; // raw content as string
            Console.WriteLine(content2);

            // View Expense Report
            GitMonitorReportPage expenseReportPage = new GitMonitorReportPage(this.peopleListBox.SelectedItem);
            this.NavigationService.Navigate(expenseReportPage);
        }
    }
}
