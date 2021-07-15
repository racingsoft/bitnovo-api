using Market.Core.DTO;

namespace Market.Infrastructure.Converters
{
    public interface IOrderConverter
    {
        IOrder ConvertToDTO(Entities.Order order);
    }
}