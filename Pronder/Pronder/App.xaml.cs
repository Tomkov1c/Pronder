using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppLifecycle;
using Pronder.FileSchemes;
using Pronder.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Pronder
{
    public partial class App : Application
    {
        public Window? _window;

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.AppWindow.TitleBar.PreferredHeightOption = Microsoft.UI.Windowing.TitleBarHeightOption.Tall;
            _window.Activate();

            var activatedArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();

            if (activatedArgs.Kind == ExtendedActivationKind.File)
            {
                var fileArgs = activatedArgs.Data as IFileActivatedEventArgs;
                if (fileArgs?.Files[0] is Windows.Storage.StorageFile file)
                {
                    var data = (SchemeV1Data)BinaryFileHandler.Read(file.Path);

                    var textBlock = new TextBlock
                    {
                        Text = data.Name,
                        FontSize = 24,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    var fileWindow = new Window
                    {
                        Title = "File Opened",
                        Content = textBlock
                    };

                    fileWindow.Activate();
                }
            }
        }
    }
}
