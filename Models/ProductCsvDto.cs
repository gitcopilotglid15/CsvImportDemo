namespace CsvImportDemo.Models
{
    public class ProductCsvDto
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}