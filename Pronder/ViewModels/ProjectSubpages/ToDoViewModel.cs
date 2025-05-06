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
        task.VMSubTasks = new ObservableCollection<TodoTask>();

        if (task.SubTasks != null && task.SubTasks.Count > 0)
        {
            foreach (TodoTask? subTask in task.SubTasks)
            {
                ConvertSubtasksToObservable(subTask);
                task.VMSubTasks.Add(subTask);
                task.VMSubTasks.CollectionChanged += (s, e) => Save();
            }
        }else
        {
            task.SubTasks = new List<TodoTask>();
            task.VMSubTasks.CollectionChanged += (s, e) => Save();
        }
        task.PropertyChanged += (s, e) => Save();
    }
    private TodoTask? FindTask(ObservableCollection<TodoTask> taskList, TodoTask taskToFind)
    {
        foreach (var task in taskList)
        {
            if (task.Id == taskToFind.Id)
            {
                return task;
            }

            if (task.VMSubTasks != null && task.VMSubTasks.Count > 0)
            {
                var result = FindTask(task.VMSubTasks, taskToFind);
                if (result != null)
                    return result;
            }
        }

        return null;
    }


    private void AddTask(TodoTask? task)
    {
        TodoTask newTask = new()
        {
            Content = !(string.IsNullOrWhiteSpace(NewTaskTitle) && string.IsNullOrEmpty(NewTaskTitle)) ? NewTaskTitle : "New task",
        };

        if (task != null)
        {
            var parentTask = FindTask(Tasks, task);

            if(parentTask.TodoTasksNullOrEmpty())
            {
                parentTask.SubTasks = new();
            }
            bool hasNoSubtasksBefore = parentTask.SubTasks.Count == 0;

            if (parentTask.VMSubTasks == null)
            {
                parentTask.VMSubTasks = new();
            }
            parentTask.VMSubTasks.CollectionChanged += (s, e) => Save();
            parentTask.PropertyChanged += (s, e) => Save();
            parentTask.VMSubTasks.Add(newTask);
            parentTask.SubTasks = parentTask.VMSubTasks.ToList();

            if (hasNoSubtasksBefore)
                RefreshTemplate(parentTask);
        }
        else
        {
            Tasks.Add(newTask);
            _project.TodoTasks = Tasks.ToList();
        }

        NewTaskTitle = "";
    }
    private void RemoveTask(TodoTask? taskToRemove)
    {
        FindAndRemoveTask(taskToRemove, Tasks);
    }
    private void FindAndRemoveTask(TodoTask? taskToRemove, ObservableCollection<TodoTask> taskList)
    {
        if (taskToRemove == null) return;

        var match = taskList.FirstOrDefault(t => t.Id == taskToRemove.Id);
        if (match != null)
        {
            taskList.Remove(match);
            return;
        }

        foreach (var parent in taskList)
        {
            var subtaskMatch = parent.VMSubTasks.FirstOrDefault(t => t.Id == taskToRemove.Id);
            if (subtaskMatch != null)
            {
                parent.VMSubTasks.Remove(subtaskMatch);
                parent.SubTasks = parent.VMSubTasks.ToList();

                if (parent.SubTasks.Count == 0)
                    RefreshTemplate(parent);

                return;
            }

            if (parent.VMSubTasks != null && parent.VMSubTasks.Count > 0)
            {
                FindAndRemoveTask(taskToRemove, parent.VMSubTasks);
                parent.SubTasks = parent.VMSubTasks.ToList();
            }
        }
    }

    private async void ShowEditPopup(TodoTask? task)
    {
        popup = new(task);
        await popup.ShowAsync();
    }

    private void RefreshTemplate(TodoTask taskToRefresh)
    {
        bool TryRefresh(ObservableCollection<TodoTask> list)
        {
            int index = list.IndexOf(taskToRefresh);
            if (index >= 0)
            {
                list.RemoveAt(index);
                ConvertSubtasksToObservable(taskToRefresh);
                list.Insert(index, taskToRefresh);
                return true;
            }

            foreach (var t in list)
            {
                if (t.VMSubTasks != null && t.VMSubTasks.Count > 0)
                {
                    int subIndex = t.VMSubTasks.IndexOf(taskToRefresh);
                    if (subIndex >= 0)
                    {
                        t.VMSubTasks.RemoveAt(subIndex);
                        ConvertSubtasksToObservable(taskToRefresh);
                        t.VMSubTasks.Insert(subIndex, taskToRefresh);
                        t.SubTasks = t.VMSubTasks.ToList();
                        return true;
                    }

                    if (TryRefresh(t.VMSubTasks))
                        return true;
                }
            }

            return false;
        }

        TryRefresh(Tasks);
    }

    private void Save()
    {
        if (_isDoneImporting)
        {
            if(!_project.TodoTasksNullOrEmpty())
            {
                _project.TodoTasks = new List<TodoTask>();
            }
            _project.TodoTasks = Tasks.ToList();
            _project.SaveToFile();
        }
    }

}