using GitMonitor.Objects;
using GitMonitor.Services;
using LibGit2Sharp;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
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

namespace GitMonitor.Pages
{
    /// <summary>
    /// Interaction logic for CloneAndCommit.xaml
    /// </summary>
    public partial class CloneAndCommit : Page
    {
        public CloneAndCommit()
        {
            InitializeComponent();
            InitializeComboBox();
        }

        static String directory = ConfigManager.get("cloneDirectory");

        private void Button_Clone_Repos(object sender, RoutedEventArgs e)
        {
            if (selectOrganization.SelectedItem == null)
            {
                return;
            }
            String orgname = ((MyComboBoxItem)selectOrganization.SelectedItem).Name;
            List<MyRepository> list = RepositoryService.GetRepositories(orgname);

            foreach (MyRepository repo in list)
            {
                RepositoryService.cloneOneRepo(orgname,repo.Name);
            }

            if (ConfigManager.get("showDir").Equals("true"))
            {
                Process.Start("explorer.exe", "/open, "+directory);
            }
        }

        public void InitializeComboBox()
        {
            IRestResponse response = OrganizationService.getOrganizations();
            if (!response.IsSuccessful)
            {
                MessageBox.Show("Failed to get or no data", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var content = response.Content;
            dynamic magic = JsonConvert.DeserializeObject(content);
            Newtonsoft.Json.Linq.JArray result = (Newtonsoft.Json.Linq.JArray)magic;
            List<MyComboBoxItem> orgList = new List<MyComboBoxItem>();
            for (int i = 0; i < result.Count; i++)
            {
                Organization o = new Organization(magic[i].id, magic[i].login, magic[i].description);
                orgList.Add(new MyComboBoxItem((String)magic[i].login));
            }
            selectOrganization.ItemsSource = orgList;
            selectOrganization.SelectedIndex = 0;
        }

        private void Button_Select(object sender, RoutedEventArgs e)
        {
            selectFiles();
        }

        public void selectFiles()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                Console.WriteLine(filename);
            }
        }
    }
}
