using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO = Market.Core.DTO;
using Market.Core.Factories;
using Market.Infrastructure.Entities;
using Market.Infrastructure.Repositories;

namespace Market.Infrastructure.Converters
{
    public class CatalogConverter : ICatalogConverter
    {
        private readonly IVendorRepository _vendorRepository;
        private readonly IFactory<DTO.ICatalog> _catalogFactory;
        private readonly IVendorConverter _vendorConverter;

        public CatalogConverter(
            IVendorRepository vendorRepository,
            IFactory<DTO.ICatalog> catalogFactory, 
            IVendorConverter vendorConverter)
        {
            _vendorRepository = vendorRepository;
            _catalogFactory = catalogFactory;
            _vendorConverter = vendorConverter;
        }

        public DTO.ICatalog ConvertToDTO(Catalog catalog)
        {
            var catalogDTO = _catalogFactory.Create();

            catalogDTO.Id = catalog.Id;
            catalogDTO.Name = catalog.Name;
            catalogDTO.ExternalId = catalog.ExternalId;
            catalogDTO.Amount = catalog.Amount;
            catalogDTO.ExtraFieldOne = catalog.ExtraFieldOne;
            catalogDTO.ExtraFieldTwo = catalog.ExtraFieldTwo;
            catalogDTO.Vendor = _vendorConverter
                .ConvertToDTO(_vendorRepository.GetById(catalog.VendorId));

            return catalogDTO;
        }
    }
}
