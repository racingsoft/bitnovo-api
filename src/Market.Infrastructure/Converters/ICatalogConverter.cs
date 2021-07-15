namespace Market.Infrastructure.Converters
{
    public interface ICatalogConverter
    {
        Core.DTO.ICatalog ConvertToDTO(Entities.Catalog catalog);
    }
}