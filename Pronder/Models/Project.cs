using System;
using System.Collections.Generic;
using System.Linq;

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
