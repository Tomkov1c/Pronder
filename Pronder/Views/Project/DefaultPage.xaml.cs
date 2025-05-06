using System.Diagnostics;
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
using Microsoft.UI.Xaml.Navigation;
using System.Reflection.Metadata.Ecma335;

namespace Pronder.Views;

public sealed partial class GeneralProjectDisplayPage : Page, IPerPageHelpButtonAction
{

    public GeneralProjectDisplayViewModel _viewModel;

    int previousSelectedIndex;
    public string path;

    public static event Action OnProjectCreated;

    EditProjectPopup editPopup;
    public GeneralProjectDisplayPage()
    {
        InitializeComponent();
        EditProjectPopup.OnProjectEdited += RefreshPage;
    }
    void IPerPageHelpButtonAction.HelpButtonAction(object sender, RoutedEventArgs e)
    {
        ToggleThemeTeachingTip1.IsOpen = !ToggleThemeTeachingTip1.IsOpen;
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is string path)
        {
            this.path = path;
            if (_viewModel == null)
                await InitializeViewModel();

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["currentlyActiveProject"] = path;
        }
    }
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);

        EditProjectPopup.OnProjectEdited -= RefreshPage;
        _viewModel.BackgroundTaskFinished -= PageLoadedBackgroundTasks;

        _viewModel.ClearData();
        _viewModel = null;
        path = null;

        GC.Collect();
    }

    private Task<string> InitializeViewModel()
    {
        DataContext = null;
        _viewModel = new GeneralProjectDisplayViewModel(this.path);
        _viewModel.BackgroundTaskFinished += PageLoadedBackgroundTasks;
        _viewModel.EventsSubscribed();
        DataContext = _viewModel;

        return Task.FromResult("ViewModel Initialized");
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
            case 2:
                pageType = typeof(ProjectBudgetPage);
                break;
            default:
                pageType = typeof(ProjectAboutPage);
                break;
        }

        var slideNavigationTransitionEffect = currentSelectedIndex - previousSelectedIndex > 0 ? SlideNavigationTransitionEffect.FromRight : SlideNavigationTransitionEffect.FromLeft;

        ContentFrame.Navigate(pageType, null, new SlideNavigationTransitionInfo() { Effect = slideNavigationTransitionEffect });

        previousSelectedIndex = currentSelectedIndex;

        if (ContentFrame.BackStack.Count > 0)
        {
            ContentFrame.BackStack.Clear();
            GC.Collect();
        }

    }


    //mine
    private void PageLoaded(object sender, RoutedEventArgs e)
    {
        if (Project.GlobalInstance.LinksNullOrEmpty())
        {
            this.ProjectExternalLinks.Visibility = Visibility.Collapsed;
        }

        if (Project.GlobalInstance.BannerNullOrEmpty())
        {
            ProjectBannerParent.Height = 0;
            ProjectBannerAfter.Margin = new Thickness(0, 0, 0, 0);
        }
        else
        {
            ProjectBannerParent.Height = 400;
            ProjectBannerAfter.Margin = new Thickness(0, 20, 0, 0);
        }

        if (Project.GlobalInstance.IconNullOrEmpty())
        {
            var bitmapImage = new BitmapImage();
            bitmapImage = new BitmapImage(new Uri(base.BaseUri, @"/Assets/Icon8/Color/icons8-project-512.png"));
            ProjectIcon.Source = bitmapImage;
        }
        SubPageTabBarFirst.IsSelected = true;
    }
    private void PageLoadedBackgroundTasks()
    {
        ProjectExternalLinksInsert.Items.Clear();
        foreach (var item in _viewModel.ExternalLinks)
        {
            ProjectExternalLinksInsert.Items.Add(item);
        }

        editPopup = new EditProjectPopup();
    }

    private void RefreshPage()
    {
        InitializeViewModel();
        PageLoaded(null, null);
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
        await editPopup.ShowAsync();
    }
}
