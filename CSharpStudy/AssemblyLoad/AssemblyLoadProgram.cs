using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStudy
{
    public class AssemblyLoadProgram
    {
        public static void AssemblyLoadMain()
        {
            // You must supply a valid fully qualified assembly name.
            Assembly SampleAssembly = Assembly.Load("System.data, version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            // Display all the types contained in the specified assembly.
            foreach (Type oType in SampleAssembly.GetTypes())
            {
                Console.WriteLine(oType.Name);
            }
            Console.ReadKey();
        }

        public static void SystemAssemblyMain()
        {
            string longName = "system, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            Assembly assem = Assembly.Load(longName);
            if (assem == null)
                Console.WriteLine("Unable to load assembly...");
            else
                Console.WriteLine(assem.FullName);
            Console.ReadKey();

            // The example displays the following output:
            //        system, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        }
    }
}
