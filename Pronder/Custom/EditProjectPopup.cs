using System;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Pronder.Views;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace Pronder.Custom
{
    public class EditProjectPopup
    {
        private Windows.Storage.ApplicationDataContainer localSettings { get; set; } = Windows.Storage.ApplicationData.Current.LocalSettings;
        private string path;

        public static event Action OnProjectEdited;

        public Popup popup = new();
        TransitionCollection transitionCollection = new();

        Grid content = new();

        Frame frame = new();
        Border border = new();

        NavigationView navView = new();
        StackPanel mainStackPanel = new();
        StackPanel stackPanel = new();

        public EditProjectPopup(XamlRoot xamlRoot, string path)
        {
            this.path = path;

            popup.IsLightDismissEnabled = false;
            popup.LightDismissOverlayMode = LightDismissOverlayMode.Off;
            popup.XamlRoot = xamlRoot;
            popup.IsOpen = true;
            transitionCollection.Add(new PopupThemeTransition());

            content.Width = App.MainWindow.Bounds.Width;
            content.Height = App.MainWindow.Bounds.Height;
            content.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(71, 0, 0, 0));
            content.ChildrenTransitions = transitionCollection;

            border = new()
            {
                Width = App.MainWindow.Bounds.Width - 500,
                Height = App.MainWindow.Bounds.Height - 150,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(1),
                BorderBrush = (SolidColorBrush)Application.Current.Resources["SurfaceStrokeColorDefaultBrush"],
                CornerRadius = (CornerRadius)Application.Current.Resources["OverlayCornerRadius"],
                Background = GetTheme(),
            };

            mainStackPanel = new()
            {
                Orientation = Orientation.Vertical,
                Width = border.Width,
                Height = border.Height,
            };

            stackPanel = new()
            {
                Orientation = Orientation.Horizontal,
                Height = App.MainWindow.Bounds.Height - 150 - (2 * 24 + 32),
                Width = border.Width,
                Background = (SolidColorBrush)Application.Current.Resources["SolidBackgroundFillColorBaseBrush"],
            };

            frame.VerticalAlignment = VerticalAlignment.Stretch;
            frame.Width = stackPanel.Width - 200;
            frame.Padding = new Thickness(0, -32, 0, -10);

            navView = new()
            {
                IsSettingsVisible = false,
                OpenPaneLength = 200,
                Width = 200,
                Height = stackPanel.Height,
                IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed,
                IsPaneOpen = true,
                IsPaneToggleButtonVisible = false,
                ExpandedModeThresholdWidth = 100,
                Margin = new Thickness(0, 12, 0, 0),
            };

            navView.SelectionChanged += NavSelectionChanged;

            Helper_AddItemsToSidebar();

            stackPanel.Children.Add(navView);
            frame.BackStack.Clear();
            stackPanel.Children.Add(frame);

            StackPanel bottomSection = new StackPanel
            {
                Background = (SolidColorBrush)Application.Current.Resources["SolidBackgroundFillColorSecondaryBrush"],
                Padding = new Thickness(24),
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom,
            };

            StackPanel buttonGroup = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            Button button = new()
            {
                Content = "Close",
                Width = 150,
                Margin = new Thickness(0, 0, 8, 0),
            };

            button.KeyboardAccelerators.Add(new KeyboardAccelerator() { Key = VirtualKey.Escape, });
            button.Click += ClosePopup;

            buttonGroup.Children.Add(button);
            bottomSection.Children.Add(buttonGroup);
            mainStackPanel.Children.Add(stackPanel);
            mainStackPanel.Children.Add(bottomSection);

            border.Child = mainStackPanel;
            content.Children.Add(border);
            popup.Child = content;

            App.MainWindow.SizeChanged += UpdatePopupSize;
        }

        //TODO: Fix this shit
        private void UpdatePopupSize(object sender, WindowSizeChangedEventArgs e)
        {
            content.Width = Math.Max(App.MainWindow.Bounds.Width, 300);
            content.Height = Math.Max(App.MainWindow.Bounds.Height, 200);

            if (App.MainWindow.Bounds.Width <= 1400 && App.MainWindow.Bounds.Height <= 800)
            {
                border.Width = Math.Max(App.MainWindow.Bounds.Width - 100, 300);
                border.Height = Math.Max(App.MainWindow.Bounds.Height - 50, 200);
                mainStackPanel.Width = Math.Max(App.MainWindow.Bounds.Width - 100, 300);
                mainStackPanel.Height = Math.Max(App.MainWindow.Bounds.Height - 50, 200);
                this.frame.Width = Math.Max(App.MainWindow.Bounds.Width - 100 - 200, 300);
                this.frame.Height = Math.Max(this.stackPanel.Height, 200);
                this.stackPanel.Width = Math.Max(App.MainWindow.Bounds.Width - 100, 300);
                this.stackPanel.Height = Math.Max(App.MainWindow.Bounds.Height - 50 - (2 * 24 + 32), 200);
            }
            else
            {
                border.Width = Math.Max(App.MainWindow.Bounds.Width - 500, 300);
                border.Height = Math.Max(App.MainWindow.Bounds.Height - 150, 200);
                mainStackPanel.Width = Math.Max(App.MainWindow.Bounds.Width - 500, 300);
                mainStackPanel.Height = Math.Max(App.MainWindow.Bounds.Height - 150, 200);
                this.frame.Width = Math.Max(App.MainWindow.Bounds.Width - 500 - 200, 300);
                this.frame.Height = Math.Max(this.stackPanel.Height, 200);
                this.stackPanel.Width = Math.Max(App.MainWindow.Bounds.Width - 500, 300);
                this.stackPanel.Height = Math.Max(App.MainWindow.Bounds.Height - 150 - (2 * 24 + 32), 200);
            }

            navView.Height = stackPanel.Height;

        }
        private Brush GetTheme()
        {
            ContentDialog contentDialog = new() { Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style, Padding = new Thickness(16, 16, 0, 0)};
            if (localSettings.Values.ContainsKey("global.ActiveAppTheme"))
                switch (localSettings.Values["global.ActiveAppTheme"])
                {
                    case "Light":
                        contentDialog.RequestedTheme = ElementTheme.Light;
                        break;

                    case "Dark":
                        contentDialog.RequestedTheme = ElementTheme.Dark;
                        break;

                    default:
                        contentDialog.RequestedTheme = ElementTheme.Default;
                        break;
                }
            return contentDialog.Background;
        }

        private void NavSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            NavigationView nav = sender as NavigationView;

            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                string pageTag = selectedItem.Content as string;

                Type pageType = pageTag switch
                {
                    "General" => typeof(EditProjectPagesGeneralPage),
                    "External links" => typeof(EditProjectPagesExternalLinksPage),
                    _ => null
                };

                if (pageType != null)
                {
                    frame.Navigate(pageType, this.path);
                }
            }
        }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            OnProjectEdited?.Invoke();
            popup.IsOpen = false;
        }

        private void Helper_AddItemsToSidebar()
        {
            var title = new TextBlock { Text = "Edit", Style = (Style)Application.Current.Resources["BodyStrongTextBlockStyle"], Margin = new Thickness(16, 0, 0, 0), };
            navView.PaneCustomContent = title;

            var firstItem = Helper_CreateMenuItem(new SymbolIcon(Symbol.Page2), "General");
            firstItem.IsSelected = true;
            firstItem.Margin = new Thickness(0, 16, 0, 0);

            navView.MenuItems.Add(firstItem);
            navView.MenuItems.Add(Helper_CreateMenuItem(new SymbolIcon(Symbol.Link), "External links"));
        }

        private NavigationViewItem Helper_CreateMenuItem(SymbolIcon icon, string text)
        {
            return new NavigationViewItem()
            {
                Icon = icon,
                Content = text,
            };
        }
    }
}
