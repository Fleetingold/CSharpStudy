using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDemo
{
    internal class Device
    {
        public Device(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
