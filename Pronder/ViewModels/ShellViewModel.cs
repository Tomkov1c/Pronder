using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Pronder.Classes;
using Pronder.Contracts.Services;
using Pronder.Helpers;
using Pronder.Models;
using Pronder.Views;
using Windows.Storage;

namespace Pronder.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    private NavigationService? _navigationService;
    private NavigationView? _navigationView;
    public ICommand NavigateToPageCommand { get; private set; }

    private SettingsHelper localSettings = new();

    public Action ProjectImported;

    [ObservableProperty] private object? selected;

    public ObservableCollection<object> PaneItems { get; set; } = new();
    private ObservableCollection<object> StaticPages { get; set; } = new();
    private ObservableCollection<ProjectBrief> ProjectPages { get; set; } = new();

    public ShellViewModel(NavigationService? navigationService)
    {
        _navigationService = navigationService;
        NavigateToPageCommand = new RelayCommand<string>(NavigateToPage);
         
        StaticPages.Add(new Pages() { Icon = new SymbolIcon(Symbol.Home), Name = "Home", PageType = typeof(HomePage) });
        StaticPages.Add(new object());

        NewProjectViewModel.OnProjectCreated += () => ImportProjects();
        GeneralProjectDisplayPage.OnProjectDeleted += () => ImportProjects();
    }

    public async Task ImportProjects()
    {
        PaneItems.Clear();
        ProjectPages.Clear();

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
            using (var fileStream = File.OpenRead(item.Path))
            using (var reader = new StreamReader(fileStream))
            {
                string fileContent = await reader.ReadToEndAsync();
                ProjectBrief project = JsonConvert.DeserializeObject<ProjectBrief>(fileContent, settings);
                project.ProjectPath = item.Path;
                project.IfNullOrWhiteSpace();

                ProjectPages.Add(project);
            }
            GC.Collect();
        }

        switch (localSettings.Read("PaneProjectsSorted"))
        {
            case 0:
                ProjectPages = new ObservableCollection<ProjectBrief>(ProjectPages.OrderBy(p => p.Name));
                break;
            case 1:
                ProjectPages = new ObservableCollection<ProjectBrief>(ProjectPages.OrderByDescending(p => p.Name));
                break;
            case 2:
                ProjectPages = new ObservableCollection<ProjectBrief>(ProjectPages.OrderBy(p => p.DateLastViewed));
                break;
            case 3:
                ProjectPages = new ObservableCollection<ProjectBrief>(ProjectPages.OrderByDescending(p => p.DateLastViewed));
                break;
        }

        foreach (var page in StaticPages) PaneItems.Add(page);
        foreach (var project in ProjectPages) PaneItems.Add(project);

    }

    private void NavigateToPage(string? pageName)
    {
        _navigationService.NavigateTo(pageName);
    }
}

public class ProjectBrief
{
    [JsonIgnore] public string ProjectPath { get; set; } = "";

    public string Name { get; set; }
    public string Icon { get; set; } = ((BitmapImage)App.Current.Resources["Icon8Project"]).UriSource.ToString();
    public string DateLastViewed { get; set; }

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