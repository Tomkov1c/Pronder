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

public sealed partial class EditProjectPagesBudgetPage : Page
{
    public EditProjectPagesBudgetViewModel _viewModel = new();

    public EditProjectPagesBudgetPage()
    {
        DataContext = _viewModel;
        InitializeComponent();
    }
}
