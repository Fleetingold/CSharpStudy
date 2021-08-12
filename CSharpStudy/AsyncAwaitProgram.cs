using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpStudy
{
    class AsyncAwaitProgram
    {
        public static void AsyncAwaitMain()
        {
            Console.WriteLine("Hey David, How much is 98745 divided by 7?");

            Task<int> david = ThinkAboutIt();

            Console.WriteLine("While he thinks, lets chat about the weather for a bit.");
            Console.WriteLine("Do you think it's going to rain tomorrow?");
            Console.WriteLine("No, I think it should be sunny.");

            david.Wait();
            var davidsAnswer = david.Result;

            Console.WriteLine($"David: {davidsAnswer}");

            Console.ReadKey();
        }

        private static async Task<int> ThinkAboutIt()
        {
            await ReadTheManual();

            Console.WriteLine("Think I got it.");

            return (98745 / 7);
        }

        private static async Task ReadTheManual()
        {
            string file = @"F:\HowToCalc.txt";
            if (File.Exists(file) == false)
            {
                Console.WriteLine("file not found: " + file);
            }
            else
            {
                Console.WriteLine("Reading a manual.");

                using (StreamReader reader = new StreamReader(file))
                {
                    string text = await reader.ReadToEndAsync();
                }

                Console.WriteLine("Done.");
            }
        }
    }

    public class AsyncAwaitProgram2
    {
        public static void AsyncAwait2Main()
        {
            MyAsyncMethod();
            string input = "";
            while ((input = Console.ReadLine()) != "q")
            {
                Console.WriteLine($"your input is {input}");
            }
        }

        private static async void MyAsyncMethod()
        {
            Console.WriteLine("call MyAsyncMethod.....");
            Task<int> calculateResult = CalculateAsync();
            Console.WriteLine("MyAsyncMethod was called.......");
            int result = await calculateResult;
            Console.WriteLine($"result is {result}");
        }

        private static async Task<int> CalculateAsync()
        {
            Console.WriteLine("calculation begin.....");
            int t = await Task.Run(() => Calculate());
            Console.WriteLine("calculation complete.....");
            return t;
        }

        private static int Calculate()
        {
            // Compute total count of digits in strings.
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(5000);
                Console.WriteLine($"calculating step {i + 1}");
            }
            Random random = new Random();
            return random.Next();
        }
    }

    public class AsyncAwaitProgram3
    {
        static void AsyncAwaitProgram3Main(string[] args)
        {
            Test();
            Console.WriteLine("Main End");
            Console.Read();
        }


        static async void Test()
        {
            var task1 = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("task1");
            });
            var task2 = Task.Run(() =>
            {
                Thread.Sleep(5000);
                Console.WriteLine("task2");
            });

            await Task.WhenAny(new Task[] { task1, task2 });
            Console.WriteLine("有一个任务完成");
            await Task.WhenAll(new Task[] { task1, task2 });
            Console.WriteLine("全部完成！");
        }
    }

    #region AsyncStateMachineProgram

    // This class is used by the code below to discover method attributes.
    public class TheClass
    {
        public async Task<int> AsyncMethod()
        {
            await Task.Delay(5);
            return 1;
        }

        public int RegularMethod()
        {
            return 0;
        }
    }

    public class AsyncStateMachineProgram
    {
        private static bool IsAsyncMethod(Type classType, string methodName)
        {
            // Obtain the method with the specified name.
            MethodInfo method = classType.GetMethod(methodName);

            Type attType = typeof(AsyncStateMachineAttribute);

            // Obtain the custom attribute for the method.
            // The value returned contains the StateMachineType property.
            // Null is returned if the attribute is't present for the method.
            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);

            #region MyCode
            var attr = method.GetCustomAttribute(attType);

            if (attr is AsyncStateMachineAttribute) 
            {
                return true;
            }
            #endregion

            return attrib != null;
        }

        private static void ShowResult(Type classType, string methodName)
        {
            Console.Write(methodName + ": ".PadRight(16));

            if (IsAsyncMethod(classType, methodName))
                Console.WriteLine("Async method");
            else
                Console.WriteLine("Regular method");
        }

        static void AsyncStateMachineMain(string[] args)
        {
            ShowResult(classType: typeof(TheClass), methodName: nameof(TheClass.AsyncMethod));
            ShowResult(classType: typeof(TheClass), methodName: nameof(TheClass.RegularMethod));

            // Note: The IteratorStateMachineAttribute applies to Visual Basic methods
            // but not C# methods.

            Console.ReadKey(true);

            // Output:
            //  AsyncMethod:    Async method
            //  RegularMethod:  Regular method
        }
    }

    #endregion
}
