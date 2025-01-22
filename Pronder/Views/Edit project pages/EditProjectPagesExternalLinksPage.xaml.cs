using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Pronder.Custom;
using Pronder.Helpers.Mine;
using Pronder.Models;
using Pronder.ViewModels;
using Windows.ApplicationModel.Calls;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml;

namespace Pronder.Views;

public sealed partial class EditProjectPagesExternalLinksPage : Page
{
    public EditProjectPagesExternalLinksViewModel _viewModel;

    public EditProjectPagesExternalLinksPage()
    {
        InitializeComponent();
    }
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is string path)
        {
            _viewModel = new EditProjectPagesExternalLinksViewModel(path);
            DataContext = _viewModel;

            List<Link> links = _viewModel._project.Links;

            if(links != null)
            foreach (Link link in links)
            {
                var grid = new Grid
                {
                    Background = (SolidColorBrush)Application.Current.Resources["CardBackgroundFillColorDefaultBrush"],
                    Padding = new Thickness(16),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    CornerRadius = (CornerRadius)Application.Current.Resources["ControlCornerRadius"],
                    BorderBrush = (SolidColorBrush)Application.Current.Resources["CardStrokeColorDefaultBrush"],
                    BorderThickness = new Thickness(1),
                    ColumnSpacing = 16
                };

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(24) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(32) });

                var image = new Image
                {
                    Source = new BitmapImage(new Uri(await new ExternalLinkHelper().GetIconPath(link))),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(image, 0);

                var nameTextBox = new TextBox
                {
                    Text = link.Name,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(nameTextBox, 1);

                var pathTextBox = new TextBox
                {
                    Text = link.Href,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(pathTextBox, 2);

                var button = new Button
                {
                    Content = "..",
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(button, 3);

                grid.Children.Add(image);
                grid.Children.Add(nameTextBox);
                grid.Children.Add(pathTextBox);
                grid.Children.Add(button);


                await DispatcherQueue.EnqueueAsync(() => StackPanel.Children.Add(grid));
            }
        }
    }
}
