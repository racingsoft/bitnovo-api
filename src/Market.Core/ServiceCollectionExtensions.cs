using Market.Core.DTO;
using Market.Core.Factories;
using Market.Core.Messages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMarketCore(this IServiceCollection services)
        {
            RegisterMarketCore(services);
        }

        public static void AddFactory<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        {
            services.AddTransient<TService, TImplementation>();
            services.AddSingleton<Func<TService>>(x => () => x.GetService<TService>());
            services.AddSingleton<IFactory<TService>, Factory<TService>>();
        }

        private static void RegisterMarketCore(IServiceCollection services)
        {
            /// DTO
            services.AddFactory<IVendor, Vendor>();
            services.AddFactory<ICatalog, Catalog>();
            services.AddFactory<IOrder, Order>();

            /// Messages
            services.AddFactory<IGetProductsResponse, GetProductsResponse>();

            services.AddFactory<ICreateOrderRequest, CreateOrderRequest>();
            services.AddFactory<ICreateOrderResponse, CreateOrderResponse>();

            services.AddFactory<IVendorOrderRequest, VendorOrderRequest>();
            services.AddFactory<IVendorOrderResponse, VendorOrderResponse>();
        }
    }
}
