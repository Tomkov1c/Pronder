using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using Pronder.Models;
using Pronder.ViewModels;
using Windows.Storage;

namespace Pronder.Views;

public sealed partial class NewProjectPage : Page
{
    public NewProjectViewModel _viewModel = new();

    public NewProjectPage()
    {
        DataContext = _viewModel;
        InitializeComponent();
    }

    private void CreateClicked(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_viewModel.Name) || string.IsNullOrEmpty(_viewModel.Name)) 
        { 
            InfoBar.IsOpen = true;
            NameTextBox.BorderBrush = Application.Current.Resources["SystemFillColorCriticalBrush"] as SolidColorBrush;
        }
    }
}
