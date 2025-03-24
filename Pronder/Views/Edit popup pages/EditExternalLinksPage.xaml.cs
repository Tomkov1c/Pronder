using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Pronder.Custom;
using Pronder.Models;
using Pronder.ViewModels;
using Windows.ApplicationModel.Calls;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml;
using CommunityToolkit.WinUI.Controls;
using Windows.Security.Cryptography.Core;
using Windows.UI.Text;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.Storage.AccessCache;
using Windows.Storage;
using static Pronder.Models.Project;
using static Pronder.Models.ProjectExtraProperties;

namespace Pronder.Views;

public sealed partial class EditProjectPagesExternalLinksPage : Page
{
    public EditProjectPagesExternalLinksViewModel _viewModel;

    public EditProjectPagesExternalLinksPage()
    {
        _viewModel = new EditProjectPagesExternalLinksViewModel();
        DataContext = _viewModel;
        InitializeComponent();
    }


}

