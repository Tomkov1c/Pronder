using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace Pronder.Classes
{
    class NavigationService
    {
        private static NavigationService _instance;
        public static NavigationService Instance => _instance ?? (_instance = new NavigationService());

        public NavigationView NavigationView
        {
            get; set;
        }
        public NavigationViewItem ActiveItem
        {
            get; set;
        }
    }
}
