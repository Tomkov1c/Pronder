using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace Pronder.Classes
{
    public class NavigationService
    {
        public readonly Frame? _frame;
        public readonly NavigationView? _view;

        public NavigationService(Frame? frame, NavigationView? view)
        {
            _frame = frame;
            _view = view;
        }

        public void NavigateTo(string? pageName)
        {
            var fullTypeName = $"Pronder.Views.{pageName}";
            var pageType = Type.GetType(fullTypeName);

            if (pageType != null)
            {
                _view.SelectedItem = null;
                _frame.Navigate(pageType, null, new DrillInNavigationTransitionInfo());
            }
            else
            {
                Debug.WriteLine($"Page not found: {fullTypeName}");
            }
        }
    }
}
