using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.System;
using Pronder.Models;
using Windows.Security.Cryptography.Core;
using Windows.Storage;

namespace Pronder.Helpers.Mine
{
    public class ExternalLinkHelper
    {
        public async Task<string> GetIconPath(Link link)
        {
            string internalPath = "ms-appx:///Assets/Icon8/Color/Brands/";
            string fileName = "";

            if (link.Type == "link")
            {
                System.Uri uri = new System.Uri(link.Href.ToLower());
                string domain = uri.Host.StartsWith("www.") ? uri.Host.Substring(4) : uri.Host;
                string domainName = domain.Split('.')[0];

                switch (domainName)
                {
                    case "github":
                        fileName = "icons8-github-512.png";
                        break;

                    case "behance":
                        fileName = "icons8-behance-512.png";
                        break;

                    case "facebook":
                        fileName = "icons8-facebook-512.png";
                        break;

                    case "music.apple":
                        fileName = "icons8-apple-music-512.png";
                        break;

                    case "instagram":
                        fileName = "icons8-instagram-512.png";
                        break;

                    case "soundcloud":
                        fileName = "icons8-soundcloud-512.png";
                        break;

                    case "spotify":
                        fileName = "icons8-spotify-512.png";
                        break;

                    case "x":
                    case "twitter":
                        fileName = "icons8-x-512.png";
                        break;

                    case "youtube":
                        fileName = "icons8-youtube-512.png";
                        break;

                    case "music.youtube":
                        fileName = "icons8-youtube-music-512.png";
                        break;

                    case "ko-fi":
                        fileName = "icons8-ko-fi-512.png";
                        break;

                    default:
                        fileName = "icons8-website-512.png";
                        break;
                }

                if(!string.IsNullOrEmpty(fileName))
                {
                    StorageFile iconFile = await StorageFile.GetFileFromApplicationUriAsync(new System.Uri(internalPath + fileName));
                    return iconFile.Path;
                }
            }
            else if (link.Type == "path")
            {
                StorageFile iconFile = await StorageFile.GetFileFromApplicationUriAsync(new System.Uri("ms-appx:///Assets/Icon8/Color/icons8-folder-512.png"));
                return iconFile.Path;
            }
            return null;
        }
    }
    
}
