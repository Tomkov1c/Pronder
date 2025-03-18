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
using Pronder.Helpers.Mine;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class GeneralProjectDisplayViewModel : ObservableRecipient
{
    private string projectPath;
    public Project _project;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private string _tag;

    [ObservableProperty]
    private string? _iconPath;

    [ObservableProperty]
    private string? _bannerPath;

    public ObservableCollection<MenuFlyoutItem> ExternalLinks { get; } = new();

    public GeneralProjectDisplayViewModel(string path)
    {
        InitializeAsync(path);
    }

    private async Task InitializeAsync(string path)
    {
        await MainTasksAsync(path);
        BackgroundTasksAsync();
    }

    private async Task MainTasksAsync(string path)
    {
        this.projectPath = path;

        _project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));

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
    }

    private void BackgroundTasksAsync()
    {
        if (!_project.LinksNullOrEmpty())
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

                //menuItem.Icon = new ImageIcon { Source = new BitmapImage(new Uri(await new ExternalLinkHelper().GetIconPath(deserialized.Links[i]))), };

                menuItem.Tag = item.Href;
                ExternalLinks.Add(menuItem);
            }
    }

    [RelayCommand]
    private void OpenLink(string url)
    {
        try
        {
            var uri = new Uri(url);
            var success = Windows.System.Launcher.LaunchUriAsync(uri);
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
