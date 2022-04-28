// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

SqlSugarDemo.SqlSugarHelper sugarHelper = new SqlSugarDemo.SqlSugarHelper();

string sSql = "select * from Person; select * from Device";
var queryResult = sugarHelper.QueryMultiple(sSql);
var personlist = queryResult.Item1;
var devicelist = queryResult.Item2;

Console.WriteLine($"Person:{personlist.Count} Device:{devicelist.Count}");

Console.ReadKey();