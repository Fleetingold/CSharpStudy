using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDemo.Models
{
    public class Person
    {
        public Person(string name, string text, string creator, string updater)
        {
            Name = name;
            Text = text;
            Creator = creator;
            Updater = updater;
        }

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(50)", ColumnDescription = "姓名")]//自定格式的情况 length不要设置
        public string Name { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(255)", ColumnDescription = "文本")]
        public string Text { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(50)", ColumnDescription = "创建人")]
        public string Creator { get; set; }

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(50)", ColumnDescription = "修改人")]
        public string Updater { get; set; }

        [SugarColumn(ColumnDescription = "修改时间", IsNullable = true)]
        public DateTime UpdateTime { get; set; }
    }
}
