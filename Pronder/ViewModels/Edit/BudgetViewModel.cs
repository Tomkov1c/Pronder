using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Newtonsoft.Json;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class EditProjectPagesBudgetViewModel : ObservableRecipient
{
    public static Project? _project => Project.GlobalInstance;

    [ObservableProperty] private double budget;

    public EditProjectPagesBudgetViewModel()
    {
        budget = _project.Budget.InitialBudget;

        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Budget))
                Save();
        };

    }

    private void Save()
    {
        _project.Budget.InitialBudget = budget;
        _project.SaveToFile();
    }
}
