using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

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

    public class TodoTask
    {
        
        public TodoTask()
        {
            Id = Guid.NewGuid();
        }
        public int Order { get; set; }
        public string Content { get; set; }
        public bool Done { get; set; }
        public List<TodoTask> SubTasks { get; set; }

        [JsonIgnore]
        public Guid Id;

        [JsonIgnore]
        public ObservableCollection<TodoTask> VMSubTasks { get; set; } = new();
    }

}
