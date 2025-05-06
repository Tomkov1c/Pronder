using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class ProjectBudgetViewModel : ObservableRecipient
{
    public ICommand AddItemCommand { get; private set; }

    public static Project? _project => Project.GlobalInstance;

    public ObservableCollection<BudgetItem> Items { get; set; } = new();


    public string InitialBudget => _project.Budget.InitialBudget.ToString();

    public ProjectBudgetViewModel()
    {
        AddItemCommand = new RelayCommand(AddItem);

        if (!_project.Budget.ItemsNullOrEmpty())
        {
            foreach (BudgetItem item in _project.Budget.Items)
            {
                if(item.DoesDecrease)
                {
                    item.Background = Application.Current.Resources["SystemFillColorCriticalBackgroundBrush"] as SolidColorBrush;
                }else
                {
                    item.Background = Application.Current.Resources["SystemFillColorSuccessBackgroundBrush"] as SolidColorBrush;
                }
                Items.Add(item);
            }
        }

        Items.CollectionChanged += (s, e) => Save();
    }



    private void AddItem()
    {
        BudgetItem newItem = new()
        {
            Amount = 69,
            Currency = "€",
            DoesDecrease = true,
            Name = "Lorem",
            Description = "mcdonalds"
        };
        if (newItem.DoesDecrease)
        {
            newItem.Background = Application.Current.Resources["SystemFillColorCriticalBackgroundBrush"] as SolidColorBrush;
        }
        else
        {
            newItem.Background = Application.Current.Resources["SystemFillColorSuccessBackgroundBrush"] as SolidColorBrush;
        }
        Items.Add(newItem);
    }

    private void Save()
    {
        if (!_project.Budget.ItemsNullOrEmpty())
        {
            _project.Budget.Items = new List<BudgetItem>();
        }
        _project.Budget.Items = Items.ToList();
        _project.SaveToFile();
    }
}