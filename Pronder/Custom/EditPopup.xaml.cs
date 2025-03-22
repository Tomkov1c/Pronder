using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pronder.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Pronder.Custom;
public sealed partial class EditPopup : UserControl
{
    public static event Action OnProjectEdited;

    XamlRoot xamlRoot;

    public EditPopup(XamlRoot xamlRoot)
    {
        this.xamlRoot = xamlRoot;
        App.MainWindow.SizeChanged += UpdatePopupSize;

        this.InitializeComponent();
        InitializePopup();
    }

    public bool IsPopupOpen()
    {
        return Popup.IsOpen;
    }

    public void OpenPopup()
    {
        Popup.XamlRoot = this.xamlRoot;
        Popup.IsOpen = true;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        OnProjectEdited.Invoke();
        Popup.IsOpen = false;
        FirstNavItem.IsSelected = true;
    }

    private void UpdatePopupSize(object sender, WindowSizeChangedEventArgs e)
    {
        var windowsWidth = Math.Max(App.MainWindow.Bounds.Width, 300);
        var windowsHeight = Math.Max(App.MainWindow.Bounds.Height, 200);
        Popup.Width = windowsWidth;
        Popup.Height = windowsHeight;
        ContentGrid.Width = windowsWidth;
        ContentGrid.Height = windowsHeight;
        ContentGrid.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(71, 0, 0, 0));

        var widthDecrease = 0;
        var heightDecrease = 0;

        switch (windowsWidth)
        {
            case <= 1500:
                widthDecrease = 100;
                break;

            case <= 1700:
                widthDecrease = 150;
                break;

            case <= 1800:
                widthDecrease = 400;
                break;

            default:
                widthDecrease = 500;
                break;
        }

        switch (windowsHeight)
        {
            case <= 900:
                heightDecrease = 50;
                break;

            case <= 1000:
                heightDecrease = 100;
                break;

            default:
                heightDecrease = 200;
                break;
        }

        Border.Height = windowsHeight - heightDecrease;
        Border.Width = windowsWidth - widthDecrease;
        FrameScrollViewer.Height = Border.Height - 72;
        FrameScrollViewer.Width = Border.Width - 200;

    }

    private void InitializePopup()
    {
        UpdatePopupSize(null, null);
        NavView.SelectionChanged += NavSelectionChanged;
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
                _ => null
            };

            if (pageType != null)
            {
                NavigationFrame.Navigate(pageType);
            }
        }
    }
}
