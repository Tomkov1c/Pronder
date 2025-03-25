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
                ConvertSubtasksToObservable(task);
                Tasks.Add(task);
            }
        }
    }
    private void ConvertSubtasksToObservable(TodoTask task)
    {
        if (task.SubTasks != null && task.SubTasks.Count > 0)
        {
            task.VMSubTasks = new ObservableCollection<TodoTask>();

            foreach (var subTask in task.SubTasks)
            {
                ConvertSubtasksToObservable(subTask);
                task.VMSubTasks.Add(subTask);
            }
        }
    }



    private void RemoveTask(TodoTask task)
    {
        Debug.WriteLine("Pressed Remove");
        RemoveTaskRecursive(Tasks, task);
    }

    private void RemoveTaskRecursive(ObservableCollection<TodoTask> taskList, TodoTask taskToRemove)
    {
        var task = taskList.FirstOrDefault(t => t.Id == taskToRemove.Id);
        if (task != null)
        {
            taskList.Remove(task);
            return;
        }

        foreach (var t in taskList)
        {
            if (t.SubTasks != null && t.SubTasks.Count > 0)
            {
                RemoveTaskRecursive(t.VMSubTasks, taskToRemove);
            }
        }
    }

}