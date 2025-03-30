using System;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Pronder.Contracts.Services;

namespace Pronder.ViewModels;

public partial class SettingsInterfaceViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;

    [ObservableProperty]
    private ElementTheme _elementTheme;
    [ObservableProperty]
    private int _selectedThemeIndex;

    public ICommand SwitchThemeCommand { get; }

    public SettingsInterfaceViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;

        switch (_elementTheme)
        {
            case ElementTheme.Light:
                SelectedThemeIndex = 0;
                break;

            case ElementTheme.Dark:
                SelectedThemeIndex = 1;
                break;

            default:
                SelectedThemeIndex = 2;
                break;
        }

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
}
