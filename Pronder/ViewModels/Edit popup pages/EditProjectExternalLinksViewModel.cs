using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class EditProjectPagesExternalLinksViewModel : ObservableRecipient
{
    public static Project? _project => Project.GlobalInstance;

    public ObservableCollection<Link> Links
    {
        get; set;
    }

    public EditProjectPagesExternalLinksViewModel()
    {
        Links = new ObservableCollection<Link>();
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
        Project.GlobalInstance.SaveToFile();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
