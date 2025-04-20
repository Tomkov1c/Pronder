using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Pronder.Custom;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class ProjectToDoViewModel : ObservableRecipient
{
    public static Project? _project => Project.GlobalInstance;
    public ObservableCollection<TodoTask> Tasks { get; set; } = new();

    private EditTodoTaskPopup popup;

    private bool _isDoneImporting = false;

    public ICommand AddTaskCommand { get; private set; }
    public ICommand RemoveTaskCommand { get; private set; }
    public ICommand EditTaskCommand { get; private set; }
    private string _newTaskTitle;
    public string NewTaskTitle
    {
        get => _newTaskTitle;
        set
        {
            if (_newTaskTitle != value)
            {
                _newTaskTitle = value;
                OnPropertyChanged();
            }
        }
    }

    public ProjectToDoViewModel()
    {
        AddTaskCommand = new RelayCommand<TodoTask?>(AddTask);
        RemoveTaskCommand = new RelayCommand<TodoTask?>(RemoveTask);
        EditTaskCommand = new RelayCommand<TodoTask?>(ShowEditPopup);

        if (!_project.TodoTasksNullOrEmpty())
        {
            foreach (var task in _project.TodoTasks)
            {
                ConvertSubtasksToObservable(task);
                Tasks.Add(task);
            }
        }
        Tasks.CollectionChanged += (sender, args) => Save();

        _isDoneImporting = true;
    }
    private void ConvertSubtasksToObservable(TodoTask? task)
    {
        if (task.SubTasks != null && task.SubTasks.Count > 0)
        {
            task.VMSubTasks = new ObservableCollection<TodoTask>();

            foreach (TodoTask? subTask in task.SubTasks)
            {
                ConvertSubtasksToObservable(subTask);
                task.VMSubTasks.Add(subTask);
                task.VMSubTasks.CollectionChanged += (s, e) => Save();
            }
        }
        task.PropertyChanged += (s, e) => Save();
    }

    private void AddTask(TodoTask? task)
    {
        TodoTask newTask = new()
        {
            Content = NewTaskTitle,
        };

        if(task != null)
        {
            task.VMSubTasks.Add(newTask);
        }else
        {
            Tasks.Add(newTask);
        }

        NewTaskTitle = "";
    }
    private void RemoveTask(TodoTask? task)
    {
        RemoveTaskRecursive(Tasks, task);
    }
    private void RemoveTaskRecursive(ObservableCollection<TodoTask>? taskList, TodoTask? taskToRemove)
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
                t.SubTasks = t.VMSubTasks.ToList();
            }
        }
    }
    private async void ShowEditPopup(TodoTask? task)
    {
        popup = new(task);
        await popup.ShowAsync();
    }

    private void Save()
    {
        if (_isDoneImporting)
        {
            _project.TodoTasks = Tasks.ToList();
            _project.SaveToFile();
        }
    }

}