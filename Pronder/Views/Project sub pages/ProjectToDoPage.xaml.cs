using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Newtonsoft.Json;
using Pronder.Models;
using Pronder.ViewModels;
using Windows.ApplicationModel.Contacts;
using static Pronder.Models.Project;
using static Pronder.Models.ProjectExtraProperties;
namespace Pronder.Views;

public sealed partial class ProjectToDoPage : Page
{
    public ProjectToDoViewModel _viewModel;
    string path;

    public ProjectToDoPage()
    {
        _viewModel = new ProjectToDoViewModel();
        DataContext = _viewModel;
        InitializeComponent();

        CreateListView(_viewModel.Tasks);
    }

    private void CreateListView(ObservableCollection<TodoTask> tasks)
    {
        ListView mainListView = new ListView();
        mainListView.Margin = new Thickness(0, 24, 0, 0);
        mainListView.SelectionMode = ListViewSelectionMode.None;

        mainListView.Transitions.Add(new RepositionThemeTransition());
        mainListView.Transitions.Add(new ContentThemeTransition());
        mainListView.Transitions.Add(new AddDeleteThemeTransition());


        foreach (var task in tasks)
        {
            mainListView.Items.Add(CreateListViewItem(task, true));
        }

        PageContent.Children.Add(mainListView);
    }

    private ListViewItem CreateListViewItem(TodoTask task, bool IsOneMain = false)
    {
        ListViewItem listViewItem = new ListViewItem();
        listViewItem.Transitions.Add(new RepositionThemeTransition());
        listViewItem.Transitions.Add(new ContentThemeTransition());
        listViewItem.Transitions.Add(new AddDeleteThemeTransition());
        StackPanel stackPanel = new StackPanel { Orientation = Orientation.Vertical };
        stackPanel.Margin = new Thickness(24, 0, 0, 0);

        if (IsOneMain)
        {
            stackPanel.BorderThickness = new Thickness(0, 0, 0, 1);
            stackPanel.BorderBrush = (Brush)Application.Current.Resources["CircleElevationBorderBrush"];
        }

        StackPanel taskHeader = new StackPanel { Orientation = Orientation.Horizontal, Padding = new Thickness(0, 14, 0, 14) };
        CheckBox checkBox = new CheckBox { Margin = new Thickness(0, 0, 8, 0), CornerRadius = new CornerRadius(12), MinWidth = 0 };
        TextBlock taskText = new TextBlock { VerticalAlignment = VerticalAlignment.Center };

        Binding checkboxBinding = new Binding
        {
            Path = new PropertyPath("Done"),
            Mode = BindingMode.TwoWay,
            Source = task
        };
        BindingOperations.SetBinding(checkBox, CheckBox.IsCheckedProperty, checkboxBinding);

        Binding textBinding = new Binding
        {
            Path = new PropertyPath("Content"),
            Source = task
        };
        BindingOperations.SetBinding(taskText, TextBlock.TextProperty, textBinding);

        taskHeader.Children.Add(checkBox);
        taskHeader.Children.Add(taskText);

        Button removeButton = new Button
        {
            Content = "Remove",
            Command = _viewModel.RemoveTaskCommand,
            CommandParameter = task
        };
        taskHeader.Children.Add(removeButton);

        stackPanel.Children.Add(taskHeader);

        if (task.SubTasks != null && task.SubTasks.Count > 0)
        {
            ListView subTaskListView = new ListView();
            subTaskListView.Margin = new Thickness(16, 0, 0, 0);
            subTaskListView.SelectionMode = ListViewSelectionMode.None;

            foreach (var subTask in task.SubTasks)
            {
                subTaskListView.Items.Add(CreateListViewItem(subTask));
                subTaskListView.Transitions.Add(new RepositionThemeTransition());
                subTaskListView.Transitions.Add(new ContentThemeTransition());
                subTaskListView.Transitions.Add(new AddDeleteThemeTransition());
            }

            stackPanel.Children.Add(subTaskListView);
        }

        listViewItem.Content = stackPanel;

        return listViewItem;
    }



}
