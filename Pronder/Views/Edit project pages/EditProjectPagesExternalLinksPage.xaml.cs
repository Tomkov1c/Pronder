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
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.Storage.AccessCache;
using Windows.Storage;

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

            LoadData();
        }
    }


    private void NewLinkClicked(object sender, RoutedEventArgs e)
    {
        _viewModel.AddNewLink(NameTextBox.Text, LinkTextBox.Text);

        NameTextBox.Text = string.Empty;
        LinkTextBox.Text = string.Empty;

        LinkFlyout.Hide();
        LoadData();
    }

    private void NewPathClicked(object sender, RoutedEventArgs e)
    {
        _viewModel.AddNewPath(NameTextBox2.Text, PathTextBox.Text);

        NameTextBox2.Text = string.Empty;
        PathTextBox.Text = string.Empty;

        DirectoryFlyout.Hide();
        LoadData();
    }



    void SaveData()
    {
        StackPanel.Children.Clear();
        if (!string.IsNullOrEmpty(_viewModel.ProjectPath))
        {
            File.WriteAllText(_viewModel.ProjectPath, JsonConvert.SerializeObject(_viewModel._project, Formatting.Indented));

            LoadData();
        }
    }


    async void LoadData()
    {
        StackPanel.Children.Clear();
        List<Link> links = _viewModel._project.Links;

        if (links != null)
        foreach (Link link in links)
        {
            StackPanel.Children.Add(await LinkListItemHelper(link));
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

        SettingsCard name = new SettingsCard()
        {
            Header = "Name",
        };
        TextBox nameTextBox = new TextBox() { Text = link.Name, MinWidth = 200 };
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

        Button deleteButton = new Button() { Content = "Delete" };
        deleteButton.Tag = link;
        deleteButton.Click += DeleteButtonClick;
        delete.Content = deleteButton;
        linkDisplay.Items.Add(delete);

        return linkDisplay;
    }


    private void DeleteButtonClick(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        if (button != null)
        {
            Link linkToDelete = button.Tag as Link;

            if (linkToDelete != null)
            {
                _viewModel._project.Links.Remove(linkToDelete);
                SaveData();
            }
        }
    }

    private async void OpenFileDialog(object sender, RoutedEventArgs e)
    {
        var senderButton = sender as Button;
        senderButton.IsEnabled = false;

        FolderPicker openPicker = new Windows.Storage.Pickers.FolderPicker();

        var window = App.MainWindow;

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");

        StorageFolder folder = await openPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            PathTextBox.Text = folder.Path;
        }

        senderButton.IsEnabled = true;

    }
}

