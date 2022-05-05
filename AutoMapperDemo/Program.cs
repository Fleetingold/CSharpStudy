// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

AutoMapperDemo.AutoMapperHelper autoMapper = new AutoMapperDemo.AutoMapperHelper();

AutoMapperDemo.Models.Order order = new AutoMapperDemo.Models.Order(1, "a0001");

var orderDto = autoMapper.GetOrderDto(order);

Console.ReadKey();