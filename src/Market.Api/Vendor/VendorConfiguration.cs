using Market.Core.Adapters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Api.Vendor
{
    public class VendorConfiguration : IVendorConfiguration
    {
        private readonly object addLock = new object();
        private readonly IList<IVendorAdapter> _vendors;

        public IEnumerable<IVendorAdapter> Vendors 
        { 
            get => new ConcurrentBag<IVendorAdapter>(_vendors); 
        }

        public VendorConfiguration()
        {
            _vendors = new List<IVendorAdapter>();
        }

        public void Add(IVendorAdapter adapter)
        {
            lock(addLock)
            {
                _vendors.Add(adapter);
            }
        }
    }
}
