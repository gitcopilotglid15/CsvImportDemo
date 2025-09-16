using CsvImportDemo.Models;

namespace CsvImportDemo.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddRangeAsync(IEnumerable<Product> products);
    }
}