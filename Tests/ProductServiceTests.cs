using CsvImportDemo.Models;
using CsvImportDemo.Repositories;
using CsvImportDemo.Services;
using CsvImportDemo.Validators;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;
using Xunit;

namespace CsvImportDemo.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task ImportProductsAsync_ValidCsv_ImportsProducts()
        {
            // Arrange
            var csv = "Name,Price,Quantity\nTest,10,2";
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(csv)), 0, csv.Length, "file", "test.csv");
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.AddRangeAsync(It.IsAny<IEnumerable<Product>>())).Returns(Task.CompletedTask);
            var validator = new ProductValidator();
            var service = new ProductService(repoMock.Object, validator);

            // Act
            var (success, errors) = await service.ImportProductsAsync(file);

            // Assert and Test and Test 
            Assert.True(success);
            Assert.Empty(errors);
        }

        [Fact]
        public async Task ImportProductsAsync_InvalidCsv_ReturnsErrors()
        {
            // Arrange
            var csv = "Name,Price,Quantity\n,10,2";
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(csv)), 0, csv.Length, "file", "test.csv");
            var repoMock = new Mock<IProductRepository>();
            var validator = new ProductValidator();
            var service = new ProductService(repoMock.Object, validator);

            // Act
            var (success, errors) = await service.ImportProductsAsync(file);

            // Assert
            Assert.False(success);
            Assert.NotEmpty(errors);
        }
    }
}