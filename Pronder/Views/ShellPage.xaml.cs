using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

using Pronder.Contracts.Services;
using Pronder.Helpers;
using Pronder.ViewModels;

using Windows.System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

using Newtonsoft.Json;
using Windows.Storage;
using Pronder.Classes;
using Microsoft.UI.Xaml.Media.Imaging;
using ExternalLinkIconListWorkspace;
using Microsoft.UI.Input;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using WinRT.Interop;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Microsoft.UI;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel;
using Windows.UI.WindowManagement;
using Pronder.Interfaces;
using AppWindow = Windows.UI.WindowManagement.AppWindow;
using Microsoft.UI.Xaml.Hosting;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Windows.Services.Maps;

namespace Pronder.Views;

// TODO: Update NavigationViewItem titles and icons in ShellPage.xaml.
public sealed partial class ShellPage : Page
{

    public ShellViewModel ViewModel
    {
        get;
    }

    public ShellPage(ShellViewModel viewModel)
    {
        ViewModel = viewModel;
        InitializeComponent();

        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        localSettings.Values["shellPage_NewProject"] = null;
        NewProjectPage.OnProjectCreated += importProjects;
        GeneralProjectDisplayPage.OnProjectCreated += importProjects;

        CreateDirectoryAsync();
        importProjects();

        NavigationService.Instance.NavigationView = NavigationViewControl;
        NavigationViewControl.ItemInvoked += ItemClicked;

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;

        themeCheck();
    }

    private void OnLoaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);

        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu));
        KeyboardAccelerators.Add(BuildKeyboardAccelerator(VirtualKey.GoBack));
    }

    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        App.AppTitlebar = AppTitleBarText as UIElement;
    }

    private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
    {
        AppTitleBarParent.Margin = new Thickness()
        {
            Left = 48,
            Top = AppTitleBar.Margin.Top,
            Right = AppTitleBar.Margin.Right,
            Bottom = AppTitleBar.Margin.Bottom
        };
    }

    private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
    {
        var keyboardAccelerator = new KeyboardAccelerator() { Key = key };

        if (modifiers.HasValue)
        {
            keyboardAccelerator.Modifiers = modifiers.Value;
        }

        keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;

        return keyboardAccelerator;
    }

    private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        var navigationService = App.GetService<INavigationService>();

        var result = navigationService.GoBack();

        args.Handled = result;
    }


    //min
    void OpenSettingsAcc(object sender, KeyboardAcceleratorInvokedEventArgs e)
    {
        openSettings(null, null);
    }
    void OpenNewProjectAcc(object sender, KeyboardAcceleratorInvokedEventArgs e)
    {
        openNewProject(null, null);
    }
    void OpenActivatePageHelpAcc(object sender, KeyboardAcceleratorInvokedEventArgs e)
    {
        ActivatePageHelp(null, null);
    }
    void OpenAboutAcc(object sender, KeyboardAcceleratorInvokedEventArgs e)
    {
        openAbout(null, null);
    }
    void openSettings(object sender, RoutedEventArgs e)
    {
        NavigationFrame.Navigate(typeof(SettingsPage));
        NavigationViewControl.SelectedItem = null;
    }
    void openNewProject(object sender, RoutedEventArgs e)
    {
        NavigationFrame.Navigate(typeof(NewProjectPage));
        NavigationViewControl.SelectedItem = null;
    }
    void openAbout(object sender, RoutedEventArgs e)
    {
        NavigationFrame.Navigate(typeof(AboutPage));
        NavigationViewControl.SelectedItem = null;
    }
    void ActivatePageHelp(object sender, RoutedEventArgs e)
    {
        var currentPage = NavigationFrame.Content as IPerPageHelpButtonAction;
        currentPage?.HelpButtonAction(null, null);
    }



    void themeCheck()
    {
        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        if (localSettings.Values.ContainsKey("global_ActiveAppTheme"))
        {
            var activeAppTheme = localSettings.Values["global_ActiveAppTheme"]?.ToString();

            if (activeAppTheme == "Light")
            {
                ShellPageName.RequestedTheme = ElementTheme.Light;
            }
            else if (activeAppTheme == "Dark")
            {
                ShellPageName.RequestedTheme = ElementTheme.Dark;
            }
            else if (activeAppTheme == "Default")
            {
                ShellPageName.RequestedTheme = ElementTheme.Default;
            }
        }

    }

    private async void ItemClicked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        NavigationService.Instance.ActiveItem = args.InvokedItemContainer as NavigationViewItem;

        /*
        if (args.InvokedItemContainer.Tag != null)
        {
            string pageTag = args.InvokedItemContainer.Tag.ToString();

            Uri fileUri = new Uri($"ms-appx:///Assets/pagesList.json");
            StorageFile jsonFile = await StorageFile.GetFileFromApplicationUriAsync(fileUri);

            string jsonContent = await FileIO.ReadTextAsync(jsonFile);
                List<PageList> pages = JsonConvert.DeserializeObject<List<PageList>>(jsonContent);

                // Step 2: Find the matching page based on PageID (which is the pageTag here)
                PageList matchingPage = pages.FirstOrDefault(p => p.PageID == pageTag);

                if (matchingPage == null)
                {
                    throw new Exception("Matching page not found in the JSON.");
                }

                // Step 3: Get the Page Type from the assembly based on the ClassName
                // We assume all pages are within the same assembly (the app's main assembly)
                Assembly currentAssembly = typeof(App).GetTypeInfo().Assembly;
                Type pageType = currentAssembly.GetType(matchingPage.ClassName);

                if (pageType == null)
                {
                    throw new Exception($"Page type '{matchingPage.ClassName}' not found.");
                }

                // Step 4: Navigate to the found page
        */
        
                NavigationFrame.Navigate(typeof(GeneralProjectDisplayPage));
        /*}
        */
    }
    public async void CreateDirectoryAsync()
    {
        //C:\Users\gamin\AppData\Local\Packages\90d93993-b7aa-4fff-9757-12ef0c6c27e0_1116rh51nqx02\LocalState

        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        storageFolder.CreateFolderAsync("Projects", Windows.Storage.CreationCollisionOption.FailIfExists);
        storageFolder.CreateFolderAsync("Pages", Windows.Storage.CreationCollisionOption.FailIfExists);


    }


    public async void importProjects()
    {
        try
        {
            var itemsToRemove = new List<NavigationViewItemBase>();

            // Iterate through the primary menu items
            foreach (var item in NavigationViewControl.MenuItems)
            {
                if (item is NavigationViewItem navItem && navItem.Tag != null)
                {
                    itemsToRemove.Add(navItem);
                }
            }

            // Remove the items that don't have a Tag
            foreach (var item in itemsToRemove)
            {
                NavigationViewControl.MenuItems.Remove(item);
            }
        }
        catch (Exception ex) { }
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        string projectFolderName = "Projects";
        StorageFolder projectFolder = await storageFolder.GetFolderAsync(projectFolderName);
        IReadOnlyList<StorageFile> files = await projectFolder.GetFilesAsync();


        for (int i = 0; i < files.Count; i++)
        {
            Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(files[i].Path));

            using (StreamReader file = File.OpenText(files[i].Path))
            {
                JsonSerializer serializer = new JsonSerializer();
                Project deserialized = (Project)serializer.Deserialize(file, typeof(Project));

                var navigationViewItem = new NavigationViewItem
                {
                    Tag = files[i].Path,
                    Content = deserialized.Name
                };

                if ((deserialized.Icon != null) || (deserialized.Icon == ""))
                {
                    BitmapIcon bitmapIcon = new BitmapIcon
                    {
                        // Set the URI of the image
                        UriSource = new Uri(deserialized.Icon),

                        // Optional: Set whether the icon should be shown as monochrome
                        ShowAsMonochrome = false
                    };
                    navigationViewItem.Icon = bitmapIcon;


                }
                else
                {
                    var icon = new ImageIcon
                    {
                        Source = new BitmapImage(new Uri(base.BaseUri, @"/Assets/Icon8/Color/icons8-project-512.png")),
                    };
                    navigationViewItem.Icon = icon;
                }

                NavigationViewControl.MenuItems.Add(navigationViewItem);

            }
        }
        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        if (localSettings.Values["shellPage_NewProject"] != null)
        {
            foreach (var item in NavigationViewControl.MenuItems)
            {
                if (item is NavigationViewItem navItem && navItem.Tag == localSettings.Values["shellPage_NewProject"])
                {
                    NavigationFrame.Navigate(typeof(GeneralProjectDisplayPage));
                }
            }
        }
    }
}
