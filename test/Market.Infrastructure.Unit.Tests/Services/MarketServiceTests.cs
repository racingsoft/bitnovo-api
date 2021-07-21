using Microsoft.VisualStudio.TestTools.UnitTesting;
using Market.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Market.Core.Services;
using Moq;
using Market.Core.Adapters;
using Market.Infrastructure.Repositories;
using Market.Infrastructure.Converters;
using Market.Core.Factories;
using DTO = Market.Core.DTO;
using Market.Infrastructure.Entities;

namespace Market.Infrastructure.Services.Tests
{
    [TestClass()]
    public class MarketServiceTests
    {
        [TestMethod()]
        public void MarketServiceTest()
        {
            /// Arrange
            
            var vendorConfigurationMock = new Mock<IVendorConfiguration>();
            var catalogRepositoryMock = new Mock<ICatalogRepository>();
            var catalogConverterMock = new Mock<ICatalogConverter>();
            var vendorRepositoryMock = new Mock<IVendorRepository>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var orderConverterMock = new Mock<IOrderConverter>();
            var vendorOrderRequestConverterMock = new Mock<IVendorOrderRequestConverter>();
            var orderDTOFactoryMock = new Mock<IFactory<DTO.IOrder>>();

            /// Act
            
            var market = new MarketService(
                vendorConfigurationMock.Object,
                catalogRepositoryMock.Object,
                catalogConverterMock.Object,
                vendorRepositoryMock.Object,
                orderRepositoryMock.Object,
                orderConverterMock.Object,
                vendorOrderRequestConverterMock.Object,
                orderDTOFactoryMock.Object
                );

            /// Assert
            
            market.Should().BeAssignableTo<IMarketService>();
        }

        [TestMethod()]
        public async Task GetProductsAsyncShoudReturnCatalogDTOList()
        {
            /// Arrange

            var vendorConfigurationMock = new Mock<IVendorConfiguration>();
            var catalogRepositoryMock = new Mock<ICatalogRepository>();
            var catalogConverterMock = new Mock<ICatalogConverter>();
            var vendorRepositoryMock = new Mock<IVendorRepository>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var orderConverterMock = new Mock<IOrderConverter>();
            var vendorOrderRequestConverterMock = new Mock<IVendorOrderRequestConverter>();
            var orderDTOFactoryMock = new Mock<IFactory<DTO.IOrder>>();
            var catalogDTOMock = new Mock<DTO.ICatalog>();

            var catalog = new Catalog()
            {
                Id = 1,

            };

            var catalogList = new List<Catalog>() 
            { 
                catalog
            };

            catalogRepositoryMock
                .Setup(x => x.ListAsync())
                .Returns(AsyncEnumerable.Create<Catalog>((cancelationToken) =>
                {
                    var enumerator = catalogList.GetEnumerator();

                    return AsyncEnumerator.Create<Catalog>(
                        async () => enumerator.MoveNext(),
                        () => enumerator.Current,
                        async () => enumerator.Dispose());
                }));

            catalogConverterMock
                .Setup(x => x.ConvertToDTO(catalog))
                .Returns(catalogDTOMock.Object);

            var market = new MarketService(
                vendorConfigurationMock.Object,
                catalogRepositoryMock.Object,
                catalogConverterMock.Object,
                vendorRepositoryMock.Object,
                orderRepositoryMock.Object,
                orderConverterMock.Object,
                vendorOrderRequestConverterMock.Object,
                orderDTOFactoryMock.Object
                );

            /// Act

            var result = await market.GetProductsAsync();

            /// Assert

            result.Should().Contain(new[] { catalogDTOMock.Object });
        }
    }
}