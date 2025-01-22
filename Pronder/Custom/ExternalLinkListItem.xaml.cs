using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Pronder.Custom
{
    public sealed partial class ExternalLinkListItem : UserControl
    {
        public ExternalLinkListItem()
        {
            this.InitializeComponent();
        }

        public string Icon
        {
            get
            {
                return (string)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(string), typeof(ExternalLinkListItem), new PropertyMetadata(null));

        public string Name
        {
            get
            {
                return (string)GetValue(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(nameof(Name), typeof(string), typeof(ExternalLinkListItem), new PropertyMetadata(string.Empty));

        public string Path
        {
            get
            {
                return (string)GetValue(PathProperty);
            }
            set
            {
                SetValue(PathProperty, value);
            }
        }

        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register(nameof(Path), typeof(string), typeof(ExternalLinkListItem), new PropertyMetadata(string.Empty));
    }
}
