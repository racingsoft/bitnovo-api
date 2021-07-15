using System.Collections.Generic;

namespace Market.Core.DTO
{
    public interface ICatalog
    {
        int Id { get; set; }
        string Name { get; set; }
        string ExternalId { get; set; }
        decimal Amount { get; set; }
        string ExtraFieldOne { get; set; }
        string ExtraFieldTwo { get; set; }
        IVendor Vendor { get; set; }
    }
}