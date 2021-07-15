using Market.Core.Adapters;
using System.Collections.Generic;

namespace Market.Core.Adapters
{
    public interface IVendorConfiguration
    {
        void Add(IVendorAdapter adapter);
        IEnumerable<IVendorAdapter> Vendors { get; }
    }
}