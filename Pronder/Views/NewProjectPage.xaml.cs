using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using ProjectWorkspace;
using Pronder.ViewModels;
using Windows.Storage;

namespace Pronder.Views;

public sealed partial class NewProjectPage : Page
{
    public NewProjectViewModel ViewModel
    {
        get;
    }

    public NewProjectPage()
    {
        ViewModel = App.GetService<NewProjectViewModel>();
        InitializeComponent();
    }

    public static event Action OnProjectCreated;

    async void createNewProject(object sender, RoutedEventArgs e)
    {
        var project = new Project
        {
            Name = ProjectNameTextbox.Text,
            Tag = ProjectTagTextbox.Text,
            About = ProjectAboutTextbox.Text,
        };

        // Serialize the Project object to a JSON string
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

        // Create and write to the JSON file
        string fileName = $"{ProjectNameTextbox.Text}.json";
        StorageFile projectFile = await projectFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteTextAsync(projectFile, json);

        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        localSettings.Values["shellPage_NewProject"] = ProjectNameTextbox.Text;
        OnProjectCreated?.Invoke();
    }
}
