using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapperDemo.Models
{
    internal class Order
    {
        public Order(int id, string orderNo)
        {
            Id = id;
            OrderNo = orderNo;
        }

        public int Id { get; set; }

        public string OrderNo { get; set; }
    }
}
