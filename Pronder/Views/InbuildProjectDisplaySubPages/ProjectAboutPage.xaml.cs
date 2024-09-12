using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Pronder.Classes;
using Pronder.ViewModels;

namespace Pronder.Views;

public sealed partial class ProjectAboutPage : Page
{
    public ProjectAboutViewModel ViewModel
    {
        get;
    }
    string path = (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["cccc"];

    public ProjectAboutPage()
    {
        ViewModel = App.GetService<ProjectAboutViewModel>();
        InitializeComponent();

        importData();
    }

    async void importData()
    {
        Project deserialized;
        using (StreamReader file = File.OpenText((string)Windows.Storage.ApplicationData.Current.LocalSettings.Values["currentlyActiveProject"]))
        {
            deserialized = JsonConvert.DeserializeObject<Project>(file.ReadToEnd());
        }

        DescriptionContnet.Text = deserialized.About;
    }
}
