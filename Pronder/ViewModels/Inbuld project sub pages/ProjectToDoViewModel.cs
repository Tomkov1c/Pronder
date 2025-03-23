using CommunityToolkit.Mvvm.ComponentModel;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class ProjectToDoViewModel : ObservableRecipient
{
    public Project _project;
    public ProjectToDoViewModel()
    {
        _project = Project.GlobalInstance;
    }
}
