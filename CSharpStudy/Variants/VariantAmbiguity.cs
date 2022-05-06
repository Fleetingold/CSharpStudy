using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStudy
{
    // Simple class hierarchy
    class Animal { }
    class Cat : Animal { }
    class Dog : Animal { }

    // This class introduces ambiguity
    // because IEnumerable<out T> is covariant.
    class Pets : IEnumerable<Cat>, IEnumerable<Dog>
    {
        IEnumerator<Cat> IEnumerable<Cat>.GetEnumerator()
        {
            Console.WriteLine("Cat");
            // Some code.
            return null;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            // Some code.
            return null;
        }

        public IEnumerator<Dog> GetEnumerator()
        {
            Console.WriteLine("Dog");
            // Some code.
            return null;
        }
    }

    class VariantAmbiguity
    {
        public static void VariantAmbiguityMain()
        {
            IEnumerable<Animal> pets = new Pets();
            pets.GetEnumerator();
        }
    }
}
