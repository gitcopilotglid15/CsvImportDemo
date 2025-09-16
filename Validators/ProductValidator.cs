using CsvImportDemo.Models;

namespace CsvImportDemo.Validators
{
    public class ProductValidator : IProductValidator
    {
        public (bool IsValid, string? Error) Validate(ProductCsvDto dto, int rowNumber)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return (false, $"Row {rowNumber}: Name is required.");
            if (dto.Price < 0)
                return (false, $"Row {rowNumber}: Price cannot be negative.");
            if (dto.Quantity < 0)
                return (false, $"Row {rowNumber}: Quantity cannot be negative.");
            return (true, null);
        }
    }
}