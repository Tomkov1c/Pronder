using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using Pronder.Models;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Pronder.ViewModels;

public partial class NewProjectViewModel : ObservableRecipient
{
    public ICommand CreateProjectCommand { get; private set; }
    public ICommand OpenFilePickerCommand { get; private set; }

    [ObservableProperty] public string name;
    [ObservableProperty] public string tag;
    [ObservableProperty] public string description;
    [ObservableProperty] public string icon;
    [ObservableProperty] public string banner;

    [ObservableProperty] public bool visibleWarning = false;

    public static event Action OnProjectCreated;

    public NewProjectViewModel()
    {
        CreateProjectCommand = new RelayCommand(CreateProject);
        OpenFilePickerCommand = new RelayCommand<string>(OpenFilePicker);
    }
    private async void OpenFilePicker(string adress)
    {
        var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.FileTypeFilter.Add(".jpg");
        openPicker.FileTypeFilter.Add(".jpeg");
        openPicker.FileTypeFilter.Add(".png");

        var file = await openPicker.PickSingleFileAsync();
        if (file != null)
        {
            if(adress == "Icon")
            {
                icon = file.Path;
            }else if (adress == "Banner")
            {
                banner = file.Path;
            }
        }
    }
    private async void CreateProject()
    {
        if(string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            visibleWarning = true;

            return;
        }
        var project = new Project
        {
            Name = name,
            Tag = tag,
            About = description,
            Icon = icon,
            Banner = banner,
        };
        string json = JsonConvert.SerializeObject(project, Formatting.Indented);

        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        string projectFolderName = "Projects";
        StorageFolder projectFolder;
        try
        {
            projectFolder = await storageFolder.GetFolderAsync(projectFolderName);
        }
        catch (FileNotFoundException)
        {
            projectFolder = await storageFolder.CreateFolderAsync(projectFolderName);
        }

        string fileName = $"{name + " " + (new Guid()).ToString()}.json";
        StorageFile projectFile = await projectFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteTextAsync(projectFile, json);

        OnProjectCreated.Invoke();
    }
}
