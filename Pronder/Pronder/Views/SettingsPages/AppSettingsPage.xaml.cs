using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pronder.Interfaces;

namespace Pronder.Views.SettingsPages;

public sealed partial class AppSettingsPage : Page, INavigationViewInterface
{
    public AppSettingsPage()
    {
        InitializeComponent();
    }

    public void ClosePane()
    {
        PagePanel.IsPaneOpen = false;
    }

    public void OpenPane()
    {
        PagePanel.IsPaneOpen = true;
    }

    public void TogglePane()
    {
        PagePanel.IsPaneOpen = !PagePanel.IsPaneOpen;
    }
}
