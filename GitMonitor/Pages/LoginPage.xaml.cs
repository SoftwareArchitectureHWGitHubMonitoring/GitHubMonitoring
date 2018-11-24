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
        public static String userName = "";
        public static String userPassword = "";
        public static Boolean loggedIn = false;

        private System.Windows.Controls.Button newBtn = new Button();

        public LoginPage()
        {
            InitializeComponent();

            if (loggedIn)
            {
                resultLabel.Text = "Sikeres bejelentkezés!";

                CreateLogoutButton();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (loggedIn)
            {
                loggedIn = false;

                if (File.Exists("Token.txt"))
                {
                    if(TokenClaimer.Instance.DeleteToken()== 0)
                    {
                    File.Delete("Token.txt");
                    }
                }

                // TODO Login Button Remove
                //LoginPanel.Children.Remove((UIElement)this.FindName("LogoutButton"));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!loggedIn)
            {
                var userNameTry = emailField.Text;
                var userPasswordTry = passwordField.Password;

                string[] data = TokenClaimer.Instance.claimToken(userNameTry, userPasswordTry);

                if(data == null)
                {
                    //TODO Error message
                    return;
                }
                string token = data[0];
                string authorizationId = data[1];

                Console.WriteLine(token);

                userName = userNameTry;
                userPassword = token;

                if (!userName.Equals("") || token != null)
                {

                    File.WriteAllText("Token.txt", userName + ":" + userPassword+ ":"+authorizationId);

                    resultLabel.Text = "Sikeres bejelentkezés!";

                    CreateLogoutButton();
                }

                loggedIn = true;
            }
        }

        private void CreateLogoutButton()
        {
            newBtn.Content = "Logout";
            newBtn.Name = "LogoutButton";
            newBtn.Width = 50;
            newBtn.Click += Logout_Click;
            LoginPanel.Children.Add(newBtn);
        }
    }
}
