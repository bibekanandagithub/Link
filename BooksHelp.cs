using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationHelp
{

    class BookHelp<T> : BookInterface<T> where T : BookInterface
    {
        List<T> mylist = new List<T>();
        public void Add(T next)
        {
            mylist.Add(next);
        }

        public List<T> GetAll()
        {
            return mylist;
        }

        public void Remove(T Value)
        {
            mylist.Remove(Value);
        }
    }
}
