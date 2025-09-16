using CsvImportDemo.Models;

namespace CsvImportDemo.Validators
{
    public interface IProductValidator
    {
        (bool IsValid, string? Error) Validate(ProductCsvDto dto, int rowNumber);
    }
}