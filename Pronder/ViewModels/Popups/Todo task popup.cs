using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class EditTodoTaskPopupViewModel : ObservableRecipient
{
    [ObservableProperty] TodoTask? task;
    public EditTodoTaskPopupViewModel(TodoTask? task)
    {
        this.task = task;

    }
}
