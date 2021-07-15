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
    public class VendorConverter : IVendorConverter
    {
        private readonly IFactory<DTO.IVendor> _vendorFactory;

        public VendorConverter(IFactory<DTO.IVendor> vendorFactory)
        {
            _vendorFactory = vendorFactory;
        }

        public DTO.IVendor ConvertToDTO(Vendor vendor)
        {
            var vendorDTO = _vendorFactory.Create();

            vendorDTO.Id = vendor.Id;
            vendorDTO.Name = vendor.Name;

            return vendorDTO;
        }
    }
}
