using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using Pronder.Classes;
using Pronder.Models;

namespace Pronder.ViewModels;

public partial class EditProjectPagesExternalLinksViewModel : ObservableRecipient
{
    public ICommand AddItemCommand { get; private set; }
    public ICommand RemoveItemCommand { get; private set; }

    public static Project? _project => Project.GlobalInstance;

    public ObservableCollection<Link> Links { get; set; } = new();

    private int _selectedItemTypeIndex;
    public int SelectedItemTypeIndex
    {
        get => _selectedItemTypeIndex;
        set
        {
            _selectedItemTypeIndex = value; OnPropertyChanged();
        }
    }

    [ObservableProperty] public string newItemName;
    [ObservableProperty] public string newItemHref;

    public EditProjectPagesExternalLinksViewModel()
    {
        AddItemCommand = new RelayCommand(AddItem);
        RemoveItemCommand = new RelayCommand<Link?>(RemoveItem);

        Links.CollectionChanged += (e, s) => Save();
        if(_project.Links == null)
        {
            _project.Links = new();
        }
        foreach (Link link in _project.Links)
        {
            link.PropertyChanged += (e, s) => Save();
            link.Icon = link.IconFinder();
            Links.Add(link);
        }
    }

    private void AddItem()
    {
        var newItem = new Link
        {
            Name = NewItemName,
            Href = NewItemHref,
            Type = SelectedItemTypeIndex == 0 ? "link" : "path",
        };
        NewItemName = null;
        NewItemHref = null;
        SelectedItemTypeIndex = 0;

        newItem.Icon = newItem.IconFinder();
        Links.Add(newItem);
    }
    private void RemoveItem(Link? itemToRemove)
    {
        foreach (Link item in Links)
        {
            if (item.Id == itemToRemove.Id)
            {
                Links.Remove(itemToRemove);

                return;
            }
        }
    }


    private void Save()
    {
        _project.Links = Links.ToList();
        _project.SaveToFile();
    }
}
