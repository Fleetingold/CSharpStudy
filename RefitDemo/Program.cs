using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;

namespace RefitDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var testApi = RestService.For<ITestApi>("http://localhost:52044/");

            var str = await testApi.GetTest();
            Console.WriteLine("获得的返回string为：" + str);
            Console.ReadKey();
        }
    }

    interface ITestApi
    {
        [Get("/JobManager/Test1")]
        Task<string> GetTest();
    }
}
