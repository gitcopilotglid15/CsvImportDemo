using CsvImportDemo.Models;
using Microsoft.AspNetCore.Http;

namespace CsvImportDemo.Services
{
    public interface IProductService
    {
        Task<(bool Success, List<string> Errors)> ImportProductsAsync(IFormFile file);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}