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
using CommunityToolkit.WinUI.Controls;
using Windows.Security.Cryptography.Core;
using Windows.UI.Text;

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
                StackPanel.Children.Add(await LinkListItemHelper(link));
            }
        }
    }

    private async Task<SettingsExpander> LinkListItemHelper(Link link)
    {
        Uri icon = new Uri(await new ExternalLinkHelper().GetIconPath(link));
        BitmapImage convertedIcon = new BitmapImage() { UriSource = icon };
        SettingsExpander linkDisplay = new SettingsExpander()
        {
            HeaderIcon = new ImageIcon() { Source = convertedIcon },
            Header = link.Name,
            Description = link.Href,
        };

        SettingsCard type = new SettingsCard()
        {
            Header = "Type",
            Description = "An external link can lead to a specific folder on your computer or a website."
        };
        ComboBox types = new ComboBox();
        types.Items.Add("Link");
        types.Items.Add("Path");
        types.SelectedIndex = link.Type == "link" ? 0 : 1;
        type.Content = types;
        linkDisplay.Items.Add(type);

        SettingsCard name = new SettingsCard()
        {
            Header = "Name",
        };
        TextBox nameTextBox = new TextBox() { Text = link.Name, MinWidth = 200};
        name.Content = nameTextBox;
        linkDisplay.Items.Add(name);

        SettingsCard path = new SettingsCard()
        {
            Header = "Path",
        };
        TextBox pathTextBox = new TextBox() { Text = link.Href, MinWidth = 300 };
        path.Content = pathTextBox;
        linkDisplay.Items.Add(path);

        SettingsCard delete = new SettingsCard()
        {
            Header = "Options",
            Foreground = (SolidColorBrush)Application.Current.Resources["SystemFillColorCriticalBrush"],
            Background = (SolidColorBrush)Application.Current.Resources["SystemFillColorCriticalBackgroundBrush"],
        };
        Button deleteButton = new Button() { Content = "Delete"};
        delete.Content = deleteButton;
        linkDisplay.Items.Add(delete);

        return linkDisplay;
    }
}
