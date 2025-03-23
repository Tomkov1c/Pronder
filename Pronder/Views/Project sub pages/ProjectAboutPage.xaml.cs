using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Pronder.Models;
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
        Project deserialized = Project.GlobalInstance;

        DescriptionContnet.Text = deserialized.About;
    }
}
