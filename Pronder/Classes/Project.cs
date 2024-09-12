using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronder.Classes
{
    class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Icon { get; set; }
        public string Banner { get; set; }
        public List<ProjectDisplayPage> ProjectDisplayPage{ get; set; }
        public List<Link> Links
        {
            get; set;
        }
        public string About
        {
            get; set;
        }
        public List<Todo> Todo
        {
            get; set;
        }
    }

    public class ProjectDisplayPage
    {
        public string Preset
        {
            get; set;
        }
        public List<ExcludedSubPages> ExcludedInbuildSubPages
        {
            get; set;
        }
        public List<ExcludedSubPages> ExcludedCustomeSubPages
        {
            get; set;
        }
    }

    public class ExcludedSubPages
    {
        public bool Name
        {
            get; set;
        }
    }

    public class Link
    {
        public string Name
        {
            get; set;
        }
        public string Type
        {
            get; set;
        }
        public string Href
        {
            get; set;
        }
    }

    public class Todo
    {
        public int Order
        {
            get; set;
        }
        public string Content
        {
            get; set;
        }
        public bool Done
        {
            get; set;
        }
        public List<Sub> Sub
        {
            get; set;
        }
    }

    public class Sub
    {
        public int Order
        {
            get; set;
        }
        public string Content
        {
            get; set;
        }
        public bool Done
        {
            get; set;
        }
    }
}
