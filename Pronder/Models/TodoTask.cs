using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pronder.Models;
public class TodoTask : INotifyPropertyChanged
{
    [JsonIgnore]
    public Guid Id
    {
        get; private set;
    }

    public TodoTask()
    {
        Id = Guid.NewGuid();
    }

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (_done != value)
            {
                _done = value;
                OnPropertyChanged(nameof(Done));
            }
        }
    }

    private int _order;
    public int Order
    {
        get => _order;
        set
        {
            if (_order != value)
            {
                _order = value;
                OnPropertyChanged(nameof(Order));
            }
        }
    }

    private string _content;
    public string Content
    {
        get => _content;
        set
        {
            if (_content != value)
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
    }

    private List<TodoTask> _subTasks;
    public List<TodoTask> SubTasks
    {
        get => _subTasks;
        set
        {
            if (_subTasks != value)
            {
                _subTasks = value;
                OnPropertyChanged(nameof(SubTasks));
            }
        }
    }

    public bool IsExpanded
    {
        get; set;
    }


    [JsonIgnore]
    public ObservableCollection<TodoTask> VMSubTasks { get; set; } = new ObservableCollection<TodoTask>();

    public bool TodoTasksNullOrEmpty() => SubTasks == null || SubTasks.Count <= 0;

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
