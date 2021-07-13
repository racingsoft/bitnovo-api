using System;
using System.Collections.Generic;

namespace Market.Api.Vendor
{
    public class VendorTwoCatalog
    {
        public string Name { get; set; }
        public string Ean { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }

    public class VendorTwoResponse
    {
        public string TxtId { get; set; }
        public string Ean { get; set; }
        public decimal Amount { get; set; }
    }

    public class VendorTwoRequest
    {
        public string Ean { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class VendorTwo
    {
        public List<VendorTwoCatalog> GetCatalog()
        {
            #region [DO NOT TOUCH THIS]

            Random rnd = new Random();
            var catalog = new List<VendorTwoCatalog>();
            for (int i = 3; i <= 7; i++)
            {
                catalog.Add(new VendorTwoCatalog
                {
                    Amount = rnd.Next(50, 100),
                    Type = "Sale",
                    Name = $"Product {i}",
                    Ean = i.ToString(),
                    Vendor = $"Verdor{i}"
                });
            }

            #endregion [DO NOT TOUCH THIS]

            return catalog;
        }

        public VendorTwoResponse CreateOrder(VendorTwoRequest request)
        {
            #region [DO NOT TOUCH THIS]

            if (string.IsNullOrWhiteSpace(request.Ean))
            {
                throw new Exception("Txid is null");
            }
            if (string.IsNullOrWhiteSpace(request.Type))
            {
                throw new Exception("Type is null");
            }
            return new VendorTwoResponse { Ean = request.Ean, Amount = request.Amount, TxtId = Guid.NewGuid().ToString() };

            #endregion [DO NOT TOUCH THIS]
        }
    }
}