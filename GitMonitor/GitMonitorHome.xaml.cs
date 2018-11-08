using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Globalization;
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
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("Navigating to Login");
            var item = sender as ListViewItem;
            Console.WriteLine(sender);
            if (item != null && item.IsSelected)
            {
                Console.WriteLine("Doing stuff");
                //Do your stuff
                LoginPage next = new LoginPage();
                myFrame.Navigate(next);
            }
        }

    }
}
