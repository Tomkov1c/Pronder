using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Pronder.ViewModels;
using Pronder.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Pronder.Models;

namespace Pronder.Custom;
public sealed partial class EditTodoTaskPopup : ContentDialog
{
    public static event Action OnTodoTaskEdited;

    private EditTodoTaskPopupViewModel _viewmodel;

    public EditTodoTaskPopup(TodoTask? task)
    {
        this.XamlRoot = App.MainWindow.Content.XamlRoot;

        _viewmodel = new(task);
        DataContext = _viewmodel;

        this.InitializeComponent();
    }
}
