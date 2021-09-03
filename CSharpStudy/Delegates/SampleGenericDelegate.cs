using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStudy
{
    public class SampleGenericDelegateProgram
    {
        // Type T is declared covariant by using the out keyword.
        public delegate T SampleGenericDelegate<out T>();

        public static void Test()
        {
            SampleGenericDelegate<String> dString = () => " ";

            // You can assign the dObject delegate
            // to the same lambda expression as dString delegate
            // because of the variance support for
            // matching method signatures with delegate types.
            SampleGenericDelegate<Object> dObject = () => " ";

            // The following statement generates a compiler error
            // because the generic type T is not marked as covariant.
            SampleGenericDelegate<Object> dObj = dString;
        }
    }
}
