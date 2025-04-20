using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Newtonsoft.Json;
using Pronder.Custom;
using Pronder.Models;
using Pronder.ViewModels;
using Windows.ApplicationModel.Contacts;
using static Pronder.Models.Project;
namespace Pronder.Views;

public sealed partial class ProjectToDoPage : Page
{
    public ProjectToDoViewModel _viewModel;

    public ProjectToDoPage()
    {
        ReloadDataContect();
        InitializeComponent();

        _viewModel = null;
        GC.Collect();
    }

    private void ReloadDataContect()
    {
        _viewModel = new();
        DataContext = _viewModel;
    }
}

class TaskTemplateSelector : DataTemplateSelector
{
    public DataTemplate TaskWithoutSubtasks { get; set; }
    public DataTemplate TaskWithSubtasks { get; set; }
    public DataTemplate MainTaskTemplate { get; set; }
    public DataTemplate MainTaskTemplateWithSubtasks { get; set; }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        if (item is TodoTask task)
        {
            var parent = ItemsControl.ItemsControlFromItemContainer(container);
            
            if (parent is ListView listView && listView.Name == "MainListView")
            {
                if(!task.TodoTasksNullOrEmpty())
                {
                    return MainTaskTemplateWithSubtasks;
                }
                return MainTaskTemplate;
            }

            return (task.SubTasks != null && task.SubTasks.Count > 0)
                ? TaskWithSubtasks
                : TaskWithoutSubtasks;
        }

        return base.SelectTemplateCore(item, container);
    }
}

