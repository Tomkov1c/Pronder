using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pronder.Models;
using static Pronder.Models.Project;

namespace Pronder.ViewModels;

public partial class GeneralProjectDisplayViewModel : ObservableRecipient
{
    public event Action MainTaskFinished;
    public event Action BackgroundTaskFinished;

    private string projectPath;
    public Project? _project;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _description;

    [ObservableProperty]
    private string? _tag;

    [ObservableProperty]
    private string? _iconPath;

    [ObservableProperty]
    private string? _bannerPath;

    public ObservableCollection<MenuFlyoutItem> ExternalLinks = new();

    public GeneralProjectDisplayViewModel(string path, bool instantStart = false)
    {
        projectPath = path;
        if(instantStart)
        {
            EventsSubscribed();
        }
    }

    public void EventsSubscribed()
    {

        InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await MainTasksAsync();
        BackgroundTasksAsync();
    }

    private async Task MainTasksAsync()
    {
        Project.RemoveGlobalInstance();
        _project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(projectPath));
        Project.SetGlobalInstance(_project);
        Project.SetGlobalString(projectPath);

        _project.DateLastViewed = DateTime.Now.ToString();

        _name = _project.Name;
        _description = _project.About;
        _tag = _project.Tag;
        _iconPath = string.IsNullOrEmpty(_project.Icon) ? "" : _project.Icon;
        _bannerPath = string.IsNullOrEmpty(_project.Banner) ? null : _project.Banner;

        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName is nameof(Name) or nameof(Description) or nameof(Tag) or nameof(IconPath) or nameof(BannerPath))
            {

            }
        };

        MainTaskFinished?.Invoke();
    }

    private void BackgroundTasksAsync()
    {
        if (!_project.LinksNullOrEmpty())
        {
            foreach (Link item in _project.Links)
            {
                MenuFlyoutItem menuItem = new();
                menuItem.Text = item.Name;
                if (item.Type == "link")
                {
                    menuItem.Command = OpenLinkCommand;
                    menuItem.CommandParameter = item.Href;
                }
                else if (item.Type == "path")
                {
                    menuItem.Command = OpenPathCommand;
                    menuItem.CommandParameter = item.Href;
                }

                menuItem.Icon = new ImageIcon { Source = item.IconFinder(), };

                menuItem.Tag = item.Href;
                ExternalLinks.Add(menuItem);
            }
        }
        BackgroundTaskFinished?.Invoke();
        _project = null;
    }

    public void ClearData()
    {
        _project = null;
        _name = null;
        _description = null;
        _tag = null;
        _iconPath = null;
        _bannerPath = null;
    }

    [RelayCommand]
    private void OpenLink(string url)
    {
        try
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error opening link: {ex.Message}");
        }
    }

    [RelayCommand]
    private void OpenPath(string path)
    {
        try
        {
            Process.Start("explorer.exe", path);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error opening path: {ex.Message}");
        }
    }
}
