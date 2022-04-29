using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDemo.Models
{
    public class Device
    {
        public Device(string description, string deviceName)
        {
            Description = description;
            DeviceName = deviceName;
        }

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(50)", ColumnDescription = "设备名称")]//自定格式的情况 length不要设置
        public string DeviceName { get; set; }

        [SugarColumn(ColumnDataType = "Nvarchar(255)", ColumnDescription = "设备描述")]
        public string Description { get; set; }
    }
}
