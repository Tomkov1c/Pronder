using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Newtonsoft.Json;
using Pronder.Models;
using Pronder.ViewModels;
using Windows.ApplicationModel.Contacts;
using static Pronder.Models.Project;
using static Pronder.Models.ProjectExtraProperties;
namespace Pronder.Views;

public sealed partial class ProjectToDoPage : Page
{
    public ProjectToDoViewModel _viewModel = new();

    public ProjectToDoPage()
    {
        DataContext = _viewModel;
        InitializeComponent();

        _viewModel = null;
        GC.Collect();
    }
}

class TaskTemplateSelector : DataTemplateSelector
{
    public DataTemplate TaskWithoutSubtasks { get; set; }
    public DataTemplate TaskWithSubtasks { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is TodoTask task)
        {
            return (task.SubTasks != null && task.SubTasks.Count > 0) ? TaskWithSubtasks : TaskWithoutSubtasks;
        }
        return TaskWithoutSubtasks;
    }
}
