using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Pronder.Models;
using Pronder.ViewModels;

namespace Pronder.Views;

public sealed partial class ProjectAboutPage : Page
{
    public ProjectAboutPage()
    {
        InitializeComponent();
        importData();
    }

    void importData()
    {
        DescriptionContnet.Text = Project.GlobalInstance.About;
    }
}
