using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using Pronder.Models;
using Pronder.ViewModels;
using static Pronder.Models.Project;
namespace Pronder.Views;

public sealed partial class ProjectToDoPage : Page
{
    public ProjectToDoViewModel _viewModel;
    string path;

    public ProjectToDoPage()
    {
        _viewModel = App.GetService<ProjectToDoViewModel>();
        DataContext = _viewModel;
        InitializeComponent();
    }
}
