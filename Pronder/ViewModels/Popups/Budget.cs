using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class EditBudgetPopupViewModel : ObservableRecipient
{
    [ObservableProperty] BudgetItem? item;

    private int selectedItemTypeIndex;
    public int SelectedItemTypeIndex
    {
        get => selectedItemTypeIndex;
        set
        {
            if (SetProperty(ref selectedItemTypeIndex, value))
            {
                if (Item != null)
                {
                    Item.DoesDecrease = value == 0;

                    if (Item.DoesDecrease)
                    {
                        Item.Foreground = Application.Current.Resources["SystemFillColorCriticalBrush"] as SolidColorBrush;
                    }
                    else
                    {
                        Item.Foreground = Application.Current.Resources["SystemFillColorSuccessBrush"] as SolidColorBrush;
                    }
                }
            }
        }
    }

    public EditBudgetPopupViewModel(BudgetItem? item)
    {
        this.item = item;

        selectedItemTypeIndex = item.DoesDecrease ? 0 : 1;
    }
}
