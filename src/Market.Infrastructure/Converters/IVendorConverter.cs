namespace Market.Infrastructure.Converters
{
    public interface IVendorConverter
    {
        Core.DTO.IVendor ConvertToDTO(Entities.Vendor vendor);
    }
}