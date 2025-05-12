using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;

namespace Pronder.Models;
public class Link : ObservableObject
{
    private string name;
    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    private string type;
    public string Type
    {
        get => type;
        set => SetProperty(ref type, value);
    }

    private string href;
    public string Href
    {
        get => href;
        set => SetProperty(ref href, value);
    }

    [JsonIgnore] public Guid Id { get; set; } 
    [JsonIgnore] public BitmapImage Icon { get; set; } 

    public Link()
    {
        Id = new Guid();
    }

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
