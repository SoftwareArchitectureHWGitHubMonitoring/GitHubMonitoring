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
