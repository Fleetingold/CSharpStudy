// See https://aka.ms/new-console-template for more information
using Flurl;
using Flurl.Http;
using FlurlDemo.Models;

Console.WriteLine("Hello, World!");

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