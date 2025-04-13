using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using static Pronder.Models.ProjectExtraProperties;

namespace Pronder.Models;
public class ProjectExtraProperties
{
    public class Link
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Href { get; set; }

        public BitmapImage IconFinder()
        {
            if (this.Type == "link")
            {
                string key;
                System.Uri uri = new System.Uri(this.Href.ToLower());
                string domain = uri.Host.StartsWith("www.") ? uri.Host.Substring(4) : uri.Host;
                string domainName = domain.Split('.')[0];

                switch (domainName)
                {
                    case "github": key = "Github"; break;
                    case "behance": key = "Behance"; break;
                    case "facebook": key = "Facebook"; break;
                    case "music.apple": key = "AppleMusic"; break;
                    case "instagram": key = "Instagram"; break;
                    case "soundcloud": key = "Soundcloud"; break;
                    case "spotify": key = "Spotify"; break;
                    case "x":
                    case "twitter": key = "X"; break;
                    case "youtube": key = "Youtube"; break;
                    case "music.youtube": key = "YoutubeMusic"; break;
                    case "ko-fi": key = "KoFi"; break;
                    default: key = "Website"; break;
                }

                return (BitmapImage)Application.Current.Resources["Icon8" + key];
            }
            else if (this.Type == "path")
            {
                return (BitmapImage)Application.Current.Resources["Icon8Folder"];
            }
            return null;
        }
    }

    public class TodoTask: INotifyPropertyChanged
    {
        [JsonIgnore]
        public Guid Id { get; private set; }

        public TodoTask()
        {
            Id = Guid.NewGuid();
        }

        private bool _done;
        public bool Done
        {
            get => _done;
            set
            {
                if (_done != value)
                {
                    _done = value;
                    OnPropertyChanged(nameof(Done));
                }
            }
        }

        private int _order;
        public int Order
        {
            get => _order;
            set
            {
                if (_order != value)
                {
                    _order = value;
                    OnPropertyChanged(nameof(Order));
                }
            }
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private DateTime _deadline;
        public DateTime Deadline
        {
            get => _deadline;
            set
            {
                if (_deadline != value)
                {
                    _deadline = value;
                    OnPropertyChanged(nameof(Deadline));
                }
            }
        }

        private List<TodoTask> _subTasks;
        public List<TodoTask> SubTasks
        {
            get => _subTasks;
            set
            {
                if (_subTasks != value)
                {
                    _subTasks = value;
                    OnPropertyChanged(nameof(SubTasks));
                }
            }
        }

        public bool IsExpanded
        {
            get; set;
        }


        [JsonIgnore]
        public ObservableCollection<TodoTask> VMSubTasks { get; set; } = new ObservableCollection<TodoTask>();

        public bool TodoTasksNullOrEmpty() => SubTasks == null || SubTasks.Count <= 0;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
