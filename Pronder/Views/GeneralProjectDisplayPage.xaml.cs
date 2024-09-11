using System.Diagnostics;
using EditProjectMenuWorkspace;
using ExternalLinkIconListWorkspace;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using NavigationServiceWorkspace;
using Newtonsoft.Json;
using ProjectWorkspace;
using Pronder.Interfaces.PerPageHelpButtonAction;
using Pronder.ViewModels;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Pronder.Views;

public sealed partial class GeneralProjectDisplayPage : Page, IPerPageHelpButtonAction
{
    public GeneralProjectDisplayViewModel ViewModel
    {
        get;
    }

    int previousSelectedIndex;
    public string path;

    public static event Action OnProjectCreated;
    public GeneralProjectDisplayPage()
    {
        ViewModel = App.GetService<GeneralProjectDisplayViewModel>();
        InitializeComponent();

        var activeItem = NavigationService.Instance.ActiveItem;
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        EditDataContentDialog.OnProjectEdited += refreshData;

        if ((activeItem != null) && (activeItem.Tag.ToString() != null))
        {
            path = activeItem.Tag.ToString();
            getFiles();
        }
    }
    void IPerPageHelpButtonAction.HelpButtonAction(object sender, RoutedEventArgs e)
    {
        ToggleThemeTeachingTip1.IsOpen = true;
    }

    private void TabSwitch(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
    {
        SelectorBarItem selectedItem = sender.SelectedItem;
        int currentSelectedIndex = sender.Items.IndexOf(selectedItem);
        System.Type pageType;

        switch (currentSelectedIndex)
        {
            case 0:
                pageType = typeof(ProjectAboutPage);
                break;
            case 1:
                pageType = typeof(ProjectToDoPage);
                break;
            default:
                pageType = typeof(ProjectAboutPage);
                break;
        }

        // ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });

        var slideNavigationTransitionEffect = currentSelectedIndex - previousSelectedIndex > 0 ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft;

        ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = slideNavigationTransitionEffect });

        previousSelectedIndex = currentSelectedIndex;

    }


    //mine
    public async Task getFiles()
    {
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        string projectFolderName = "Projects";
        StorageFolder projectFolder = await storageFolder.GetFolderAsync(projectFolderName);
        IReadOnlyList<StorageFile> files = await projectFolder.GetFilesAsync();
        int filesCount = files.Count;
        for (int i = 0; i < filesCount; i++)
        {
            if (files[i].Name == path)
            {
                path = files[i].Path.ToString();
                break;
            }
        }

    }

    async void insertBanner(object sender, RoutedEventArgs e)
    {
        Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));
        using (StreamReader file = File.OpenText(path))
        {
            JsonSerializer serializer = new JsonSerializer();
            Project deserialized = (Project)serializer.Deserialize(file, typeof(Project));

            if ((deserialized.Banner != null) && (deserialized.Banner != ""))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(deserialized.Banner);
                ProjectBannerParent.Height = 400;
                ProjectBannerAfter.Margin = new Thickness(0, 20, 0, 0);
                ProjectBanner.Source = bitmapImage;
            }
            else
            {
                ProjectBannerParent.Height = 0;
                ProjectBannerAfter.Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }
    async void insertIcon(object sender, RoutedEventArgs e)
    {
        Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));
        using (StreamReader file = File.OpenText(path))
        {
            JsonSerializer serializer = new JsonSerializer();
            Project deserialized = (Project)serializer.Deserialize(file, typeof(Project));

            if ((deserialized.Icon != null) && (deserialized.Icon != ""))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(deserialized.Icon);
                ProjectIcon.Source = bitmapImage;
            }
            else
            {
                var bitmapImage = new BitmapImage();
                bitmapImage = new BitmapImage(new Uri(base.BaseUri, @"/Assets/Icon8/Color/icons8-project-512.png"));
                ProjectIcon.Source = bitmapImage;
            }
        }
    }
    // ^ it took me a week to make these 2. I'm not lying. UWP wont set source in the insertData() function so this needs to be ran when <Image> is loaded. wtf microsoft

    async void refreshData()
    {
        insertBanner(this, null);
        insertIcon(this, null);
        importData(this, null);
    }

    async void importData(object sender, RoutedEventArgs e)
    {
        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        localSettings.Values["currentlyActiveProject"] = path;

        ProjectExternalLinksInsert.Items.Clear();

        Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));

        using (StreamReader file = File.OpenText(path))
        {
            JsonSerializer serializer = new JsonSerializer();
            Project deserialized = (Project)serializer.Deserialize(file, typeof(Project));

            ProjectTitle.Text = deserialized.Name;
            ProjectTag.Text = deserialized.Tag;

            if (deserialized.Links != null)
            {
                for (int i = 0; i < deserialized.Links.Count; i++)
                {
                    MenuFlyoutItem newItem = new MenuFlyoutItem
                    {
                        Text = deserialized.Links[i].Name,

                    };
                    if (deserialized.Links[i].Type == "link")
                    {
                        StorageFile jsonFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Icon8/Color/Brands/list.json"));

                        using (IRandomAccessStream stream = await jsonFile.OpenAsync(FileAccessMode.Read))
                        using (StreamReader reader = new StreamReader(stream.AsStreamForRead()))
                        {
                            // Read the contents of the file
                            string json = await reader.ReadToEndAsync();

                            // Output the JSON content to debug
                            System.Diagnostics.Debug.WriteLine(json);  // This will output to the debug console

                            // Deserialize the JSON to the object
                            ExternalLinkIconList icons = JsonConvert.DeserializeObject<ExternalLinkIconList>(json);

                            for (int j = 0; icons.Icons.Count > j; j++)
                            {
                                if (deserialized.Links[i].Href.Contains(icons.Icons[j].Contains) || deserialized.Links[i].Name.ToLower() == icons.Icons[j].Contains)
                                {
                                    StorageFile iconFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Icon8/Color/Brands/" + icons.Icons[j].Path));

                                    // Create a BitmapImage from the StorageFile URI
                                    BitmapImage bitmapImage = new BitmapImage(new Uri(iconFile.Path));

                                    // Set the newItem's icon to the BitmapImage
                                    newItem.Icon = new ImageIcon { Source = bitmapImage };
                                    break;

                                }
                                else
                                {
                                    newItem.Icon = new ImageIcon { Source = new BitmapImage(new Uri(base.BaseUri, @"ms-appx:///Assets/Icon8/Color/icons8-website-512.png")), };
                                }
                            }
                            newItem.Click += externalLinksClickLink;
                        }
                    }
                    else if (deserialized.Links[i].Type == "path")
                    {

                        if (deserialized.Links[i].Name == "File Directory")
                        {
                            newItem.Icon = new ImageIcon { Source = new BitmapImage(new Uri(base.BaseUri, @"/Assets/Icon8/Color/icons8-folder-512.png")), };
                        }
                        else
                        {
                            newItem.Icon = new FontIcon { Glyph = "\uE8B7", };
                        }
                        newItem.Click += externalLinksClickPath;
                    }
                    else
                        newItem.Icon = new FontIcon { Glyph = "\uE897", };

                    newItem.Tag = deserialized.Links[i].Href;
                    ProjectExternalLinksInsert.Items.Add(newItem);
                }
            }
            else
            {
                this.ProjectExternalLinks.Visibility = Visibility.Collapsed;
            }
            SubPageTabBarFirst.IsSelected = false;
            SubPageTabBarFirst.IsSelected = true;
        }

        void externalLinksClickLink(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.Tag is string href)
            {
                var uri = new Uri(href);
                var success = Windows.System.Launcher.LaunchUriAsync(uri);
            }
        }
        void externalLinksClickPath(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem && menuItem.Tag is string href)
            {
                Process.Start("explorer.exe", menuItem.Tag.ToString());
            }
        }

    }

    async void deleteProject(object sender, RoutedEventArgs e)
    {
        ContentDialog customDialog = new ContentDialog
        {
            Title = "Delete: " + ProjectTitle.Text,
            PrimaryButtonText = "Yes",
            CloseButtonText = "No",
            DefaultButton = ContentDialogButton.Close
        };
        StackPanel content = new StackPanel();
        content.Children.Add(new TextBlock { Text = "Are you sure you want to delete this project?", Margin = new Thickness(0, 0, 0, 10) });
        customDialog.Content = content;

        customDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;


        customDialog.XamlRoot = this.XamlRoot;
        ContentDialogResult result = await customDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            File.Delete(path);
            OnProjectCreated?.Invoke();
        }
    }

    async void editData(object sender, RoutedEventArgs e)
    {
        ContentDialog editDialog = new EditDataContentDialog();
        editDialog.XamlRoot = this.XamlRoot;
        var result = await editDialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            OnProjectCreated?.Invoke();
        }
    }
}
