using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStudy
{
    internal class HttpClientProgram
    {
        static readonly HttpClient _httpClient = new HttpClient();

        public static void Main()
        {
            string getUri = "http://60.170.183.213:2587";

            GetAsync(getUri);

            string param = "{\"ID\":\"7f953fd4-7ab4-48f4-a9fa-6d6db1780a00\",\"OrderNo\":\"PDDWLH00009455\",\"StoreID\":\"4e68b489-ee4b-47d7-b12d-73c8f72b4561\",\"StoreNum\":\"103\",\"StoreType\":\"常温\",\"StoreArea\":null,\"StoreLocation\":\"103\",\"DrugNum\":\"ZC000571\",\"BatchNo\":\"123456\",\"ProductDate\":\"2022-01-01\",\"DueDate\":\"2022-08-01\",\"ReceiptQuantity\":500.000,\"ReceiptRejectQuantity\":0.0,\"Receipter\":\"刘金飞\",\"ReceiptTime\":\"2022-04-12T11:29:34.15\",\"ReceiptRemark\":\"\",\"CheckQuantity\":500.000,\"CheckRejectQuantity\":0.0,\"Checker\":\"刘金飞\",\"CheckTime\":\"2022-04-12T11:33:42.407\",\"CheckRemark\":\"合格\",\"CheckerND\":\"\",\"KeepQuantity\":500.000,\"KeepRejectQuantity\":0.0,\"Keeper\":\"刘金飞\",\"KeepTime\":\"2022-04-12T11:34:01.863\",\"KeepRemark\":\"合格\",\"ErpSn\":3}";

            string postUri = "http://60.170.183.213:2587/wms/purchin/writebackinfo";

            StringContent httpContent = new StringContent(param, Encoding.UTF8, "application/json");

            PostAsync(postUri, httpContent);

            SendAsync(postUri, httpContent);

            Console.ReadKey();
        }

        private static void GetAsync(string requestUri)
        {
            for (int i = 0; i < 10; i++)
            {
                var response = _httpClient.GetAsync(requestUri).GetAwaiter().GetResult();

                string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Console.WriteLine($"{i}:{response.StatusCode} Content:{responseContent}");
            }
        }

        private static void PostAsync(string requestUri, StringContent httpContent)
        {
            HttpResponseMessage response = _httpClient.PostAsync(requestUri, httpContent).GetAwaiter().GetResult();

            string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            Console.WriteLine($"Post {response.StatusCode} Content:{responseContent}");
        }

        private static void SendAsync(string requestUri, StringContent httpContent)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage()
            {
                Content = httpContent,
                Method = HttpMethod.Post,
                RequestUri = new Uri(requestUri),
                //Version = new Version(2, 0)
            };

            //当前仅支持 HTTP/1.0 和 HTTP/1.1 版本的请求。
            HttpResponseMessage response = _httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();

            string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Console.WriteLine($"Send {response.StatusCode} Content:{responseContent}");
        }
    }
}
