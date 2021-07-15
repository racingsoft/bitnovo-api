using Market.Core.DTO;
using System.Collections.Generic;

namespace Market.Core.Messages
{
    public interface IGetProductsResponse
    {
        IList<ICatalog> Products { get; set; }
    }
}