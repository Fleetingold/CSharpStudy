﻿// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

AutoMapperDemo.AutoMapperHelper autoMapper = new AutoMapperDemo.AutoMapperHelper();

AutoMapperDemo.Models.Order order = new AutoMapperDemo.Models.Order();

var orderDto = autoMapper.GetOrderDto(order);

Console.ReadKey();