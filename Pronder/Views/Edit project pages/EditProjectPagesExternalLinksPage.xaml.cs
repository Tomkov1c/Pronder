using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Pronder.Custom;
using Pronder.Helpers.Mine;
using Pronder.Models;
using Pronder.ViewModels;
using Windows.ApplicationModel.Calls;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml;
using CommunityToolkit.WinUI.Controls;

namespace Pronder.Views;

public sealed partial class EditProjectPagesExternalLinksPage : Page
{
    public EditProjectPagesExternalLinksViewModel _viewModel;

    public EditProjectPagesExternalLinksPage()
    {
        InitializeComponent();
    }
    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is string path)
        {
            _viewModel = new EditProjectPagesExternalLinksViewModel(path);
            DataContext = _viewModel;

            List<Link> links = _viewModel._project.Links;

            if(links != null)
            foreach (Link link in links)
            {
                var icon = new Uri(await new ExternalLinkHelper().GetIconPath(link));
                SettingsExpander linkDisplay = new SettingsExpander()
                {
                    HeaderIcon = new BitmapIcon() { UriSource = icon, ShowAsMonochrome = false},
                    Header = link.Name,
                    Description = link.Href,
                };


                await DispatcherQueue.EnqueueAsync(() => StackPanel.Children.Add(linkDisplay));
            }
        }
    }
}
