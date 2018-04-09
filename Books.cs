using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationHelp
{
    class Books : BookInterface
    {
       public int BookID { set; get; }
        public string BookName { set; get; }
        public string BookAuthor { set; get; }

    }
}
