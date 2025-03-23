using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Dynamic;
using static Pronder.Models.ProjectExtraProperties;
using Newtonsoft.Json;

namespace Pronder.Models
{
    // C:\Users\gamin\AppData\Local\Packages\90d93993-b7aa-4fff-9757-12ef0c6c27e0_1116rh51nqx02\LocalState\Projects
    public class Project
    {
        public static Project? GlobalInstance { get; private set; } = null;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Icon { get; set; }
        public string Banner { get; set; }
        public string About { get; set; }
        public string DateCreated { get; set; }
        public string DateLastViewed { get; set; }
        public string DateLastEdited { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Link> Links { get; set; } = null;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TodoTask> TodoTasks { get; set; } = null;


        public static void SetGlobalInstance(Project project) => GlobalInstance = project;

        public bool IconNullOrEmpty() => string.IsNullOrWhiteSpace(Icon);
        public bool BannerNullOrEmpty() => string.IsNullOrWhiteSpace(Banner);
        public bool LinksNullOrEmpty() => Links is null || !Links.Any();

    }
}
