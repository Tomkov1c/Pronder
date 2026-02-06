using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Pronder.Helpers;
using Pronder.Interfaces;
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
        private static Frame _MainWindowFrame = null;

        public MainWindow()
        {
            InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);

            _MainWindowFrame = MainWindowFrame;

            _MainWindowFrame.Navigate(typeof(HomePage), null, new DrillInNavigationTransitionInfo());

            AppTitleBar.PaneToggleRequested += (sender, args) =>
            {
                OnPaneExpandButtonPressed();
            };
        }

        private void OnPaneExpandButtonPressed()
        {
            if (_MainWindowFrame?.Content is INavigationViewInterface navController)
            {
                navController.TogglePane();
            }
        }

        private void MenuFlyout_Opening(object sender, object e)
        {
            if (sender is MenuFlyout flyout)
            {
                foreach (var item in flyout.Items.OfType<MenuFlyoutItem>())
                {
                    NavigationHelper.SetFrame(item, _MainWindowFrame);
                }
            }
        }

    }
}
