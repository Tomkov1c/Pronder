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
using Microsoft.UI.Xaml.Media.Animation;

namespace Pronder.Views;
public sealed partial class ShellPage : Page
{
    public ShellViewModel _viewModel = new();

    public ShellPage()
    {
        importProjects();
        InitializeComponent();

        NavigationService.Instance.NavigationView = NavigationViewControl;

        App.MainWindow.ExtendsContentIntoTitleBar = true;
        App.MainWindow.SetTitleBar(AppTitleBar);
        App.MainWindow.Activated += MainWindow_Activated;

        SettingsInterfaceViewModel.OnOrderChanged += importProjects;

        //themeCheck();
    }

    public async void importProjects()
    {
        DataContext = null;
        await _viewModel.ImportProjects();
        DataContext = _viewModel;
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

    private void ItemClicked(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (sender.Tag == args.SelectedItem)
        {
            return;
        }

        if (args.SelectedItem is ProjectBrief project && !string.IsNullOrEmpty(project.ProjectPath))
        {
            NavigationFrame.Navigate(typeof(GeneralProjectDisplayPage), project.ProjectPath, new EntranceNavigationTransitionInfo());
        }
        else if (args.SelectedItem is Pages page && page.PageType != null)
        {
            NavigationFrame.Navigate(page.PageType, null, new EntranceNavigationTransitionInfo());
        }
        sender.Tag = args.SelectedItem;
        if (NavigationFrame.BackStack.Count > 0)
        {
            NavigationFrame.BackStack.Clear();
        }
    }
}

class ItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate GlyphTemplate { get; set; }
    public DataTemplate IconTemplate { get; set; }

    public DataTemplate SeparatorTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is Pages)
        {
            return GlyphTemplate;
        }
        else if(item is ProjectBrief)
        {
            return IconTemplate;
        }
        else
        {
            return SeparatorTemplate;
        }
    }
}