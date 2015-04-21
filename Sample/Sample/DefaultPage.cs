using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample
{
    public class DefaultPage : MasterDetailPage
    {

        public ListView ItemsListView;

        public DefaultPage()
        {

            this.SetBinding(TitleProperty, "Name");

            Label headerLabel = new Label
            {
                Text = "SP Utilities",
                HorizontalOptions = LayoutOptions.Start
            };

            ItemsListView = new ListView
            {
                ItemsSource = new[] { "Home", "Add New", "Contact Us", "Log Out" }
            };
            ItemsListView.ItemSelected += (sender, args) =>
            {
                string itemSelected = args.SelectedItem as string;
                if (String.IsNullOrEmpty(itemSelected))
                    return;
                if (itemSelected.Equals("Home"))
                    Detail = new NavigationPage(new ExistingSitePage()) { Title = "Home" };
                else if (itemSelected.Equals("Add New"))
                    Detail = new NavigationPage(new NewSitePage()) { Title = "New Entry" };
                else if (itemSelected.Equals("Log Out"))
                {
                    
                }

                IsPresented = false;
            };

            MasterBehavior = MasterBehavior.Popover;
            Master = new ContentPage
            {
                Title = headerLabel.Text,
                Content = new StackLayout
                {
                    Children =
                    {
                        headerLabel,
                        ItemsListView
                    }
                }
            };

            Detail = new NavigationPage(new ExistingSitePage()) { Title = "Home" };
        }
    }

    public class ExistingSitePage : ContentPage
    {
        private readonly ObservableCollection<SpUrlsPerUser> listItems;


        public ExistingSitePage()
        {
            listItems = new ObservableCollection<SpUrlsPerUser>();

            LoadSpSites();

            Content = GetSitesListView();
        }

        private bool isLoading;
        private ListView GetSitesListView()
        {
            ListView sitesListView = new ListView { ItemsSource = listItems };
            sitesListView.ItemAppearing += async (sender, e) =>
            {
                if (isLoading || listItems.Count == 0 || listItems.Count > 20)
                    return;
                if (e.Item == listItems[listItems.Count - 1])
                    await LoadSpSites();
            };

            DataTemplate dataTemplate = new DataTemplate(() => new DefaultPageListViewTemplate());
            DefaultPageListViewTemplate defaultPageListViewTemplate =
                dataTemplate.CreateContent() as DefaultPageListViewTemplate;
            if (defaultPageListViewTemplate != null)
            {
                //Do Something here....
            }

            sitesListView.ItemTemplate = dataTemplate;
            sitesListView.ItemTapped += async (sender, args) =>
            {
                SpUrlsPerUser spUrlsPerUser =
                    args.Item as SpUrlsPerUser;
                if (spUrlsPerUser == null || String.IsNullOrEmpty(spUrlsPerUser.Title)) return;

                //Do something here
            };
            return sitesListView;
        }

        private async Task LoadSpSites()
        {
            isLoading = true;

            //List<SpUrlsPerUser> items = new List<SpUrlsPerUser>();
            for (int i = 0; i < 20; i++)
            {
                listItems.Add(new SpUrlsPerUser { ObjectId = i.ToString(), Title = "Hello " + i, Description = "Description " + i });
            }

            isLoading = false;
        }
    }

    public class NewSitePage : ContentPage
    {
        public NewSitePage()
        {
            Content = new StackLayout();
        }
    }
}
