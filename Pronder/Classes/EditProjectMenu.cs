using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System.ComponentModel;
using Microsoft.UI.Xaml.Markup;
using Newtonsoft.Json;
using ProjectWorkspace;
using Pronder;
using Windows.Storage.Pickers;
using Windows.Storage;

namespace EditProjectMenuWorkspace
{
    public sealed class EditDataContentDialog : ContentDialog
    {
        public Brush CardBackground{ get; set; }
        public Brush CardStroke{ get; set; }
        public Brush TextColor{ get; set; }
        public static event Action OnProjectEdited;

        string path;


        string iconPath = null;
        string bannerPath = null;
        public EditDataContentDialog()
        {
            this.Title = "Edit project";
            this.PrimaryButtonText = "Save";
            this.CloseButtonText = "Cancel";
            this.DefaultButton = ContentDialogButton.Primary;
            this.Style = (Style)Application.Current.Resources["DefaultContentDialogStyle"];
            this.PrimaryButtonClick += saveEditedData;
            this.Loaded += importData;

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                CornerRadius = (CornerRadius)Application.Current.Resources["OverlayCornerRadius"]
            };

            var stackPanel = new StackPanel { Orientation = Orientation.Vertical, Width = 490 , Margin = new Thickness(0, 8, 0, 0)};

            stackPanel.Children.Add(CreateField("Name", "What is your project called?", "Name", "F1 M48,48z M0,0z M9.5,4C7.0324991,4,5,6.0324991,5,8.5L5,37.5A1.50015,1.50015,0,0,0,5.0234375,37.728516C5.1465046,40.089678,7.1106165,42,9.5,42L38.5,42C40.967501,42,43,39.967501,43,37.5L43,14.5C43,12.032499,40.967501,10,38.5,10L14,10 14,6.5C14,5.1364058,12.863594,4,11.5,4L9.5,4z M9.5,7L11,7 11,11.253906A1.50015,1.50015,0,0,0,11,11.740234L11,33 9.5,33C8.9694092,33,8.4744795,33.134564,8,33.306641L8,8.5C8,7.6535009,8.6535009,7,9.5,7z M14,13L38.5,13C39.346499,13,40,13.653501,40,14.5L40,37.5C40,38.346499,39.346499,39,38.5,39L9.5,39C8.6535009,39 8,38.346499 8,37.5 8,36.653501 8.6535009,36 9.5,36L11.5,36C12.863594,36,14,34.863594,14,33.5L14,13z M35.470703,17.986328A1.50015,1.50015,0,0,0,35.310547,18L31.5,18A1.50015,1.50015,0,1,0,31.5,21L31.878906,21 28.5,24.378906 24.560547,20.439453A1.50015,1.50015,0,0,0,22.439453,20.439453L17.439453,25.439453A1.50015,1.50015,0,1,0,19.560547,27.560547L23.5,23.621094 27.439453,27.560547A1.50015,1.50015,0,0,0,29.560547,27.560547L34,23.121094 34,23.5A1.50015,1.50015,0,1,0,37,23.5L37,19.673828A1.50015,1.50015,0,0,0,35.470703,17.986328z M18.5,31A1.50015,1.50015,0,1,0,18.5,34L35.5,34A1.50015,1.50015,0,1,0,35.5,31L18.5,31z"));
            stackPanel.Children.Add(CreateField("Tag", "What is your project?", "Tag", "F1 M48,48z M0,0z M28,4C26.408649,4,24.883169,4.6324568,23.757812,5.7578125L5.7578125,23.757812C3.4278601,26.087765,3.4278601,29.912235,5.7578125,32.242188L15.757812,42.242188C16.883169,43.367542 18.408649,44 20,44 21.591351,44 23.116831,43.367543 24.242188,42.242188L42.242188,24.242188C43.367959,23.116416,44,21.589398,44,19.998047L44,9C44,6.2565285,41.743472,4,39,4L28,4z M28,7L39,7C40.116528,7,41,7.8834715,41,9L41,19.998047C41,20.810696,40.695322,21.546865,40.121094,22.121094L22.121094,40.121094C21.546449,40.695738 20.812649,41 20,41 19.187351,41 18.453551,40.695738 17.878906,40.121094L7.8789062,30.121094C6.6988587,28.941046,6.6988586,27.058954,7.8789062,25.878906L25.878906,7.8789062C26.453551,7.3042619,27.187351,7,28,7z M34,11C32.343,11 31,12.343 31,14 31,15.657 32.343,17 34,17 35.657,17 37,15.657 37,14 37,12.343 35.657,11 34,11z"));
            stackPanel.Children.Add(CreateField("Description", "Describe your project.", "Description", "F1 M48,48z M0,0z M24,4C12.972066,4 4,12.972074 4,24 4,35.027926 12.972066,44 24,44 35.027934,44 44,35.027926 44,24 44,12.972074 35.027934,4 24,4z M24,7C33.406615,7 41,14.593391 41,24 41,33.406609 33.406615,41 24,41 14.593385,41 7,33.406609 7,24 7,14.593391 14.593385,7 24,7z M24,14A2,2,0,0,0,24,18A2,2,0,0,0,24,14z M23.976562,20.978516A1.50015,1.50015,0,0,0,22.5,22.5L22.5,33.5A1.50015,1.50015,0,1,0,25.5,33.5L25.5,22.5A1.50015,1.50015,0,0,0,23.976562,20.978516z"));
            stackPanel.Children.Add(CreateFileField("Icon", "Give your project an icon.", "Pick a file", "F1 M48,48z M0,0z M11.5,6C8.4802259,6,6,8.4802259,6,11.5L6,36.5C6,39.519774,8.4802259,42,11.5,42L36.5,42C39.519774,42,42,39.519774,42,36.5L42,11.5C42,8.4802259,39.519774,6,36.5,6L11.5,6z M11.5,9L36.5,9C37.898226,9,39,10.101774,39,11.5L39,31.955078 32.988281,26.138672A1.50015,1.50015,0,0,0,32.986328,26.136719C32.208234,25.385403 31.18685,25 30.173828,25 29.16122,25 28.13988,25.385387 27.361328,26.138672L25.3125,28.121094 19.132812,22.142578C18.35636,21.389748 17.336076,21 16.318359,21 15.299078,21 14.280986,21.392173 13.505859,22.140625A1.50015,1.50015,0,0,0,13.503906,22.142578L9,26.5 9,11.5C9,10.101774,10.101774,9,11.5,9z M30.5,13C29.125,13 27.903815,13.569633 27.128906,14.441406 26.353997,15.313179 26,16.416667 26,17.5 26,18.583333 26.353997,19.686821 27.128906,20.558594 27.903815,21.430367 29.125,22 30.5,22 31.875,22 33.096185,21.430367 33.871094,20.558594 34.646003,19.686821 35,18.583333 35,17.5 35,16.416667 34.646003,15.313179 33.871094,14.441406 33.096185,13.569633 31.875,13 30.5,13z M30.5,16C31.124999,16 31.403816,16.180367 31.628906,16.433594 31.853997,16.686821 32,17.083333 32,17.5 32,17.916667 31.853997,18.313179 31.628906,18.566406 31.403816,18.819633 31.124999,19 30.5,19 29.875001,19 29.596184,18.819633 29.371094,18.566406 29.146003,18.313179 29,17.916667 29,17.5 29,17.083333 29.146003,16.686821 29.371094,16.433594 29.596184,16.180367 29.875001,16 30.5,16z M16.318359,24C16.578643,24,16.835328,24.09366,17.044922,24.296875A1.50015,1.50015,0,0,0,17.046875,24.298828L23.154297,30.207031 14.064453,39 11.5,39C10.101774,39,9,37.898226,9,36.5L9,30.673828 15.589844,24.298828C15.802764,24.093234,16.059641,24,16.318359,24z M30.173828,28C30.438806,28,30.692485,28.09229,30.902344,28.294922L39,36.128906 39,36.5C39,37.898226,37.898226,39,36.5,39L18.380859,39 29.447266,28.294922C29.654714,28.094207,29.910436,28,30.173828,28z"));
            stackPanel.Children.Add(CreateFileField("Banner", "Give your project a banner.", "Pick a file", "F1 M48,48z M0,0z M5.4707031,6A1.50015,1.50015,0,0,0,4,7.5L4,40.5A1.50015,1.50015,0,0,0,6.0566406,41.892578C6.0566406,41.892578 13.272727,39 24,39 34.727273,39 41.943359,41.892578 41.943359,41.892578A1.50015,1.50015,0,0,0,44,40.5L44,7.5A1.50015,1.50015,0,0,0,41.943359,6.1074219C41.943359,6.1074219 34.727273,9 24,9 13.272727,9 6.0566406,6.1074219 6.0566406,6.1074219A1.50015,1.50015,0,0,0,5.4707031,6z M7,9.4960938C9.3040875,10.279317 14.914206,12 24,12 33.085794,12 38.695912,10.279317 41,9.4960938L41,38.503906C40.516731,38.33963,39.80306,38.120669,39.027344,37.898438L26.839844,26.152344A1.50015,1.50015,0,0,0,26.835938,26.148438C26.048489,25.394647 25.024373,25.017555 24,25.017578 22.975627,25.017601 21.951166,25.394229 21.164062,26.148438A1.50015,1.50015,0,0,0,21.160156,26.152344L8.9726562,37.900391C8.1972367,38.122552,7.4831349,38.339675,7,38.503906L7,9.4960938z M30.5,17C29.119,17 28,18.119 28,19.5 28,20.881 29.119,22 30.5,22 31.881,22 33,20.881 33,19.5 33,18.119 31.881,17 30.5,17z M24.001953,28.001953C24.272809,28.002662,24.5435,28.107519,24.761719,28.316406L33.458984,36.699219C30.879849,36.299292 27.753591,36 24,36 20.245461,36 17.118556,36.299138 14.539062,36.699219L23.240234,28.314453 23.242188,28.3125C23.460076,28.104679,23.731097,28.001245,24.001953,28.001953z"));

            scrollViewer.Content = stackPanel;
            this.Content = scrollViewer;
        }

        public void saveEditedData(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));

            TextBox TextBoxName = this.FindName("TextBoxName") as TextBox;
            TextBox TextBoxTag = this.FindName("TextBoxTag") as TextBox;
            TextBox TextBoxDescription = this.FindName("TextBoxDescription") as TextBox;

            project.Name = TextBoxName.Text;
            project.Tag = TextBoxTag.Text;
            project.About = TextBoxDescription.Text;
            project.Icon = iconPath;
            project.Banner = bannerPath;

            string updatedJson = JsonConvert.SerializeObject(project, Formatting.Indented);
            File.WriteAllText(path, updatedJson);

            OnProjectEdited?.Invoke();
        }

        public void importData(object sender, RoutedEventArgs e)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            path = localSettings.Values["currentlyActiveProject"].ToString();

            Project project = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));

            TextBox TextBoxName = this.FindName("TextBoxName") as TextBox;
            TextBox TextBoxTag = this.FindName("TextBoxTag") as TextBox;
            TextBox TextBoxDescription = this.FindName("TextBoxDescription") as TextBox;

            TextBoxName.Text = project.Name;
            TextBoxTag.Text = project.Tag;
            TextBoxDescription.Text = project.About;
            iconPath = project.Icon;
            bannerPath = project.Banner;

            TextBoxName.Select(TextBoxName.Text.Length, 0);
            TextBoxTag.Select(TextBoxTag.Text.Length, 0);
            TextBoxDescription.Select(TextBoxDescription.Text.Length, 0);
        }

        private StackPanel CreateField(string label, string description, string placeholder, string glyphPath)
        {
            var stackPanel = new StackPanel
            {
                Background = (Brush)Application.Current.Resources["CardBackgroundFillColorDefaultBrush"],
                Padding = new Thickness(24, 16, 24, 16),
                Margin = new Thickness(0, 0, 0, 16),
                CornerRadius = (CornerRadius)Application.Current.Resources["ControlCornerRadius"]
            };

            var headerPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var icon = new Canvas
            {
                Width = 20,
                Height = 20,
                Margin = new Thickness(0, 0, 12, 0),
            };
            var viewbox = new Viewbox
            {
                Width = 20,
                Height = 20,
            };
            var path = new Microsoft.UI.Xaml.Shapes.Path
            {
                Fill = (Brush)Application.Current.Resources["TextFillColorPrimaryBrush"],
                StrokeThickness = 1,
                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), glyphPath)
            };
            viewbox.Child = path;
            icon.Children.Add(viewbox);
            var headerText = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(8, 0, 0, 0)
            };
            headerText.Children.Add(new TextBlock { Text = label, Style = (Style)Application.Current.Resources["BodyStrongTextBlockStyle"] });
            headerText.Children.Add(new TextBlock { Text = description, Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"], Foreground = (Brush)Application.Current.Resources["TextFillColorTertiaryBrush"] });

            headerPanel.Children.Add(icon);
            headerPanel.Children.Add(headerText);

            var textBox = new TextBox
            {
                PlaceholderText = placeholder,
                Margin = new Thickness(0, 24, 0, 8),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                Name = "TextBox" + label,
                Foreground = (Brush)Application.Current.Resources["TextFillColorPrimaryBrush"],
            };

            stackPanel.Children.Add(headerPanel);
            stackPanel.Children.Add(textBox);

            return stackPanel;
        }

        private Grid CreateFileField(string label, string description, string buttonText, string glyphPath)
        {
            var grid = new Grid
            {
                Background = (Brush)Application.Current.Resources["CardBackgroundFillColorDefaultBrush"],
                Padding = new Thickness(24, 16, 24, 16),
                Margin = new Thickness(0, 0, 0, 16),
                CornerRadius = (CornerRadius)Application.Current.Resources["ControlCornerRadius"]
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)});

            var icon = new Canvas
            {
                Width = 20,
                Height = 20,
                Margin = new Thickness(0, 0, 12, 0),
            };
            var viewbox = new Viewbox
            {
                Width = 20,
                Height = 20,
            };
            var path = new Microsoft.UI.Xaml.Shapes.Path
            {
                Fill = (Brush)Application.Current.Resources["TextFillColorPrimaryBrush"],
                StrokeThickness = 1,
                Data = (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), glyphPath)
            };
            viewbox.Child = path;
            icon.Children.Add(viewbox);

            var headerText = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(8, 0, 0, 0)
            };
            headerText.Children.Add(new TextBlock { Text = label, Style = (Style)Application.Current.Resources["BodyStrongTextBlockStyle"] });
            headerText.Children.Add(new TextBlock { Text = description, Style = (Style)Application.Current.Resources["CaptionTextBlockStyle"], Foreground = (Brush)Application.Current.Resources["TextFillColorTertiaryBrush"] });

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
            };
            var fontIcon = new FontIcon
            {
                Glyph = "\uEC50",
                FontSize = 15,
            };
            var text = new TextBlock
            {
               Text = buttonText,
               Margin = new Thickness(8, 0, 0 , 0),
               Style = (Style)Application.Current.Resources["BodyTextBlockStyle"]
            };
            stackPanel.Children.Add(fontIcon);
            stackPanel.Children.Add(text);

            var button = new Button
            {
                Content = stackPanel,
                Style = (Style)Application.Current.Resources["DefaultButtonStyle"],
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 0, 0),
                Tag = label
            };

            button.Click += openFileExplorer;
            Grid.SetColumn(icon, 0);
            Grid.SetColumn(headerText, 1);
            Grid.SetColumn(button, 2);

            grid.Children.Add(icon);
            grid.Children.Add(headerText);
            grid.Children.Add(button);

            return grid;
        }

        async void openFileExplorer(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = App.MainWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            // Open the picker for the user to pick a file
            var file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                if (button.Tag == "Icon")
                {
                    iconPath = file.Path;
                }
                else if(button.Tag == "Banner")
                {
                    bannerPath = file.Path;
                }
                else
                {

                }
            }


        }
    }

}
