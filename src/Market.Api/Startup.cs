using Market.Api.Vendor;
using Market.Core.Adapters;
using Market.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Market.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /// VendorAdapters
            services.AddTransient<VendorOne>();
            services.AddTransient<VendorOneCatalogConverter>();
            services.AddTransient<VendorOneRequestConverter>();
            services.AddTransient<VendorOneResponseConverter>();

            services.AddTransient<VendorTwo>();
            services.AddTransient<VendorTwoCatalogConverter>();
            services.AddTransient<VendorTwoRequestConverter>();
            services.AddTransient<VendorTwoResponseConverter>();

            /// Configuration
            services.AddSingleton<IVendorConfiguration>((serviceProvider) =>
            {
                var vendorConfiguration = new VendorConfiguration();

                var vendorOne = serviceProvider.GetRequiredService<VendorOne>();
                vendorConfiguration.Add(vendorOne);

                var vendorTwo = serviceProvider.GetRequiredService<VendorTwo>();
                vendorConfiguration.Add(vendorTwo);

                return vendorConfiguration;
            });

            /// Market.Infrastructure
            services.AddMarket();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Market.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Market.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}