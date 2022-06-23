using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStudy.ParamNullChecking
{
    internal class ParamNullChecking
    {
        public static void M(string s)
        {
            if(s is null)
            {
                throw new ArgumentNullException(nameof(s));
            }
            // Body of the method
        }

        //public static void M(string s!!)
        //{
        //    // Body of the method
        //}
    }
}