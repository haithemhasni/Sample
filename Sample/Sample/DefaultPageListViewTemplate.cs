using System;
using Xamarin.Forms;

namespace Sample
{
    public class DefaultPageListViewTemplate : ViewCell
    {
        public DefaultPageListViewTemplate()
        {
            Grid gridDefaultPage = new Grid
            {
                Padding = new Thickness(5, 10, 0, 0),
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(2, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(10, GridUnitType.Star)}
                },
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                }
            };

            Image favoriteImage = new Image
            {
                //HeightRequest = 42,
                WidthRequest = Device.OnPlatform(55, 55, 75),
                HeightRequest = Device.OnPlatform(55, 55, 75),
                VerticalOptions = LayoutOptions.Center
            };
            favoriteImage.SetBinding(Image.SourceProperty, "Image");
            gridDefaultPage.SetGridLocation(favoriteImage, 0, 0, 0, 2);

            Label siteNameLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = 13
            };
            siteNameLabel.SetBinding(Label.TextProperty, "Title");
            gridDefaultPage.SetGridLocation(siteNameLabel, 0, 1, 0, 0);

            Label siteDescriptionLabel = new Label
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = 11
            };
            siteDescriptionLabel.SetBinding(Label.TextProperty, "Description");
            gridDefaultPage.SetGridLocation(siteDescriptionLabel, 1, 1, 0, 0);

            MenuItem moreAction = new MenuItem {Text = "Update"};
            moreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            moreAction.Clicked += async (sender, e) =>
            {
                MenuItem mi = ((MenuItem) sender);
                SpUrlsPerUser spUrlsPerUser = mi.CommandParameter as SpUrlsPerUser;
                if (spUrlsPerUser == null || String.IsNullOrEmpty(spUrlsPerUser.ObjectId))
                    return;
            };
            MenuItem deleteAction = new MenuItem {Text = "Delete", IsDestructive = true}; // red background
            deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            deleteAction.Clicked += async (sender, e) =>
            {
                MenuItem mi = ((MenuItem) sender);
                SpUrlsPerUser spUrlsPerUser = mi.CommandParameter as SpUrlsPerUser;
                if (spUrlsPerUser == null || String.IsNullOrEmpty(spUrlsPerUser.ObjectId))
                    return;
                RaiseDeleteSpUrlEvent(spUrlsPerUser.ObjectId);
            };

            ContextActions.Add(moreAction);
            ContextActions.Add(deleteAction);
            View = gridDefaultPage;
        }

        private void RaiseDeleteSpUrlEvent(string objectId)
        {
            OnDeleteSpUrlEvent(new DeleteSpUrlEventArgs(objectId));
        }

        public virtual void OnDeleteSpUrlEvent(DeleteSpUrlEventArgs ea)
        {
            if (DeleteSpUrlRaised != null)
                DeleteSpUrlRaised(this, ea);
        }

        public event EventHandler<DeleteSpUrlEventArgs> DeleteSpUrlRaised;
    }

    public static class GridExtensions
    {
        public static void SetGridLocation(this Grid grid, BindableObject control, int rowNumber, int columnNumber,
            int columnSpan, int rowSpan)
        {
            Grid.SetRow(control, rowNumber);
            Grid.SetColumn(control, columnNumber);

            if (columnSpan > 0)
                Grid.SetColumnSpan(control, columnSpan);

            if (rowSpan > 0)
                Grid.SetRowSpan(control, rowSpan);

            grid.Children.Add(control as View);
        }
    }
}