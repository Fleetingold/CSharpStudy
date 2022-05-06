// See https://aka.ms/new-console-template for more information
using Flurl;
using Flurl.Http;
using FlurlDemo.Models;

Console.WriteLine("Hello, World!");

DateTime dt = DateTime.Now;
var resoponse = await "http://60.170.183.213:2587/wms/purchin/writebackinfo".PostJsonAsync(new
{
    ID = Guid.NewGuid(),
    OrderNo = "JH-2022-05-03-000071",
    StoreID = Guid.NewGuid(),
    StoreNum = "",
    StoreType = "",
    StoreArea = "",
    StoreLocation = "",
    DrugNum = "",
    BatchNo = "",
    ProductDate = "",
    DueDate = "",
    ReceiptQuantity = 10.0m,
    ReceiptRejectQuantity = 0.0m,
    Receipter = "",
    ReceiptTime = dt,
    ReceiptRemark = "",
    CheckQuantity = 10.0m,
    CheckRejectQuantity = 0.0m,
    Checker = "",
    CheckTime = dt,
    CheckRemark = "",
    CheckerND = "",
    KeepQuantity = 10.0m,
    KeepRejectQuantity = 0.0m,
    Keeper = "",
    KeepTime = dt,
    KeepRemark = "",
    ErpSn = 3
}).ReceiveString();

// Flurl will use 1 HttpClient instance per host
var person = await "https://api.com"
    .AppendPathSegment("person")
    .SetQueryParams(new { a = 1, b = 2 })
    .WithOAuthBearerToken("my_oauth_token")
    .PostJsonAsync(new
    {
        first_name = "Claire",
        last_name = "Underwood"
    })
    .ReceiveJson<Person>();

Console.ReadKey();