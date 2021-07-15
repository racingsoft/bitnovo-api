using Market.Core.Factories;
using Market.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Market.Core.DTO;

namespace Market.Infrastructure.Converters
{
    public class OrderConverter : IOrderConverter
    {
        private readonly IFactory<DTO.IOrder> _orderFactory;

        public OrderConverter(IFactory<DTO.IOrder> orderFactory)
        {
            _orderFactory = orderFactory;
        }

        public DTO.IOrder ConvertToDTO(Order order)
        {
            var orderDTO = _orderFactory.Create();

            orderDTO.Id = order.Id;
            orderDTO.CatalogId = order.CatalogId;
            orderDTO.ExternalId = order.ExternalId;
            orderDTO.Amount = order.Amount;

            return orderDTO;
        }
    }
}
