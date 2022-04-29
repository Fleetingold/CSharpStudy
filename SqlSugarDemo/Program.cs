// See https://aka.ms/new-console-template for more information
using System.Reflection;

Console.WriteLine("Hello, World!");

SqlSugarDemo.SqlSugarHelper sugarHelper = new SqlSugarDemo.SqlSugarHelper();

#region CodeFirst
//sugarHelper.CreateDatabase();

//sugarHelper.InitTable();

//批量处理
Type[] types = Assembly.LoadFrom("SqlSugarDemo.dll").GetTypes()
               .Where(it => !string.IsNullOrEmpty(it.FullName) && it.FullName.Contains("SqlSugarDemo.Models."))
               .ToArray();

//sugarHelper.InitTables(types);

sugarHelper.BackupTable();
#endregion

string sSql = "select * from Person; select * from Device";
var queryResult = sugarHelper.QueryMultiple(sSql);
var personlist = queryResult.Item1;
var devicelist = queryResult.Item2;

Console.WriteLine($"Person:{personlist?.Count} Device:{devicelist?.Count}");

Console.ReadKey();