using GitMonitor.Objects;
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

        //TODO find better solution
        public static String userName = "barabali";
        public static String userPassword = "asdf";

        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var userNameTry = emailField.Text;
            var userPasswordTry = passwordField.Password;

            string token = TokenClaimer.Instance.claimToken(userNameTry, userPasswordTry);

            Console.WriteLine(token);

            userName = userNameTry;
            userPassword = token;

            CredentialStorage.addItem(userNameTry, token);

        }
    }
}
