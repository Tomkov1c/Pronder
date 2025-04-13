using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using static Pronder.Models.ProjectExtraProperties;

namespace Pronder.ViewModels;

public partial class EditTodoTaskPopupViewModel : ObservableRecipient
{
    [ObservableProperty] TodoTask? task;
    public EditTodoTaskPopupViewModel(TodoTask? task)
    {
        this.task = task;

    }
}
