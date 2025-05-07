using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class EditBudgetPopupViewModel : ObservableRecipient
{
    [ObservableProperty] BudgetItem? item;
    [ObservableProperty] int selectedItemTypeIndex;

    public EditBudgetPopupViewModel(BudgetItem? item)
    {
        this.item = item;

        selectedItemTypeIndex = item.DoesDecrease ? 0 : 1;
    }
}
