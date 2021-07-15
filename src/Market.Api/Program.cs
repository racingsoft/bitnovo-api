using Entities = Market.Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Market.Api.Vendor;
using Market.Core.Services;
using Market.Core.Adapters;

namespace Market.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureServices(services =>
                {
                    #region [DO NOT TOUCH THIS]
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<Entities.MarketContext>));

                    services.Remove(descriptor);

                    var _connection = new SqliteConnection("DataSource=:memory:");
                    _connection.Open();

                    services.AddDbContext<Entities.MarketContext>(options =>
                    {
                        options.UseSqlite(_connection);
                    });

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var env = scopedServices.GetService<IWebHostEnvironment>();
                    var db = scopedServices.GetRequiredService<Entities.MarketContext>();

                    db.Database.EnsureCreated();
                    #endregion

                    try
                    {
                        db.Vendors.Add(new Entities.Vendor { Id = 1, Name = "vendorOne" });
                        db.Vendors.Add(new Entities.Vendor { Id = 2, Name = "vendorTwo" });
                        db.SaveChanges();

                        db.Catalogs.Add(new Entities.Catalog { Id = 1, Name = "Product 1", ExtraFieldOne = "Vendor", Amount = 100, ExternalId = "1", VendorId = 1, ExtraFieldTwo = "" });
                        db.Catalogs.Add(new Entities.Catalog { Id = 2, Name = "Product 3", ExtraFieldOne = "Sale", Amount = 100, ExternalId = "1", VendorId = 1, ExtraFieldTwo = "" });
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        //throw;
                    }
                });
    }
}