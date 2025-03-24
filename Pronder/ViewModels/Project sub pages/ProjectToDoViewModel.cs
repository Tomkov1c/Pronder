using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Pronder.Models;
using static Pronder.Models.ProjectExtraProperties;

namespace Pronder.ViewModels;

public partial class ProjectToDoViewModel : ObservableRecipient
{
    public static Project? _project => Project.GlobalInstance;
    public ObservableCollection<TodoTask> Tasks { get; set; } = new();

    public ProjectToDoViewModel()
    {
        if (!_project.TodoTasksNullOrEmpty())
        {
            foreach (var task in _project.TodoTasks)
            {
                Tasks.Add(task);
            }
        }
    }
}