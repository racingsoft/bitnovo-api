using Market.Core.Messages;
using Market.Infrastructure.Entities;

namespace Market.Infrastructure.Converters
{
    public interface IVendorOrderRequestConverter
    {
        IVendorOrderRequest ConvertToVendorOrderRequestMessage(Catalog producto);
    }
}