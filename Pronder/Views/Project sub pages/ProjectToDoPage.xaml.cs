using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using Pronder.Models;
using Pronder.ViewModels;
using static Pronder.Models.Project;
using static Pronder.Models.ProjectExtraProperties;
namespace Pronder.Views;

public sealed partial class ProjectToDoPage : Page
{
    public ProjectToDoViewModel _viewModel;
    string path;

    public ProjectToDoPage()
    {
        _viewModel = new();
        DataContext = _viewModel;
        InitializeComponent();

        
        foreach(var item in _viewModel.Tasks)
        {
            idk.Children.Add(item);
        }
        
    }
}
