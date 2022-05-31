using Asp.Net_Core_WebApi.Models;
using JWT.Algorithms;
using JWT.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to Microsoft.EntityFrameworkCore
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthentication(options =>
{
    // Prevents from System.InvalidOperationException: No authenticationScheme was specified, and there was no DefaultAuthenticateScheme found.
    options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;

    // Prevents from System.InvalidOperationException: No authenticationScheme was specified, and there was no DefaultChallengeScheme found.
    options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
})
.AddJwt(options =>
{
    options.Keys = null;
    options.VerifySignature = true;
});

string ServerRsaPublicKey2 = "MIIDfDCCAmSgAwIBAgIQQDCxkdjCQqmQsnSLtcHj3TANBgkqhkiG9w0BAQsFADA7MQswCQYDVQQGEwJ1czELMAkGA1UECBMCVVMxETAPBgNVBAoTCENlcnR0ZXN0MQwwCgYDVQQDEwNqd3QwHhcNMjAwMzIzMDI1NDAzWhcNMjMwMzIzMDMwNDAzWjA7MQswCQYDVQQGEwJ1czELMAkGA1UECBMCVVMxETAPBgNVBAoTCENlcnR0ZXN0MQwwCgYDVQQDEwNqd3QwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQC5hM+0cIjO0oLcxQPGdnSS0ZVJDSNVsPmtiXimSLhEIPczbZ35OSWa9PI+PRIztr/yjtwjTlCES4EjyEoJ8LYIQmGVLdYV5ULS/CyXVpgWpDdiSv6QOwB2qMv3mKiPcmaKxy+oo4zfihBqGkCC6QnspyvUFPZiWTx86Apw3u3WqBRE3HQ+PjMnjDSnWdPaAsb75ti61RU+9qYj3BwxDJR6xnAaYz1RSkxHOw4+Ty+/tNtObrZmTH7msVRpV7kMU1QgyD3Y2/JTTf3YUU0LCm1J+WJ0cMbVrILAvVlOQnRn3IlcI1LOL/e6XEyET5tVymv8S5EoJjGf2o8VnTsF3vttAgMBAAGjfDB6MA4GA1UdDwEB/wQEAwIFoDAJBgNVHRMEAjAAMB0GA1UdJQQWMBQGCCsGAQUFBwMBBggrBgEFBQcDAjAfBgNVHSMEGDAWgBTTMvXgytSFWwQk58CpxCpZAr5G1jAdBgNVHQ4EFgQU0zL14MrUhVsEJOfAqcQqWQK+RtYwDQYJKoZIhvcNAQELBQADggEBAK5vSwzh0x0pJm6njJX29rsd53ktyph+L90Enh0xzLFN0Ku9p+tM8E9TmKR+9ppdPqIEe4G/AuR1fHvmWenEw44M85Y/pBIPZDM2QVQngjg6iRQ42yD5hb/P4+UnvP9a5uI4Xc3f4NlJi3n54qBmdD5Hg52tNYgr8FKRoNzAoUCHelLk5PW0llF8Nc6cjJf0JfrSA1lVua488Dd34sPt798xM3IoISof1dqKslTypHP4BCyZ55SSfQJ+GrY7T9J3ct23BTrPnhhq0sPDogN4j258RmDriBGZmRcnrlmuBD5v+lvjYk0fISYNMfkrCQg5zae4d6BJIZVLY3gITGbaNoA=";

X509Certificate2 x509Certificate2 = new X509Certificate2(Convert.FromBase64String(ServerRsaPublicKey2));
RS256Algorithm rS256Algorithm = new RS256Algorithm(x509Certificate2);
builder.Services.AddSingleton<IAlgorithmFactory>(new DelegateAlgorithmFactory(rS256Algorithm));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.Use(async (HttpContext context, Func<Task> next) =>
{
    var authenticationResult = await context.AuthenticateAsync();
    if (authenticationResult.Succeeded)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = new ContentType("text/json").MediaType;

        await context.Response.WriteAsync("Hello");
    }
    else
    {
        await context.ChallengeAsync();
    }
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/", () => "This is a GET");
app.MapPost("/", () => "This is a POST");
app.MapPut("/", () => "This is a PUT");
app.MapDelete("/", () => "This is a DELETE");

app.MapPost("/person", (Person person) => { });

app.MapPost("/person2", async (HttpRequest request) =>
{
    var person = await request.ReadFromJsonAsync<Person>();

    // ...
});

app.MapPost("/products", (Product? product) => { });

app.MapPost("/product", (Product product) => product);

app.MapPost("/uploadstream", async (IConfiguration config, HttpRequest request) =>
{
    var filePath = Path.Combine(config["StoredFilesPath"], Path.GetRandomFileName());

    await using var writeStream = File.Create(filePath);
    await request.BodyReader.CopyToAsync(writeStream);
});

app.MapPost("/wms/purchin/writebackinfo", (PurchInWriteBackInfoParam param) =>
{
    return ApiResult.From(ReturnCode.OK, "³É¹¦");
});

app.MapGet("/hello", () => Results.Ok(new { Message = "Hello World" }));

//app.MapGet("/api/todoitems/{id}", async (int id, TodoDb db) =>
//         await db.Todos.FindAsync(id)
//         is Todo todo
//         ? Results.Ok(todo)
//         : Results.NotFound())
//   .Produces<Todo>(StatusCodes.Status200OK)
//   .Produces(StatusCodes.Status404NotFound);

app.MapGet("/hellojson", () => Results.Json(new { Message = "Hello World" }));

app.MapGet("/405", () => Results.StatusCode(405));

app.MapGet("/text", () => Results.Text("This is some text"));

var proxyClient = new HttpClient();
app.MapGet("/pokemon", async () =>
{
    var stream = await proxyClient.GetStreamAsync("http://consoto/pokedex.json");
    // Proxy the response as JSON
    return Results.Stream(stream, "application/json");
});

app.MapGet("/old-path", () => Results.Redirect("/new-path"));

app.MapGet("/download", () => Results.File("myfile.text"));

app.MapGet("/todoitems", async (TodoDb db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/complete", async (TodoDb db) =>
    await db.Todos.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());

app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}