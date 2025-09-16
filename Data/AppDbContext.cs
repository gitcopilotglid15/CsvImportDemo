using CsvImportDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace CsvImportDemo.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}