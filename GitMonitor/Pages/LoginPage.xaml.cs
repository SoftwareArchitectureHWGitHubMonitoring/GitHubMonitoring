using GitMonitor.Objects;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static Boolean loggedIn = false;

        public static int counter = 0;

        public LoginPage()
        {
            InitializeComponent();

            if (loggedIn)
                resultLabel.Text = "Sikeres bejelentkezés!";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!loggedIn)
            {
                var userNameTry = emailField.Text;
                var userPasswordTry = passwordField.Password;

                string token = TokenClaimer.Instance.claimToken(userNameTry, userPasswordTry);

                Console.WriteLine(token);

                userName = userNameTry;
                userPassword = token;

                if (!userName.Equals("") || !token.Equals(""))
                {
                    CredentialStorage.addItem(userNameTry, token);

                    File.WriteAllText("Token.txt", userName + ":" + userPassword);

                    resultLabel.Text = "Sikeres bejelentkezés!";
                }

                loggedIn = true;
            }
        }
    }
}
