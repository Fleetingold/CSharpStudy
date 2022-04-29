using SqlSugar;
using SqlSugarDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDemo
{
    internal class SqlSugarHelper
    {
        private readonly SqlSugarClient _db;

        internal SqlSugarHelper()
        {
            _db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=192.168.1.173;uid=sa;pwd=ABC!@#abc123;database=SubERPCommissionBusiness",//必填, 数据库连接字符串
                DbType = DbType.SqlServer,         //必填, 数据库类型
                IsAutoCloseConnection = true,       //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                InitKeyType = InitKeyType.Attribute    //默认SystemTable, 字段信息读取, 如：该属性是不是主键，是不是标识列等等信息
            });
        }

        internal Tuple<List<Person>, List<Device>> QueryMultiple(string sSql)
        {
            return _db.Ado.SqlQuery<Person, Device>(sSql);
        }

        #region CodeFirst
        internal void CodeFirst()
        {
            ICodeFirst codeFirst = _db.CodeFirst;
            //codeFirst.BackupTable();
            //codeFirst.InitTables();
            //codeFirst.SplitTables();
        }

        internal void CreateDatabase()
        {
            _db.DbMaintenance.CreateDatabase();
        }

        internal void InitTable()
        {
            _db.CodeFirst.InitTables(typeof(Person), typeof(Device));
        }

        internal void InitTables(params Type[] entityTypes)
        {
            _db.CodeFirst.InitTables(entityTypes);
        }
        #endregion
    }
}
