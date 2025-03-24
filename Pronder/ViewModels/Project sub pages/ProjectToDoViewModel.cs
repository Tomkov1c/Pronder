using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Pronder.Models;
using static Pronder.Models.ProjectExtraProperties;

namespace Pronder.ViewModels;

public partial class ProjectToDoViewModel : ObservableRecipient
{
    public static Project? _project => Project.GlobalInstance;
    public ObservableCollection<TodoTask> Tasks { get; set; } = new();


    public ICommand RemoveTaskCommand { get; private set; }

    public ProjectToDoViewModel()
    {
        RemoveTaskCommand = new RelayCommand<TodoTask>(RemoveTask);

        if (!_project.TodoTasksNullOrEmpty())
        {
            foreach (var task in _project.TodoTasks)
            {
                Tasks.Add(task);
            }
        }
    }


    private void RemoveTask(TodoTask task)
    {
        Debug.WriteLine("pressed");
        if (task != null)
        {
            Debug.WriteLine("task != null");
                Debug.WriteLine("Removing");
                Tasks.Remove(task);
                Debug.WriteLine(task.ToString());

            foreach (var t in Tasks)
            {
                RemoveTaskFromSubTasks(t, task);
            }
        }
    }

    private void RemoveTaskFromSubTasks(TodoTask parentTask, TodoTask taskToRemove)
    {
        var subTaskToRemove = parentTask.SubTasks.FirstOrDefault(t => t == taskToRemove);
        if (subTaskToRemove != null)
        {
            parentTask.SubTasks.Remove(subTaskToRemove);
        }

        foreach (var subTask in parentTask.SubTasks)
        {
            RemoveTaskFromSubTasks(subTask, taskToRemove);
        }
    }
}