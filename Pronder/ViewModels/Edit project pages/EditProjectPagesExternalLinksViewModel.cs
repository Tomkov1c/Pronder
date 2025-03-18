using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class EditProjectPagesExternalLinksViewModel : ObservableRecipient
{
    public ObservableCollection<Link> Links
    {
        get; set;
    }
    public Project _project;
    public string? ProjectPath
    {
        get; set;
    }
    public Project Project
    {
        get => _project;
        set
        {
            _project = value;
            OnPropertyChanged(nameof(Project));
        }
    }

    public EditProjectPagesExternalLinksViewModel(string path)
    {
        Links = new ObservableCollection<Link>();
        Project = new Project();

        ProjectPath = path;
        _project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path)) ?? new Project();
    }

    public void AddNewLink(string name, string href)
    {
        Link link = new Link()
        {
            Name = name,
            Href = href,
            Type = "link"
        };
        _project.Links.Add(link);
        SaveData();
    }

    public void AddNewPath(string name, string href)
    {
        Link path = new Link()
        {
            Name = name,
            Href = href,
            Type = "path"
        };
        _project.Links.Add(path);
        SaveData();
    }

    private void SaveData()
    {
        if (!string.IsNullOrEmpty(ProjectPath))
        {
            File.WriteAllText(ProjectPath, JsonConvert.SerializeObject(_project, Formatting.Indented));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
