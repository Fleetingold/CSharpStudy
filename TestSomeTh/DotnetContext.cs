using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestSomeTh
{
    class DotnetContext
    {
        static void DotnetContextMain(string[] args)
        {
            ContextMessage("DotnetContext Test\n");
        }

        private static void ContextMessage(string data)
        {
            Context context = Thread.CurrentContext;
            Console.WriteLine(string.Format("{0}ContextId is {1}", data, context.ContextID));
            foreach (var prop in context.ContextProperties)
                Console.WriteLine(prop.Name);
            Console.ReadLine();
        }
    }
}
