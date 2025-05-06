using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;

namespace Pronder.Models;
public class Budget
{
    public double InitialBudget;

    public List<BudgetItem> Items;

    public bool ItemsNullOrEmpty() => Items == null || Items.Count <= 0;
}

public class BudgetItem
{
    public BudgetItem()
    {
        Date = DateTime.Now;
    }

    public double Amount { get; set; }
    public string Currency { get; set; }
    public bool DoesDecrease { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public DateTime Date { get; private set; }

    
    [JsonIgnore] public SolidColorBrush Background { get; set; }
}
