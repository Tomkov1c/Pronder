using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Pronder.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace Pronder.Custom;
public sealed partial class EditProjectPopup : ContentDialog
{
    public static event Action OnProjectEdited;

    public EditProjectPopup()
    {
        App.MainWindow.SizeChanged += UpdatePopupSize;
        this.XamlRoot = App.MainWindow.Content.XamlRoot;
        this.InitializeComponent();

        UpdatePopupSize(null, null);
    }

    private void UpdatePopupSize(object sender, WindowSizeChangedEventArgs e)
    {
        var windowsWidth = Math.Max(App.MainWindow.Bounds.Width, 300);
        var windowsHeight = Math.Max(App.MainWindow.Bounds.Height, 200);


        MainGrid.Width = windowsWidth - 200;
        MainGrid.Height = windowsHeight - 300;
    }

    private void CloseButton(object sender, RoutedEventArgs e)
    {
        App.MainWindow.SizeChanged -= UpdatePopupSize;
        OnProjectEdited.Invoke();
        Hide();
    }

    private void NavSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        NavigationView nav = sender as NavigationView;

        if (args.SelectedItem is NavigationViewItem selectedItem)
        {
            string pageTag = selectedItem.Content as string;

            Type pageType = pageTag switch
            {
                "General" => typeof(EditProjectPagesGeneralPage),
                "External links" => typeof(EditProjectPagesExternalLinksPage),
                "Budget" => typeof(EditProjectPagesBudgetPage),
                _ => null
            };

            if (pageType != null)
            {
                NavigationFrame.Navigate(pageType, null, new DrillInNavigationTransitionInfo());
            }
        }
    }
}
