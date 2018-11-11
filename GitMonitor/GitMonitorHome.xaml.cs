﻿using GitMonitor.Objects;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

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

                if (tokenized.Length == 2)
                {
                    CredentialStorage.addItem(tokenized[0], tokenized[1]);
                    LoginPage.userName = tokenized[0];
                    LoginPage.userPassword = tokenized[1];
                    LoginPage.loggedIn = true;

                }


            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                Page next;
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
                }

            }
        }

    }
}
