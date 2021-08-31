using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStudy
{
    class Base
    {
        public static void PrintBases(IEnumerable<Base> bases)
        {
            foreach(Base b in bases)
            {
                Console.WriteLine(b);
            }
        }
    }

    class Derived : Base
    {

    }

    public class CovariantDemo
    {
        public static void CovariantMain()
        {
            List<Derived> dlist = new List<Derived>();

            Base.PrintBases(dlist);
            IEnumerable<Base> bIEnum = dlist;
        }
    }
}
