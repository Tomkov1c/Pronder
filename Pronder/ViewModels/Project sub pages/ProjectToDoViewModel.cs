using System.Collections.ObjectModel;
using System.Dynamic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Pronder.Models;
using static Pronder.Models.ProjectExtraProperties;

namespace Pronder.ViewModels;

public partial class ProjectToDoViewModel : ObservableRecipient
{
    public Project _project;
    public List<TextBlock> Tasks { get; set; } = new List<TextBlock>();

    public ProjectToDoViewModel()
    {
        _project = Project.GlobalInstance;


    }
}
