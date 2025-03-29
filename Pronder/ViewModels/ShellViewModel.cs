using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Pronder.Contracts.Services;
using Pronder.Models;
using Pronder.Views;
using Windows.Storage;

namespace Pronder.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    [ObservableProperty]
    private object? selected;
    public Action ProjectImported;

    public ObservableCollection<ProjectBrief> PaneItems { get; set; } = new();

    public ShellViewModel()
    {
        ImportProjects();
    }

    public async Task ImportProjects()
    {
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        string projectFolderPath = storageFolder.Path + "\\Projects";

        if (!Directory.Exists(projectFolderPath))
        {
            Directory.CreateDirectory(projectFolderPath);
        }

        var files = Directory.GetFiles(projectFolderPath);

        foreach (var item in files)
        {
            var projectJson = File.ReadAllText(item);
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
            ProjectBrief project = JsonConvert.DeserializeObject<ProjectBrief>(File.ReadAllText(item), settings);
            project.ProjectPath = item;
            project.IfNullOrWhiteSpace();
            Debug.WriteLine(JsonConvert.SerializeObject(project).ToString());

            PaneItems.Add(project);
        }
    }
}

public class ProjectBrief
{
    [JsonIgnore] public string ProjectPath { get; set; } = "";

    public string Name
    {
        get; set;
    }
    public string Icon { get; set; } = ((BitmapImage)App.Current.Resources["Icon8Project"]).UriSource.ToString();

    public void IfNullOrWhiteSpace()
    {
        if (string.IsNullOrWhiteSpace(this.Icon) || string.IsNullOrEmpty(this.Icon))
        {
            this.Icon = ((BitmapImage)App.Current.Resources["Icon8Project"]).UriSource.ToString();
        }
    }
}