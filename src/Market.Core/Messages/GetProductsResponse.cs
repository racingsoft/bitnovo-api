using Market.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Messages
{
    public class GetProductsResponse : IGetProductsResponse
    {
        public IList<ICatalog> Products { get; set; }
    }
}
