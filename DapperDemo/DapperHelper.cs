using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using static Dapper.SqlMapper;

namespace DapperDemo
{
    internal class DapperHelper
    {
        internal static Tuple<List<Person>, List<Device>> QueryMultiple(string sSql)
        {
            //Dapper.SqlMapper

            using (var connection = new SqliteConnection("Data Source=./dapperdemo.db"))
            {
                using (GridReader multi = connection.QueryMultiple(sSql))
                {
                    var persons = multi.Read<Person>().ToList();
                    var devices = multi.Read<Device>().ToList();

                    return new Tuple<List<Person>, List<Device>>(persons, devices);
                }
            }
        }
    }
}
