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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {

        RestClient client = new RestClient("https://api.github.com/");

        //TODO find better solution
        public static String userName="barabali";
        public static String userPassword="asdf";

        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var userNameTry = emailField.Text;
            var userPasswordTry = passwordField.Password;

            var client = new RestClient("https://api.github.com/");
            client.Authenticator = new HttpBasicAuthenticator(userNameTry, userPasswordTry);
            var request = new RestRequest("", Method.GET);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            Console.WriteLine(content);
            if (response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                resultLabel.Text = "Sikeres bejelentkezés!";
                userName = userNameTry;
                userPassword = userPasswordTry;
            }
            else
            {
                resultLabel.Text = "Sikertelen bejelentkezés!";
            }
            
        }
    }
}
