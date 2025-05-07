using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Pronder.Custom;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class ProjectBudgetViewModel : ObservableRecipient
{
    public ICommand AddItemCommand { get; private set; }
    public ICommand RemoveItemCommand { get; private set; }
    public ICommand EditItemCommand { get; private set; }

    public static Project? _project => Project.GlobalInstance;

    public ObservableCollection<BudgetItem> Items { get; set; } = new();

    private EditBudgetPopup popup;

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


    private string _newItemName;
    public string NewItemName
    {
        get => _newItemName;
        set
        {
            _newItemName = value; OnPropertyChanged();
        }
    }
    private string _newItemDescription;
    public string NewItemDescription
    {
        get => _newItemDescription;
        set
        {
            _newItemDescription = value; OnPropertyChanged();
        }
    }
    private double _newItemAmount;
    public double NewItemAmount
    {
        get => _newItemAmount;
        set
        {
            _newItemAmount = value; OnPropertyChanged();
        }
    }
    private int _selectedItemTypeIndex;
    public int SelectedItemTypeIndex
    {
        get => _selectedItemTypeIndex;
        set
        {
            _selectedItemTypeIndex = value; OnPropertyChanged();
        }
    }



    public ProjectBudgetViewModel()
    {
        AddItemCommand = new RelayCommand(AddItem);
        RemoveItemCommand = new RelayCommand<BudgetItem?>(RemoveItem);
        EditItemCommand = new RelayCommand<BudgetItem?>(EditItem);

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

                item.PropertyChanged += (s, e) => Save();
                Items.Add(item);
            }
        }

        Items.CollectionChanged += (s, e) => Save();
        NoItems = Items.Any() ? Visibility.Collapsed : Visibility.Visible;
        NegateNoItems = Items.Any() ? Visibility.Visible : Visibility.Collapsed;
    }



    private void AddItem()
    {
        bool isExpense = SelectedItemTypeIndex == 0;

        var newItem = new BudgetItem
        {
            Name = NewItemName,
            Description = NewItemDescription,
            Amount = NewItemAmount,
            DoesDecrease = isExpense,

            Date = DateTime.Now.AddSeconds(-DateTime.Now.Second).AddMilliseconds(-DateTime.Now.Millisecond)
        };
        NewItemName = null;
        NewItemDescription = null;
        NewItemAmount = 0;
        SelectedItemTypeIndex = 0;

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
    private async void EditItem(BudgetItem? item)
    {
        Debug.WriteLine(item.Id);
        popup = new(item);
        await popup.ShowAsync();
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