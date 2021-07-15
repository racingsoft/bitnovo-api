using Market.Core.Adapters;
using Market.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Market.Core.Services
{
    public interface IMarketService
    {
        Task<IList<ICatalog>> GetProductsAsync();
        Task SyncCatalogAsync();
        Task<IOrder> CreateOrderAsync(int id);
    }
}