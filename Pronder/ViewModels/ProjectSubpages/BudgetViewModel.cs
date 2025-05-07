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

    private double _currentBudget = _project.Budget.InitialBudget;
    public double CurrentBudget
    {
        get => _currentBudget;
        set
        {
            if (_currentBudget != value)
            {
                _currentBudget = value;
                OnPropertyChanged();
            }
        }
    }
    private Visibility _noItems;
    public Visibility NoItems
    {
        get => _noItems;
        set
        {
            if (_noItems != value)
            {
                _noItems = value;
                OnPropertyChanged();
            }
        }
    }
    private Visibility _negateNoItems;
    public Visibility NegateNoItems
    {
        get => _negateNoItems;
        set
        {
            if (_negateNoItems != value)
            {
                _negateNoItems = value;
                OnPropertyChanged();
            }
        }
    }

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
        NoItems = Items.Any() ? Visibility.Collapsed : Visibility.Visible;
        NegateNoItems = Items.Any() ? Visibility.Visible : Visibility.Collapsed;
    }



    private void AddItem()
    {
        BudgetItem newItem = new()
        {
            Amount = 69,
            Currency = "€",
            DoesDecrease = true,
            Name = "Lorem",
            Description = "mcdonalds",
            Date = DateTime.Now.AddSeconds(-DateTime.Now.Second).AddMilliseconds(-DateTime.Now.Millisecond)
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
                if (itemToRemove.DoesDecrease)
                {
                    CurrentBudget += itemToRemove.Amount;
                }
                else
                {
                    CurrentBudget -= itemToRemove.Amount;
                }
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
        NoItems = Items.Any() ? Visibility.Collapsed : Visibility.Visible;
        NegateNoItems = Items.Any() ? Visibility.Visible : Visibility.Collapsed;
        _project.SaveToFile();
    }
}