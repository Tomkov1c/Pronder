using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Pronder.Contracts.Services;
using Pronder.Helpers;
using static Pronder.Classes.SaveData;

namespace Pronder.ViewModels;

public partial class SettingsInterfaceViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private SettingsHelper localsettings = new();

    [ObservableProperty]
    private ElementTheme _elementTheme;
    [ObservableProperty]
    private int _selectedThemeIndex;
    [ObservableProperty]
    public int _selectedPaneOrderIndex;


    public ICommand SwitchThemeCommand { get; }


    public SettingsInterfaceViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;

        LoadData();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    private void LoadData()
    {
        switch (_elementTheme)
        {
            case ElementTheme.Light: SelectedThemeIndex = 0; break;

            case ElementTheme.Dark: SelectedThemeIndex = 1; break;

            default: SelectedThemeIndex = 2; break;
        }

        _selectedPaneOrderIndex = Convert.ToInt32(localsettings.Read("PaneProjectsSorted").ToString());
        
        
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(SelectedPaneOrderIndex))
                ChangeProjectsOrder();
        };
    }

    private void ChangeProjectsOrder() => localsettings.WriteTo("PaneProjectsSorted", _selectedPaneOrderIndex);
}
