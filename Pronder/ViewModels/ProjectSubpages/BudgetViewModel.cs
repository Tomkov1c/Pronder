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
    public ICommand RemoveItemCommand { get; private set; }

    public static Project? _project => Project.GlobalInstance;

    public ObservableCollection<BudgetItem> Items { get; set; } = new();

    
    public string InitialBudget { get; set; } = _project.Budget.InitialBudget.ToString();
    public double CurrentBudget { get; set; } = _project.Budget.InitialBudget;

    public ProjectBudgetViewModel()
    {
        AddItemCommand = new RelayCommand(AddItem);
        RemoveItemCommand = new RelayCommand<BudgetItem?>(RemoveItem);

        if (!_project.Budget.ItemsNullOrEmpty())
        {
            foreach (BudgetItem item in _project.Budget.Items)
            {
                if(item.DoesDecrease)
                {
                    CurrentBudget -= item.Amount;
                    item.Foreground = Application.Current.Resources["SystemFillColorCriticalBrush"] as SolidColorBrush;
                }else
                {
                    CurrentBudget += item.Amount;
                    item.Foreground = Application.Current.Resources["SystemFillColorSuccessBrush"] as SolidColorBrush;
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
            CurrentBudget -= newItem.Amount;
            newItem.Foreground = Application.Current.Resources["SystemFillColorCriticalBrush"] as SolidColorBrush;
        }
        else
        {
            CurrentBudget += newItem.Amount;
            newItem.Foreground = Application.Current.Resources["SystemFillColorSuccessBrush"] as SolidColorBrush;
        }
        Items.Add(newItem);
    }
    private void RemoveItem(BudgetItem? itemToRemove)
    {
        foreach (BudgetItem item in Items)
        {
            if (item.Id == itemToRemove.Id)
            {
                Items.Remove(itemToRemove);

                return;
            }
        }
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