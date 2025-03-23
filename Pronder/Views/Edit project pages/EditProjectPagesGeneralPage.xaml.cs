using System.Diagnostics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Pronder.Models;
using Pronder.ViewModels;
using Windows.ApplicationModel.Calls;
using Windows.Storage.Pickers;

namespace Pronder.Views;

public sealed partial class EditProjectPagesGeneralPage : Page
{
    public EditProjectPagesGeneralViewModel _viewModel;
    private Project project;

    public EditProjectPagesGeneralPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is string path)
        {
            _viewModel = new EditProjectPagesGeneralViewModel(path);
            DataContext = _viewModel;
        }
    }
    private void TextBoxGotFocus(object sender, RoutedEventArgs e)
    {
        TextBox textBox = sender as TextBox;

        textBox.SelectionStart = textBox.Text.Length;
        textBox.SelectionLength = 0;
    }

    private async void SelectAnIcon(object sender, RoutedEventArgs e)
    {
        var senderButton = sender as Button;
        senderButton.IsEnabled = false;

        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.FileTypeFilter.Add(".jpg");
        openPicker.FileTypeFilter.Add(".jpeg");
        openPicker.FileTypeFilter.Add(".png");

        var file = await openPicker.PickSingleFileAsync();
        if (file != null)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(file.Path);
            IconFilePreviewImage.Source = bitmapImage;
            _viewModel.IconPath = file.Path;
            _viewModel.IsIconExpanderExpanded = true;

            IconExpander.IsExpanded = true;
        }

        senderButton.IsEnabled = true;

    }

    private async void SelectABanner(object sender, RoutedEventArgs e)
    {
        var senderButton = sender as Button;
        senderButton.IsEnabled = false;

        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.FileTypeFilter.Add(".jpg");
        openPicker.FileTypeFilter.Add(".jpeg");
        openPicker.FileTypeFilter.Add(".png");

        var file = await openPicker.PickSingleFileAsync();
        if (file != null)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(file.Path);
            BannerFilePreviewImage.Source = bitmapImage;
            _viewModel.BannerPath = file.Path;
            _viewModel.IsBannerExpanderExpanded = true;

            BannerExpander.IsExpanded = true;
        }

        senderButton.IsEnabled = true;

    }

    private void DeleteIcon(object sender, RoutedEventArgs e)
    {
        _viewModel.IconPath = null;
        _viewModel.IsIconExpanderExpanded = false;

        IconFilePreviewImage.Source = null;

        IconExpander.IsExpanded = true;
    }

    private void DeleteBanner(object sender, RoutedEventArgs e)
    {
        _viewModel.BannerPath = null;
        _viewModel.IsBannerExpanderExpanded = false;

        BannerFilePreviewImage.Source = null;

        BannerExpander.IsExpanded = false;
    }
}
