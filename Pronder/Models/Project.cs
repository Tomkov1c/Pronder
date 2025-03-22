using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace Pronder.Models
{
    public class Project
    {
        public static Project? GlobalInstance { get; private set; } = null;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Icon { get; set; }
        public string Banner { get; set; }
        public List<Link> Links { get; set; } = new();
        public string About { get; set; }
        public List<TodoTask> Todo { get; set; } = new();
        public string DateCreated { get; set; }
        public string DateLastViewed { get; set; }
        public string DateLastEdited { get; set; }

        public static void SetGlobalInstance(Project project) => GlobalInstance = project;

        public bool IconNullOrEmpty() => string.IsNullOrWhiteSpace(Icon) && string.IsNullOrWhiteSpace(Icon);
        public bool BannerNullOrEmpty() => string.IsNullOrWhiteSpace(Banner) && string.IsNullOrWhiteSpace(Banner);
        public bool LinksNullOrEmpty() => Links is null || !Links.Any();
    }

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
                    case "github":
                        key = "Github";
                        break;

                    case "behance":
                        key = "Behance";
                        break;

                    case "facebook":
                        key = "Facebook";
                        break;

                    case "music.apple":
                        key = "AppleMusic";
                        break;

                    case "instagram":
                        key = "Instagram";
                        break;

                    case "soundcloud":
                        key = "Soundcloud";
                        break;

                    case "spotify":
                        key = "Spotify";
                        break;

                    case "x":
                    case "twitter":
                        key = "X";
                        break;

                    case "youtube":
                        key = "Youtube";
                        break;

                    case "music.youtube":
                        key = "YoutubeMusic";
                        break;

                    case "ko-fi":
                        key = "KoFi";
                        break;

                    default:
                        key = "Website";
                        break;
                }

                if(!string.IsNullOrEmpty(key))
                {
                    return (BitmapImage)Application.Current.Resources["Icon8" + key];
                }


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
        public int Order { get; set; }
        public string Content { get; set; }
        public bool Done { get; set; }
        public List<Sub> Sub { get; set; } = new();
    }

    public class Sub
    {
        public int Order { get; set; }
        public string Content { get; set; }
        public bool Done { get; set; }
    }
}
