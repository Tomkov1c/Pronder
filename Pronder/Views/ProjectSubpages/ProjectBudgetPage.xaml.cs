using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pronder.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
namespace Pronder.Views;

public sealed partial class ProjectBudgetPage : Page
{
    ProjectBudgetViewModel _viewModel = new();

    public ProjectBudgetPage()
    {
        DataContext = _viewModel;
        this.InitializeComponent();
    }

    private void EnterPressed(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        //_viewModel.NewTaskTitle = NewTaskContentTextBox.Text;
        _viewModel.AddItemCommand.Execute(null);
    }
}
