using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomationHelp
{
   public interface BookInterface
    {

    }
    public interface BookInterface<T>
    {
        void Add(T next);
        void Remove(T Value);
        List<T> GetAll();

    }
}
