using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using Xunit;

namespace ProvaPub.Tests
{
    public class CustomerServiceTests : IDisposable
    {

        private readonly TestDbContext _dbContext;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _dbContext = new TestDbContext(options);
            _customerService = new CustomerService(_dbContext);

            InitializeData();
        }

        private void InitializeData()
        {
            // Clean up
            _dbContext.Customers.RemoveRange(_dbContext.Customers);
            _dbContext.Orders.RemoveRange(_dbContext.Orders);
            _dbContext.SaveChanges();

            // Add test data
            _dbContext.Customers.AddRange(
                new Customer { Id = 1, Name = "John Doe" },
                new Customer { Id = 2, Name = "Jane Smith" },
                new Customer { Id = 3, Name = "New Customer" } 
            );

            _dbContext.Orders.AddRange(
                new Order { Id = 1, CustomerId = 1, OrderDate = DateTime.UtcNow.AddDays(-5), Value = 50.00m },
                new Order { Id = 2, CustomerId = 1, OrderDate = DateTime.UtcNow.AddDays(-40), Value = 75.00m },
                new Order { Id = 3, CustomerId = 2, OrderDate = DateTime.UtcNow.AddDays(-10), Value = 120.00m }
            );

            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CanPurchase_WithInvalidCustomerId_ThrowsArgumentOutOfRangeException(int invalidCustomerId)
        {
            // Arrange
            decimal purchaseValue = 50.00m;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _customerService.CanPurchase(invalidCustomerId, purchaseValue));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10.50)]
        public async Task CanPurchase_WithInvalidPurchaseValue_ThrowsArgumentOutOfRangeException(decimal invalidPurchaseValue)
        {
            // Arrange
            int customerId = 1;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _customerService.CanPurchase(customerId, invalidPurchaseValue));
        }

        [Fact]
        public async Task CanPurchase_WithNonExistentCustomer_ThrowsInvalidOperationException()
        {
            // Arrange
            int nonExistentCustomerId = 999;
            decimal purchaseValue = 50.00m;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _customerService.CanPurchase(nonExistentCustomerId, purchaseValue));
        }

        [Fact]
        public async Task CanPurchase_WithValidCustomerAndValue_ReturnsTrue()
        {
            // Arrange
            int customerId = 3; // Customer sem compras recentes
            decimal purchaseValue = 50.00m;

            // Act
            var result = await _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CanPurchase_WithPurchaseInLastMonth_ReturnsFalse()
        {
            // Arrange
            int customerId = 1; // Customer com compra nos últimos 30 dias
            decimal purchaseValue = 50.00m;

            // Act
            var result = await _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CanPurchase_WithFirstPurchaseOver100_ReturnsFalse()
        {
            // Arrange
            int customerId = 3; // Customer sem compras anteriores
            decimal purchaseValue = 150.00m; // Valor acima do limite para primeira compra

            // Act
            var result = await _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CanPurchase_WithFirstPurchaseUnder100_ReturnsTrue()
        {
            // Arrange
            int customerId = 3; // Customer sem compras anteriores
            decimal purchaseValue = 99.99m; // Valor dentro do limite para primeira compra

            // Act
            var result = await _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public void ValidatePurchaseOnlyWorkingDays_DuringBusinessHours_ReturnsTrue()
        {
            // Arrange - Simular horário comercial em dia útil
            var mockDateTime = new DateTime(2024, 1, 15, 12, 0, 0); // Segunda-feira, 12:00

            // Act
            var result = _customerService.ValidatePurchaseOnlyWorkingDays(mockDateTime);

            Assert.True(result);
        }

        [Fact]
        public void ValidatePurchaseOnlyWorkingDays_OverBusinessHours_ReturnsFalse()
        {
            // Arrange - Simular horário comercial em dia não útil
            var mockDateTime = new DateTime(2024, 1, 14, 12, 0, 0); // Domingo, 12:00

            // Act
            var result = _customerService.ValidatePurchaseOnlyWorkingDays(mockDateTime);

            Assert.False(result);
        }



    }
}