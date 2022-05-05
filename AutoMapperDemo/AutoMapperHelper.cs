using AutoMapperDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMapperDemo
{
    internal class AutoMapperHelper
    {
        private readonly AutoMapper.IMapper _mapper;

        public AutoMapperHelper()
        {
            var config = new AutoMapper.MapperConfiguration(cfg => cfg.CreateMap<Order, OrderDto>());

            _mapper = config.CreateMapper();

            //var mapper2 = new AutoMapper.Mapper(config);
        }

        internal OrderDto GetOrderDto(Order order)
        {
            OrderDto dto = _mapper.Map<OrderDto>(order);

            if(dto != null)
            {
                return dto;
            }

            //AutoMapper also has non-generic versions of these methods, for those cases where you might not know the type at compile time.
            var mapResult = _mapper.Map(order, typeof(Order), typeof(OrderDto));
            if (mapResult is OrderDto dto2)
            {
                return dto2;
            }

            return new OrderDto(0, string.Empty);
        }
    }
}