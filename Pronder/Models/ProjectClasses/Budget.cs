using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;

namespace Pronder.Models;
public class Budget
{
    public double InitialBudget;

    public List<BudgetItem> Items;

    public bool ItemsNullOrEmpty() => Items == null || Items.Count <= 0;
}

public class BudgetItem : ObservableObject
{
    public BudgetItem()
    {
        Id = Guid.NewGuid();
    }

    private double amount;
    public double Amount
    {
        get => amount;
        set => SetProperty(ref amount, value);
    }

    private string currency;
    public string Currency
    {
        get => currency;
        set => SetProperty(ref currency, value);
    }

    private bool doesDecrease;
    public bool DoesDecrease
    {
        get => doesDecrease;
        set => SetProperty(ref doesDecrease, value);
    }

    private string name;
    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    private string description;
    public string Description
    {
        get => description;
        set => SetProperty(ref description, value);
    }

    private DateTime date;
    public DateTime Date
    {
        get => date;
        set => SetProperty(ref date, value);
    }


    [JsonIgnore] public Guid Id { get; private set; }
    [JsonIgnore] public SolidColorBrush Foreground { get; set; }
}
