using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Pronder.ViewModels;
using Windows.Storage.Streams;
using Windows.Storage;
using Newtonsoft.Json;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Markup;

namespace Pronder.Views;
public sealed partial class SettingsPage : Page
{
    //public SettingsViewModel _viewModel;
    public SettingsPage()
    {
        InitializeComponent();

        Frame.Navigate(typeof(SettingsInterfacePage));
    }
}
