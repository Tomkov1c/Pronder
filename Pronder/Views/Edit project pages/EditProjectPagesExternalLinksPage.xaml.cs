using Microsoft.UI.Xaml.Controls;

using Pronder.ViewModels;

namespace Pronder.Views;

public sealed partial class EditProjectPagesExternalLinksPage : Page
{
    public EditProjectPagesExternalLinksViewModel ViewModel
    {
        get;
    }

    public EditProjectPagesExternalLinksPage()
    {
        ViewModel = App.GetService<EditProjectPagesExternalLinksViewModel>();
        InitializeComponent();
    }
}
