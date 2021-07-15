using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Market.Core;
using Market.Infrastructure.Converters;
using Market.Infrastructure.Services;
using Market.Core.Services;
using Market.Infrastructure.Repositories;

namespace Market.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMarket(this IServiceCollection services)
        {
            services.AddMarketCore();
            RegisterMarket(services);
        }

        private static void RegisterMarket(IServiceCollection services)
        {
            // TODO: services.AddScoped<MarketContext>();

            /// Repositories
            services.AddTransient<ICatalogRepository, CatalogRepository>();
            services.AddTransient<IVendorRepository, VendorRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            /// Converters
            services.AddTransient<ICatalogConverter, CatalogConverter>();
            services.AddTransient<IOrderConverter, OrderConverter>();
            services.AddTransient<IVendorConverter, VendorConverter>();
            services.AddTransient<IVendorOrderRequestConverter, VendorOrderRequestConverter>();
            
            /// Services
            services.AddScoped<IMarketService, MarketService>();
        }
    }
}
