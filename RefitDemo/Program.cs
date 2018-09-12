using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

            var foo = new Foo
            {
                Bar = "a Foo"
            };
            var str2 = await testApi.CreateFoo(foo);
            Console.WriteLine("获得的返回string为：" + str2);

            var str3 = await testApi.CreateFoo(new Foo
            {
                Bar = "a Foo from json"
            });
            Console.WriteLine("获得的返回string为：" + str3);

            //通用泛型定义接口
            var api = RestService.For<IReallyExcitingCrudApi<Foo, string>>("http://localhost:52044/");

            Foo result = await api.Create(new Foo
            {
                Bar = "a 通用泛型定义接口 Foo "
            });
            Console.WriteLine("获得的返回result.Bar为：" + result.Bar);
            Console.ReadKey();
        }
    }

    interface ITestApi
    {
        [Get("/JobManager/Test")]
        Task<string> GetTest();

        [Post("/JobManager/Test1")]
        Task<string> CreateFoo([Body] Foo foo);
    }

    class Foo
    {
        //[JsonProperty(PropertyName = "b")]
        public string Bar { get; set; }
    }

    //通用泛型定义接口
    public interface IReallyExcitingCrudApi<T, in TKey> where T : class
    {
        [Post("/JobManager/Test2")]
        Task<T> Create([Body] T payload);

        [Get("")]
        Task<List<T>> ReadAll();

        [Get("/{key}")]
        Task<T> ReadOne(TKey key);

        [Put("/{key}")]
        Task Update(TKey key, [Body]T payload);

        [Delete("/{key}")]
        Task Delete(TKey key);
    }
}
