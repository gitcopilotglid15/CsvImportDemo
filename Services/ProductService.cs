using CsvHelper;
using CsvHelper.Configuration;
using CsvImportDemo.Models;
using CsvImportDemo.Repositories;
using CsvImportDemo.Validators;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace CsvImportDemo.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IProductValidator _validator;

        public ProductService(IProductRepository repository, IProductValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<(bool Success, List<string> Errors)> ImportProductsAsync(IFormFile file)
        {
            var errors = new List<string>();
            var products = new List<Product>();

            if (file == null || file.Length == 0)
            {
                errors.Add("No file uploaded.");
                return (false, errors);
            }

            try
            {
                using var stream = file.OpenReadStream();
                using var reader = new StreamReader(stream);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,
                    MissingFieldFound = null
                });

                var records = csv.GetRecords<ProductCsvDto>().ToList();
                int row = 2; // 1-based, header is row 1

                foreach (var record in records)
                {
                    var (isValid, error) = _validator.Validate(record, row);
                    if (!isValid && error != null)
                        errors.Add(error);
                    else
                        products.Add(new Product
                        {
                            Name = record.Name.Trim(),
                            Price = record.Price,
                            Quantity = record.Quantity
                        });
                    row++;
                }

                if (errors.Count > 0)
                    return (false, errors);

                await _repository.AddRangeAsync(products);
                return (true, new List<string>());
            }
            catch (HeaderValidationException)
            {
                errors.Add("CSV header is invalid. Expected: Name,Price,Quantity");
                return (false, errors);
            }
            catch (Exception ex)
            {
                errors.Add($"Unexpected error: {ex.Message}");
                return (false, errors);
            }
        }
    }
}