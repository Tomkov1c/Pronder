using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Pronder.Contracts.Services;
using Pronder.Helpers;
using Pronder.ViewModels;
using Windows.System;
using Newtonsoft.Json;
using Windows.Storage;
using Pronder.Classes;
using Microsoft.UI.Xaml.Media.Imaging;
using Pronder.Interfaces;
using Pronder.Models;
using Pronder.Custom;
using System.Diagnostics;

namespace Pronder.Views;
public sealed partial class ShellPage : Page
{
    public ShellViewModel _viewModel;

    public ShellPage(ShellViewModel viewModel)
    {
        _viewModel = viewModel;
        importProjects();
        DataContext = _viewModel;
        InitializeComponent();

        NavigationService.Instance.NavigationView = NavigationViewControl;

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;

        //themeCheck();
    }

    public async void importProjects()
    {
        await _viewModel.ImportProjects();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        TitleBarHelper.UpdateTitleBar(RequestedTheme);
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
        if (args.InvokedItemContainer is NavigationViewItem item && item.Tag != null && !item.IsSelected)
        {
            NavigationFrame.Navigate(typeof(GeneralProjectDisplayPage), item.Tag.ToString());
        }
    }
}