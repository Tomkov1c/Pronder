using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLinkIconListWorkspace
{
    public class ExternalLinkIconList
    {
        public List<Icon> Icons
        {
            get; set;
        }
    }

    public class Icon
    {
        public string Contains
        {
            get; set;
        }
        public string Path
        {
            get; set;
        }
    }
}
