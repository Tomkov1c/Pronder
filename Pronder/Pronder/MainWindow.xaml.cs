using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Pronder.Interfaces;
using Pronder.Views;
using System;
using Windows.Gaming.Input;

namespace Pronder
{
    public sealed partial class MainWindow : Window
    {
        private static Frame _MainWindowFrame = null;

        public MainWindow()
        {
            InitializeComponent();

            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(AppTitleBar);

            _MainWindowFrame = MainWindowFrame;

            ChangeToPage(typeof(HomePage));

            AppTitleBar.PaneToggleRequested += (sender, args) =>
            {
                OnPaneExpandButtonPressed();
            };
        }


        public static void ChangeToPage(Type pageType, object? additionalData = null)
        {
            _MainWindowFrame.Navigate(pageType, additionalData, new DrillInNavigationTransitionInfo());
        }

        private void OnPaneExpandButtonPressed()
        {
            if (_MainWindowFrame?.Content is INavigationViewInterface navController)
            {
                navController.TogglePane();
            }
        }
    }
}
