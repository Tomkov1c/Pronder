using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using Pronder.Classes;
using Pronder.ViewModels;
namespace Pronder.Views;

public sealed partial class ProjectToDoPage : Page
{
    public ProjectToDoViewModel ViewModel
    {
        get;
    }
    string path;
    public ProjectToDoPage()
    {
        ViewModel = App.GetService<ProjectToDoViewModel>();
        InitializeComponent();

        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        path = (string)localSettings.Values["currentlyActiveProject"];


    }
    void importData()
    {
        TodoContentToDo.Children.Clear();
        TodoContentDone.Children.Clear();


        Project deserialized;
        using (StreamReader file = File.OpenText(path))
        {
            deserialized = JsonConvert.DeserializeObject<Project>(file.ReadToEnd());
        }

        if (deserialized?.Todo != null)
        {
            int fullyDoneLists = 0;
            int toBeDoneLists = 0;

            foreach (var todo in deserialized.Todo)
            {
                StackPanel mainStackPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    Margin = new Thickness(0, 16, 0, 16),
                    Height = double.NaN,
                    Background = TemplateToDo.Background,
                    BorderBrush = TemplateToDo.BorderBrush,
                    BorderThickness = new Thickness(1),
                    Name = "idk",

                };
                if (Application.Current.Resources["ControlCornerRadius"] is CornerRadius controlCornerRadius)
                {
                    mainStackPanel.CornerRadius = controlCornerRadius;
                }

                Grid innerGrid = new Grid
                {
                    Padding = new Thickness(16)
                };

                CheckBox checkBox = new CheckBox
                {
                    Content = todo.Content,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Tag = todo.Content,
                    IsChecked = todo.Done
                };
                checkBox.Checked += toDoMainDone;
                checkBox.Unchecked += toDoMainDone;

                Button button = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 0, 16, 0),
                    Padding = new Thickness(8)
                };
                FontIcon buttonIcon = new FontIcon
                {
                    Glyph = "\uE712",
                    FontSize = 15
                };
                button.Content = buttonIcon;

                MenuFlyout buttonFlyout = new MenuFlyout
                {
                    Placement = FlyoutPlacementMode.Bottom
                };

                MenuFlyoutItem moveUpItem = new MenuFlyoutItem
                {
                    Text = "Edit",
                    Icon = new FontIcon { Glyph = "\uE70F" }
                };
                MenuFlyoutItem removeItem = new MenuFlyoutItem
                {
                    Text = "Remove",
                    Icon = new FontIcon { Glyph = "\uE74D" },
                    Tag = todo.Content
                };
                removeItem.Click += toDoRemove;

                buttonFlyout.Items.Add(moveUpItem);
                buttonFlyout.Items.Add(removeItem);
                button.Flyout = buttonFlyout;

                innerGrid.Children.Add(checkBox);
                innerGrid.Children.Add(button);
                mainStackPanel.Children.Add(innerGrid);

                if (todo.Sub != null)
                {
                    StackPanel nestedStackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Height = double.NaN,
                        Background = Application.Current.Resources["LayerFillColorAltBrush"] as Brush,
                        Padding = new Thickness(48, 16, 48, 16)
                    };

                    foreach (var subTodo in todo.Sub)
                    {
                        CheckBox subTodoCheckBox = new CheckBox
                        {
                            Content = subTodo.Content,
                            IsChecked = subTodo.Done
                        };
                        nestedStackPanel.Children.Add(subTodoCheckBox);
                    }

                    mainStackPanel.Children.Add(nestedStackPanel);
                }

                if (todo.Done || todo == null)
                {
                    TodoContentDone.Children.Add(mainStackPanel);
                    fullyDoneLists++;
                } 
                else
                {
                    TodoContentToDo.Children.Add(mainStackPanel);
                    toBeDoneLists++;
                }
            }

            if (toBeDoneLists <= 0)
            {
                TextBlock todoContentToDoIfEmpty = new TextBlock
                {
                    Text = "Currently empty. Click the + icon to add new items.",
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    TextAlignment = TextAlignment.Center,
                    Foreground = (Brush)Application.Current.Resources["TextFillColorDisabledBrush"] // Ensure this resource key exists
                };

                // Add the TextBlock to a parent container, e.g., a StackPanel or Grid
                // Replace 'parentContainer' with the actual name of your container
                TodoContentToDo.Children.Add(todoContentToDoIfEmpty);
            }

            if (fullyDoneLists > 0)
            {
                this.TodoContentDoneTitle.Visibility = Visibility.Visible;
                this.TodoContentDone.Visibility = Visibility.Visible;
            }
            else
            {
                this.TodoContentDoneTitle.Visibility = Visibility.Collapsed;
                this.TodoContentDone.Visibility = Visibility.Collapsed;
            }
        }
    }
    void setBackgroundColor(object sender, RoutedEventArgs e)
    {
        importData();
    }

    void toDoRemove(object sender, RoutedEventArgs e)
    {
        MenuFlyoutItem clickedItem = sender as MenuFlyoutItem;
        if (clickedItem != null)
        {
            // Retrieve the Tag value, which should be the content of the Todo item
            string tagValue = clickedItem.Tag as string;

            // Read and deserialize the JSON file into a Project object
            Project deserialized = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));

            if (deserialized.Todo != null)
            {
                // Find and remove the Todo item with the matching content
                var todoToRemove = deserialized.Todo.FirstOrDefault(todo => todo.Content == tagValue);

                if (todoToRemove != null)
                {
                    deserialized.Todo.Remove(todoToRemove);

                    // Serialize the modified Project object back to JSON
                    string updatedJson = JsonConvert.SerializeObject(deserialized, Formatting.Indented);

                    // Write the updated JSON back to the file
                    File.WriteAllText(path, updatedJson);

                    // Optionally refresh or reload data
                    importData();
                }
            }
        }
    }

    void toDoAdd(object sender, RoutedEventArgs e)
    {
        Project deserialized = JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));

        if (deserialized.Todo == null)
        {
            deserialized.Todo = new List<Todo>();
        }

        if (NewTodoTextBox.Text == null || NewTodoTextBox.Text == "" || NewTodoTextBox.Text == " ")
        {
            ToDoInfoBar.IsOpen = true;
            ToDoInfoBar.Title = "Error";
            ToDoInfoBar.Message = "ToDo's name cannot be empty.";
            ToDoInfoBar.Margin = new Thickness(0, 16, 0, 16);
            return;
        }

        // Create a new Todo item
        Todo newTodo = new Todo
        {
            Order = deserialized.Todo.Count + 1, // Set the order (adjust as needed)
            Content = NewTodoTextBox.Text, // Set the content of the new task
            Done = false, // Initial status of the task
            Sub = null// Optionally, initialize the Sub list if there are subtasks
        };

        // Add the new Todo item to the Todo list
        deserialized.Todo.Add(newTodo);

        // Serialize the modified Project object back to JSON
        string updatedJson = JsonConvert.SerializeObject(deserialized, Formatting.Indented);

        // Write the updated JSON back to the file
        File.WriteAllText(path, updatedJson);
        NewTodoFlyout.Hide();
        NewTodoTextBox.Text = null;
        ToDoInfoBar.IsOpen = false;
        ToDoInfoBar.Margin = new Thickness(0, 0, 0, 0);
        importData();
    }
    void enterPressed(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
            toDoAdd(sender, e);
    }


    void toDoMainDone(object sender, RoutedEventArgs e)
    {
        if (sender is CheckBox checkBox)
        {
            // Retrieve the content or tag value from the CheckBox
            string content = checkBox.Tag as string;

            if (!string.IsNullOrEmpty(content))
            {
                Project deserialized;
                using (StreamReader file = File.OpenText(path))
                {
                    deserialized = JsonConvert.DeserializeObject<Project>(file.ReadToEnd());
                }

                if (deserialized?.Todo != null)
                {
                    // Find the Todo item with the matching content
                    var todoToUpdate = deserialized.Todo.FirstOrDefault(todo => todo.Content == content);

                    if (todoToUpdate != null)
                    {
                        // Toggle the Done property based on the CheckBox state
                        todoToUpdate.Done = checkBox.IsChecked.GetValueOrDefault(!todoToUpdate.Done);

                        // Serialize the modified Project object back to JSON
                        string updatedJson = JsonConvert.SerializeObject(deserialized, Formatting.Indented);

                        // Write the updated JSON back to the file
                        using (StreamWriter file = new StreamWriter(path))
                        {
                            file.Write(updatedJson);
                        }

                        // Optionally refresh or reload data
                        importData();
                    }
                }
            }
        }
    }
}
