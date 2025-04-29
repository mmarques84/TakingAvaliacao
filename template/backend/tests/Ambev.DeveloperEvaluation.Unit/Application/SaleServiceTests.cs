using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Messaging.RabbitMQ;
using Ambev.DeveloperEvaluation.Ports.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class SaleServiceTests
    {
        private readonly Mock<ISaleRepository> _saleRepoMock = new();
        private readonly Mock<IProductRepository> _productRepoMock = new();
        private readonly Mock<IBranchRepository> _branchRepoMock = new();
        private readonly Mock<ICustomerRepository> _customerRepoMock = new();
        private readonly Mock<VendaEventsPublisher> _eventPublisherMock = new();

        private readonly SaleService _saleService;

        public SaleServiceTests()
        {
            _saleService = new SaleService(
                _saleRepoMock.Object,
                _productRepoMock.Object,
                _branchRepoMock.Object,
                _customerRepoMock.Object,
                _eventPublisherMock.Object
            );
        }

        [Fact]
        public async Task Create_Sale()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            _customerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(new Customer { Id = customerId });
            _branchRepoMock.Setup(r => r.GetByIdAsync(branchId)).ReturnsAsync(new Branch { Id = branchId });
            _productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(new Item { Id = productId });

            var sale = new Sale
            {
                IdCustomer = customerId,
                IdBranch = branchId,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem { ProductId = productId, Quantity = 10, UnitPrice = 10 }
                }
            };

            // Act
            var result = await _saleService.CreateSaleAsync(sale);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.SaleItems);
            Assert.Equal(0.20m, result.SaleItems[0].Discount); // 20% discount
            Assert.Equal(80, result.SaleItems[0].TotalAmount); // 10 * 10 * 0.8
        }

        [Fact]
        public async Task Regra_quantidade_maior_20()
        {
            //Regra_quantidade_maior_20
            // Arrange
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            _customerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(new Customer { Id = customerId });
            _branchRepoMock.Setup(r => r.GetByIdAsync(branchId)).ReturnsAsync(new Branch { Id = branchId });
            _productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(new Item { Id = productId });

            var sale = new Sale
            {
                IdCustomer = customerId,
                IdBranch = branchId,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem { ProductId = productId, Quantity = 21, UnitPrice = 10 }
                }
            };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _saleService.CreateSaleAsync(sale));
        }

        [Fact]
        public async Task Desconto_menor_4()
        {
            //Desconto menor que 4
            // Arrange
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            _customerRepoMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(new Customer { Id = customerId });
            _branchRepoMock.Setup(r => r.GetByIdAsync(branchId)).ReturnsAsync(new Branch { Id = branchId });
            _productRepoMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(new Item { Id = productId });

            var sale = new Sale
            {
                IdCustomer = customerId,
                IdBranch = branchId,
                SaleItems = new List<SaleItem>
                {
                    new SaleItem { ProductId = productId, Quantity = 3, UnitPrice = 10 }
                }
            };

            // Act
            var result = await _saleService.CreateSaleAsync(sale);

            // Assert
            Assert.Equal(0m, result.SaleItems[0].Discount);
        }
    }
}
