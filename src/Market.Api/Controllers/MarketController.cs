using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Market.Core.Messages;
using Market.Core.Factories;
using Market.Core.Services;

namespace Market.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private IMarketService _marketService;
        private IFactory<IGetProductsResponse> _getProductsResponsefactory;
        private IFactory<ICreateOrderResponse> _createOrderResponseFactory;

        public MarketController(
            IMarketService marketService,
            IFactory<IGetProductsResponse> getProductsResponseFactory,
            IFactory<ICreateOrderResponse> createOrderResponseFactory)
        {
            _marketService = marketService;
            _getProductsResponsefactory = getProductsResponseFactory;
            _createOrderResponseFactory = createOrderResponseFactory;
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> GetProducts()
        {
            var response = await GetProductsResponseAsync();
            return response.Products != null ? Ok(response) : NotFound();
        }

        [HttpPost]
        [Route("sync-catalog")]
        public async Task<IActionResult> SyncCatalog()
        {
            await SyncCatalogAsync();
            return Ok();
        }

        [HttpPost]
        [Route("create-order")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest createOrderRequest)
        {
            try
            {
                var response = await CreateOrderResponseAsync(createOrderRequest.Id);
                return Ok(response);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }

        private async Task<IGetProductsResponse> GetProductsResponseAsync()
        {
            var response = _getProductsResponsefactory.Create();
            response.Products = await _marketService.GetProductsAsync();
            return response;
        }

        private async Task SyncCatalogAsync()
        {
            await _marketService.SyncCatalogAsync();
        }
        
        private async Task<ICreateOrderResponse> CreateOrderResponseAsync(int id)
        {
            var order = await _marketService.CreateOrderAsync(id);

            ICreateOrderResponse createOrderResponse = _createOrderResponseFactory.Create();
            createOrderResponse.Id = order.Id;
            createOrderResponse.Amount = order.Amount;

            return createOrderResponse;
        }
    }
}