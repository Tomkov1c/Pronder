using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronder.Classes
{
    class ProjectNew
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string DateCreatd { get; set; }
        public string DateLastViewd { get; set; }
        public string DateLastEdited { get; set; }

        public string PageID { get; set; }

        public PageClass pageClass = new PageClass();
    }
}
