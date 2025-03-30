using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Pronder.ViewModels;

namespace Pronder.Views;

public sealed partial class SettingsInterfacePage : Page
{
    public SettingsInterfaceViewModel _viewModel;

    public SettingsInterfacePage()
    {
        _viewModel = App.GetService<SettingsInterfaceViewModel>();
        DataContext = _viewModel;
        InitializeComponent();
    }

    private void ThemeChange(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            var selectedTheme = (ElementTheme)Enum.Parse(typeof(ElementTheme), selectedItem.Content.ToString());
            _viewModel.SwitchThemeCommand.Execute(selectedTheme);
        }
    }
}
