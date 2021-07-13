using Market.Api.Data;
using Market.Api.Vendor;
using Market.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Market.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private MarketContext _marketContext;

        public MarketController(MarketContext marketContext)
        {
            _marketContext = marketContext;
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> GetProduct()
        {

            var result = await _marketContext.Catalogs.ToListAsync();
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        [Route("sync-catalog")]
        public async Task<IActionResult> SyncCatalog()
        {
            var verdorTwo = new VendorTwo();
            var verdorOne = new VendorOne();

            var vendorTwoCatalog = verdorTwo.GetCatalog();
            var vendorOneCatalog = verdorOne.GetCatalog();

            var products = await _marketContext.Catalogs.ToListAsync();
            #region [Update current products]
            foreach (var product in products)
            {
                var verdonOneProduct = vendorOneCatalog.FirstOrDefault(w => w.Name.ToLower() == product.Name.ToLower());
                var verdonTwoProduct = vendorTwoCatalog.FirstOrDefault(w => w.Name.ToLower() == product.Name.ToLower());
                if (verdonOneProduct is not null && verdonTwoProduct is not null)
                {
                    if (verdonOneProduct.Amount < verdonTwoProduct.Amount)
                    {
                        var vendor = _marketContext.Vendors.Find(1);
                        product.Amount = verdonOneProduct.Amount;
                        product.VendorId = vendor.Id;
                        product.ExtraFieldOne = verdonOneProduct.Vendor;
                        product.ExternalId = verdonOneProduct.Sku;
                        product.ExtraFieldTwo = verdonOneProduct.CurrencyCode;
                        await _marketContext.SaveChangesAsync();

                    }
                    else
                    {
                        var vendor = _marketContext.Vendors.Find(2);
                        product.Amount = verdonTwoProduct.Amount;
                        product.VendorId = vendor.Id;
                        product.ExtraFieldOne = verdonTwoProduct.Type;
                        product.ExternalId = verdonTwoProduct.Ean;
                        await _marketContext.SaveChangesAsync();
                    }
                    vendorOneCatalog.Remove(verdonOneProduct);
                    vendorTwoCatalog.Remove(verdonTwoProduct);                    
                }else if (verdonOneProduct is not null)
                {
                    var vendor = _marketContext.Vendors.Find(1);
                    product.Amount = verdonOneProduct.Amount;
                    product.VendorId = vendor.Id;
                    product.ExtraFieldOne = verdonOneProduct.Vendor;
                    product.ExternalId = verdonOneProduct.Sku;
                    product.ExtraFieldTwo = verdonOneProduct.CurrencyCode;
                    await _marketContext.SaveChangesAsync();
                    vendorOneCatalog.Remove(verdonOneProduct);
                }
                else if (verdonTwoProduct is not null)
                {
                    var vendor = _marketContext.Vendors.Find(2);
                    product.Amount = verdonTwoProduct.Amount;
                    product.VendorId = vendor.Id;
                    product.ExtraFieldOne = verdonTwoProduct.Type;
                    product.ExternalId = verdonTwoProduct.Ean;
                    await _marketContext.SaveChangesAsync();
                    vendorTwoCatalog.Remove(verdonTwoProduct);
                }
            }
            #endregion

            #region [New products]
            var sameProducts = vendorOneCatalog.Where(w => vendorTwoCatalog.Select(s => s.Name.ToUpper()).Contains(w.Name.ToUpper())).ToList();
            if (sameProducts is not null) 
            {
                foreach (var sameProduct in sameProducts)
                {
                    var verdonOneProduct = vendorOneCatalog.FirstOrDefault(w => w.Name.ToLower() == sameProduct.Name.ToLower());
                    var verdonTwoProduct = vendorTwoCatalog.FirstOrDefault(w => w.Name.ToLower() == sameProduct.Name.ToLower());
                    if (verdonOneProduct.Amount < verdonTwoProduct.Amount)
                    {
                        var vendor = _marketContext.Vendors.Find(1);
                        _marketContext.Catalogs.Add(new Catalog
                        {
                            Amount = verdonOneProduct.Amount,
                            ExternalId = verdonOneProduct.Sku,
                            ExtraFieldOne = verdonOneProduct.Vendor,
                            ExtraFieldTwo = verdonOneProduct.CurrencyCode,
                            Name = verdonOneProduct.Name,
                            VendorId = vendor.Id,
                        });
                        await _marketContext.SaveChangesAsync();

                    }
                    else
                    {
                        var vendor = _marketContext.Vendors.Find(2);
                        _marketContext.Catalogs.Add(new Catalog
                        {
                            Amount = verdonTwoProduct.Amount,
                            ExternalId = verdonTwoProduct.Ean,
                            ExtraFieldOne = verdonTwoProduct.Type,
                            Name = verdonTwoProduct.Name,
                            VendorId = vendor.Id,
                        });
                        await _marketContext.SaveChangesAsync();
                    }
                    vendorOneCatalog.Remove(verdonOneProduct);
                    vendorTwoCatalog.Remove(verdonTwoProduct);
                }
                if (vendorOneCatalog.Any()) 
                {
                    var vendor = _marketContext.Vendors.Find(1);
                    _marketContext.Catalogs.AddRange(
                        vendorOneCatalog.Select(s => new Catalog
                        {
                            Amount = s.Amount,
                            ExternalId = s.Sku,
                            ExtraFieldOne = s.Vendor,
                            ExtraFieldTwo = s.CurrencyCode,
                            Name = s.Name,
                            VendorId = vendor.Id,
                        }));
                    await _marketContext.SaveChangesAsync();
                }
                if (vendorTwoCatalog.Any())
                {
                    var vendor = _marketContext.Vendors.Find(2);
                    _marketContext.Catalogs.AddRange(
                        vendorTwoCatalog.Select(s => new Catalog
                        {
                            Amount = s.Amount,
                            ExternalId = s.Ean,
                            ExtraFieldOne = s.Type,
                            Name = s.Name,
                            VendorId = vendor.Id,
                        }));
                    await _marketContext.SaveChangesAsync();
                }
            }
            #endregion
                return Ok();
        }

        [HttpPost]
        [Route("create-order")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest createOrderViewModel)
        {
            CreateOrderResponse response;
            if (!createOrderViewModel.Id.HasValue) 
            {
                throw new Exception("Id is null");
            }
            var product = await _marketContext.Catalogs.FirstOrDefaultAsync(w => w.Id == createOrderViewModel.Id);
            if (product is null) 
            {
                return NotFound();
            }
            if (product.VendorId == 1)
            {                
                var verdorOne = new VendorOne();
                var result = verdorOne.CreateOrder(new VendorOneRequest { Amount = product.Amount, Sku = product.ExternalId, Vendor = product.ExtraFieldOne });
                if (result is null) 
                {
                    throw new Exception("Error to processing payment");
                }
                var order = new Order { CatalogId = createOrderViewModel.Id.Value, ExternalId = result.OrderIdentifier, Amount = product.Amount };
                _marketContext.Orders.Add(order);
                await _marketContext.SaveChangesAsync();
                response = new CreateOrderResponse { Id = order.Id, Amount = order.Amount };
            }
            else 
            {
                var verdorTwo = new VendorTwo();
                var result = verdorTwo.CreateOrder(new VendorTwoRequest { Amount = product.Amount, Ean = product.ExternalId,  Type = product.ExtraFieldOne });
                if (result is null)
                {
                    throw new Exception("Error to processing payment");
                }
                var order = new Order { CatalogId = createOrderViewModel.Id.Value, ExternalId = result.TxtId, Amount = product.Amount };
                _marketContext.Orders.Add(order);
                await _marketContext.SaveChangesAsync();
                response = new CreateOrderResponse { Id = order.Id, Amount = order.Amount };
            }

            return Ok(response);
        }
    }
}