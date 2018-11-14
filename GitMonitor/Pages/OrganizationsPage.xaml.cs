using GitMonitor.Objects;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace GitMonitor
{
    /// <summary>
    /// Interaction logic for OrganizationsPage.xaml
    /// </summary>
    public partial class OrganizationsPage : Page
    {
        public OrganizationsPage()
        {
            InitializeComponent();
            client.Authenticator = new HttpBasicAuthenticator(LoginPage.userName, LoginPage.userPassword);

            //Add roles for members
            List<MyComboBoxItem> roles = new List<MyComboBoxItem>
            {
                new MyComboBoxItem("member"),
                new MyComboBoxItem("admin")
            };
            userRole.ItemsSource = roles;
            userRole.SelectedIndex = 0;
        }

        RestClient client = new RestClient("https://api.github.com/");

        /// <summary>
        /// Gets the list of Organizations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var request = new RestRequest("/user/orgs", Method.GET);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            dynamic magic = JsonConvert.DeserializeObject(content);
            Newtonsoft.Json.Linq.JArray result = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(content);

            List<Organization> list = new List<Organization>();
            List<MyComboBoxItem> orgList = new List<MyComboBoxItem>();

            for (int i = 0; i < result.Count; i++)
            {
                Organization o = new Organization(magic[i].id, magic[i].login, magic[i].description);
                list.Add(o);
                orgList.Add(new MyComboBoxItem((String)magic[i].login));
            }
            organizationsGrid.ItemsSource = list;
            selectOrganization.ItemsSource = orgList;
            selectOrganization.SelectedIndex = 0;
        }

        /// <summary>
        /// Gets list of users for selected organization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Show_Users(object sender, RoutedEventArgs e)
        {
            if (selectOrganization.SelectedItem == null)
            {
                return;
            }
            var requestDetails = new RestRequest("orgs/" + ((MyComboBoxItem)selectOrganization.SelectedItem).Name + "/members", Method.GET);
            IRestResponse responseDetails = client.Execute(requestDetails);

            var contentDetails = responseDetails.Content;
            dynamic magicDetails = JsonConvert.DeserializeObject(contentDetails);

            Newtonsoft.Json.Linq.JArray resultDetails = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(contentDetails);
            for (int j = 0; j < resultDetails.Count; j++)
            {
                List<User> list = new List<User>();

                for (int i = 0; i < resultDetails.Count; i++)
                {
                    User o = new User(magicDetails[i].id, magicDetails[i].login);
                    list.Add(o);
                }
                usersGrid.ItemsSource = list;
            }
            
        }

        /// <summary>
        /// Invites a user to the organization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Member(object sender, RoutedEventArgs e)
        {
            if (selectOrganization.SelectedItem == null || usernameField.Text.Length == 0)
            {
                return;
            }
            var requestDetails = new RestRequest("orgs/" + ((MyComboBoxItem)selectOrganization.SelectedItem).Name + "/memberships/" + usernameField.Text, Method.PUT);
            requestDetails.AddJsonBody(new { role = "admin" });
            IRestResponse responseDetails = client.Execute(requestDetails);
            var contentDetails = responseDetails.Content;
            dynamic magic = JsonConvert.DeserializeObject(contentDetails);
            var state = magic.state;
            Console.WriteLine(responseDetails);
        }

        /// <summary>
        /// Deletes or revokes invitations from an organization member
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete_Member(object sender, RoutedEventArgs e)
        {
            if (selectOrganization.SelectedItem == null || usernameField.Text.Length == 0)
            {
                return;
            }
            var requestDetails = new RestRequest("orgs/" + ((MyComboBoxItem)selectOrganization.SelectedItem).Name + "/memberships/" + usernameField.Text, Method.DELETE);
            IRestResponse responseDetails = client.Execute(requestDetails);
            if(responseDetails.StatusCode.Equals(System.Net.HttpStatusCode.NotFound))
            {
                MessageBox.Show("Not found user like that", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //ok
                MessageBox.Show("Successfully deleted", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Shows repositiories in the organization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Show_Repos(object sender, RoutedEventArgs e)
        {
            if (selectOrganization.SelectedItem == null)
            {
                return;
            }
            var request = new RestRequest("/orgs/" + ((MyComboBoxItem)selectOrganization.SelectedItem).Name + "/repos", Method.GET);
            IRestResponse responseDetails = client.Execute(request);

            var contentDetails = responseDetails.Content;
            dynamic magic = JsonConvert.DeserializeObject(contentDetails);

            Console.WriteLine(contentDetails);
            if (responseDetails.StatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                Newtonsoft.Json.Linq.JArray resultDetails = (Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(contentDetails);
                List<Repository> list = new List<Repository>();
                for (int j = 0; j < resultDetails.Count; j++)
                {
                    Repository o = new Repository(magic[j].id,magic[j].name,magic[j].owner.login);
                    Console.WriteLine(magic[j].name);
                    list.Add(o);
                }
                reposGrid.ItemsSource = list;
            }
            else
            {
                MessageBox.Show("Failed to get or no repos", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Creates a repository, owner is the organization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Create_Repos(object sender, RoutedEventArgs e)
        {
            if (selectOrganization.SelectedItem == null || reponameField.Text.Length==0)
            {
                return;
            }
            var request = new RestRequest("/orgs/" + ((MyComboBoxItem)selectOrganization.SelectedItem).Name + "/repos", Method.POST);
            request.AddJsonBody(new {name= reponameField.Text});
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            if (response.IsSuccessful)
            {
                Console.WriteLine(content);
                Delayed_repo_show();
            }
            else
            {
                MessageBox.Show("Failed to get or no repos", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Deletes a repo belonging to the organization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Delete_Repo(object sender, RoutedEventArgs e)
        {
            if (reposGrid.SelectedItem == null)
            {
                return;
            }

            var request = new RestRequest("/repos/" + ((MyComboBoxItem)selectOrganization.SelectedItem).Name+ "/" + ((Repository)reposGrid.SelectedItem).Name, Method.DELETE);
            IRestResponse response = client.Execute(request);

            var content = response.Content;

            Console.WriteLine(content);
            
            if (response.IsSuccessful)
            {
                Delayed_repo_show();
            }
            else
            {
                MessageBox.Show("Failed to delete repo", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        //Delayed method to get refreshed repo list
        private async void Delayed_repo_show()
        {
            await Task.Delay(2000);
            Button_Show_Repos(null, null);
        }
    }
}
