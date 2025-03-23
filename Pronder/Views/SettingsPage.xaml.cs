using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using Pronder.ViewModels;
using Windows.Storage.Streams;
using Windows.Storage;
using Newtonsoft.Json;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Markup;

namespace Pronder.Views;
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }
    public static event Action OnThemeSelected;

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();

        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        int index;
        if (localSettings.Values.ContainsKey("global.ActiveAppTheme"))
        {
            switch (localSettings.Values["global.ActiveAppTheme"])
            {
                case "Light":
                    index = 0;
                    break;

                case "Dark":
                    index = 1;
                    break;

                default:
                    index = 2;
                    break;
            }
        }
        else
        {
            index = 2;
        }
        Settings_Theme_ComboBox.SelectedIndex = index;
    }

    private void themeChange(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            var selectedTheme = (ElementTheme)Enum.Parse(typeof(ElementTheme), selectedItem.Content.ToString());
            ViewModel.SwitchThemeCommand.Execute(selectedTheme);

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["global.ActiveAppTheme"] = selectedItem.Content.ToString();
        }
    }
}
