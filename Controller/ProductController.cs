using CsvImportDemo.Models;
using CsvImportDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace CsvImportDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] Request request)
        {
            var file = request.file;
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded." });

            var (success, errors) = await _service.ImportProductsAsync(file);
            if (!success)
                return BadRequest(new { errors });
            return Ok(new { message = "Products imported successfully." });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products);
        }
    }
}