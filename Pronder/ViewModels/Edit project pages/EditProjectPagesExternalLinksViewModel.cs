using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class EditProjectPagesExternalLinksViewModel : ObservableRecipient
{
    public Project _project;
    public string? ProjectPath { get; set; }

    public EditProjectPagesExternalLinksViewModel(string path)
    {
        ProjectPath = path;
        _project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path)) ?? new Project();
    }
}
