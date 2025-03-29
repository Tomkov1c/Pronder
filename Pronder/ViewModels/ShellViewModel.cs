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

    public ObservableCollection<object> PaneItems { get; set; } = new();
    private ObservableCollection<Pages> StaticPages { get; set; } = new();
    private ObservableCollection<ProjectBrief> ProjectPages { get; set; } = new();

    public ShellViewModel()
    {
        StaticPages.Add(new Pages() { Icon = new SymbolIcon(Symbol.Home), Name = "Home" });

        foreach (var page in StaticPages) PaneItems.Add(page);
        PaneItems.Add(new object());
    }

    public async Task ImportProjects()
    {
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        StorageFolder projectFolder;
        try
        {
            projectFolder = await storageFolder.GetFolderAsync("Projects");
        }
        catch (FileNotFoundException)
        {
            projectFolder = await storageFolder.CreateFolderAsync("Projects");
        }
        IReadOnlyList<StorageFile> files = await projectFolder.GetFilesAsync();

        foreach (var item in files)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };
            ProjectBrief project = JsonConvert.DeserializeObject<ProjectBrief>(File.ReadAllText(item.Path), settings);
            project.ProjectPath = item.Path;
            project.IfNullOrWhiteSpace();
            Debug.WriteLine(JsonConvert.SerializeObject(project).ToString());

            ProjectPages.Add(project);
        }
        
        // Sort
        // ProjectPages = new ObservableCollection<ProjectBrief>(ProjectPages.OrderByDescending(p => p.Name));
        
        foreach (var project in ProjectPages) PaneItems.Add(project);
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

public class Pages
{
    public Type PageType { get; set; }
    public string Name { get; set; }
    public IconElement Icon { get; set; }
}