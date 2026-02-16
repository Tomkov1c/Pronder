using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Pronder.Helpers;
using Pronder.Interfaces;
using Pronder.ViewModels;
using Pronder.Views;
using System;
using System.Linq;
using System.Security.AccessControl;
using Windows.Gaming.Input;
using WinUIEx;

namespace Pronder
{
    public sealed partial class MainWindow : WindowEx
    {

        public MainWindow()
        {
            InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);

            MainWindowFrame.Navigate(typeof(HomePage), null, new DrillInNavigationTransitionInfo());
        }

        private void OnPaneExpandButtonPressed(Microsoft.UI.Xaml.Controls.TitleBar sender, object args)
        {
            if (MainWindowFrame?.Content is INavigationViewInterface navController)
                navController.TogglePane();
        }

        private void OnFrameNavigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            AppTitleBar.IsPaneToggleButtonVisible = MainWindowFrame.Content is INavigationViewInterface;
        }
    }
}
