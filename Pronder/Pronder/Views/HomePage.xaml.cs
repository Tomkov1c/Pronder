using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pronder.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Pronder.Views
{
    public sealed partial class HomePage : Page, INavigationViewInterface
    {
        public HomePage()
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
}
