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

    public ObservableCollection<NavigationViewItem> PaneItems = new();

    public ShellViewModel()
    {
        ImportProjects();
    }

    public async void ImportProjects()
    {
        StorageFolder projectFolder = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync("Projects");
        IReadOnlyList<StorageFile> files = await projectFolder.GetFilesAsync();

        foreach(var item in files)
        {
            Debug.Write(item.Path);

            ProjectBrief project = JsonConvert.DeserializeObject<ProjectBrief>(File.ReadAllText(item.Path));

            var navigationViewItem = new NavigationViewItem()
            {
                Content = project.Name,
            };


            if (!string.IsNullOrEmpty(project.Icon))
            {
                BitmapIcon bitmapIcon = new BitmapIcon
                {
                    UriSource = new Uri(project.Icon),
                    ShowAsMonochrome = false
                };
                navigationViewItem.Icon = bitmapIcon;
            }
            else
            {
                var icon = new ImageIcon { Source = Application.Current.Resources["Icon8Project"] as BitmapImage, };
                navigationViewItem.Icon = icon;
            }

            PaneItems.Add(navigationViewItem);
        }
    }
}

class ProjectBrief
{
    [JsonIgnore]
    public static string ProjectPath { get; private set; } = "";
    
    public string Name { get; set; }
    public string Icon { get; set; }
}