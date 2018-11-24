using GitMonitor.Objects;
using GitMonitor.Pages;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GitMonitor
{
    /// <summary>
    /// Interaction logic for GitMonitorHome.xaml
    /// </summary>
    public partial class GitMonitorHome : Window
    {

        public GitMonitorHome()
        {
            InitializeComponent();
            CosineSimilarityCalculator c = new CosineSimilarityCalculator();
            c.CalculateCosineSimilarity("aa bb cc","aa bb dd");
            ReadCredentials();
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        }

        public void ReadCredentials()
        {
            if (File.Exists("Token.txt"))
            {
                string credentials = File.ReadAllText("Token.txt");
                Console.WriteLine(credentials);

                string[] tokenized = credentials.Split(':');

                if (tokenized.Length == 3)
                {
                    LoginPage.userName = tokenized[0];
                    LoginPage.userPassword = tokenized[1];
                    LoginPage.loggedIn = true;

                }
            }

            //Load config file and configs
            if (File.Exists("../../config.txt"))
            {
                System.Collections.Generic.IEnumerable<String> lines = File.ReadLines("../../config.txt");
                foreach (String item in lines)
                {
                    string[] words = item.Split('=');
                    ConfigManager.addItem(words[0],words[1]);
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Missing config file!", "ErRrOR", MessageBoxButton.OK);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        System.Windows.Application.Current.Shutdown();
                        break;
                }
            }
        }

            private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                Page next;

                //Trigger event to close side menu
                ListViewItem btn = sender as ListViewItem;
                Storyboard myStoryboard = btn.TryFindResource("CloseMenu") as Storyboard;
                myStoryboard.Begin(btn);

                switch (item.Name)
                {
                    case "loginNavigation":
                        next = new LoginPage();
                        myFrame.Navigate(next);
                        break;
                    case "organizationsNavigation":
                        next = new OrganizationsPage();
                        myFrame.Navigate(next);
                        break;
                    case "cloningNavigation":
                        next = new CloneAndCommit();
                        myFrame.Navigate(next);
                        break;
                }

            }
        }

    }
}
