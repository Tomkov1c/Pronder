using System.Diagnostics;
using ExternalLinkIconListWorkspace;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Pronder.Classes;
using Pronder.Models;
using Newtonsoft.Json;
using Pronder.Interfaces;
using Pronder.ViewModels;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using System.Drawing;
using Microsoft.UI;
using Pronder.Custom;
using Pronder.Helpers.Mine;
using Microsoft.UI.Xaml.Navigation;

namespace Pronder.Views;

public sealed partial class GeneralProjectDisplayPage : Page, IPerPageHelpButtonAction
{
    public GeneralProjectDisplayViewModel _viewModel;
    private Project project;

    int previousSelectedIndex;
    public string path;

    public static event Action OnProjectCreated;
    EditProjectPopup editProjectPopup;
    public GeneralProjectDisplayPage()
    {
        InitializeComponent();

        var activeItem = NavigationService.Instance.ActiveItem;
        ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        EditProjectPopup.OnProjectEdited += refreshData;

        if ((activeItem != null) && (activeItem.Tag.ToString() != null))
        {
            path = activeItem.Tag.ToString();
            getFiles();
        }
    }
    void IPerPageHelpButtonAction.HelpButtonAction(object sender, RoutedEventArgs e)
    {
        ToggleThemeTeachingTip1.IsOpen = !ToggleThemeTeachingTip1.IsOpen;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is string path)
        {
            _viewModel = new GeneralProjectDisplayViewModel(path);
            DataContext = _viewModel;

            foreach (var item in _viewModel.ExternalLinks)
            {
                ProjectExternalLinksInsert.Items.Add(item);
            }
        }
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

    async void refreshData()
    {
        
    }

    async void importData(object sender, RoutedEventArgs e)
    {
        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        localSettings.Values["currentlyActiveProject"] = path;

        //ProjectExternalLinksInsert.Items.Clear();

        Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));

        using (StreamReader file = File.OpenText(path))
        {
            JsonSerializer serializer = new JsonSerializer();
            Project deserialized = (Project)serializer.Deserialize(file, typeof(Project));


            if (deserialized.Links != null)
            {
                
            }
            else
            {
                this.ProjectExternalLinks.Visibility = Visibility.Collapsed;
            }
            SubPageTabBarFirst.IsSelected = false;
            SubPageTabBarFirst.IsSelected = true;
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
        if(editProjectPopup != null)
            editProjectPopup.popup.IsOpen = false;

        editProjectPopup = new EditProjectPopup(this.XamlRoot, path);
    }

}
