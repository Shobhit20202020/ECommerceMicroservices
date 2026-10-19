using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ProductDbContext _context;
        public ProductController(ProductDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products
                .AsNoTracking()
                .ToListAsync();

            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        // for post the data
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            // Let SQL Server generate the ID.
            product.Id = 0;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = product.Id },
                product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id,Product
            updatedProduct)
        {
            if(id!=updatedProduct.Id)
            {
                return BadRequest("Route ID and product ID must match");
            }
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existingProduct is null)
            {
                return NotFound();
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.StockQuantity = updatedProduct.StockQuantity;

            await _context.SaveChangesAsync();
            return NoContent();

        }

        // DELETE: api/Product/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }


            //private static readonly List<Product> Products = new()
            //{
            //    new Product
            //    {
            //        Id=1,
            //        Name="",
            //        Description="WireLess Mouse with USB receiver",
            //        Price=799.00m,
            //        StockQuantity=50
            //    },
            //     new Product
            //{
            //    Id = 2,
            //    Name = "",
            //    Description = "Mechanical keyboard with RGB lighting",
            //    Price = 2499.00m,
            //    StockQuantity = 25
            //},
            //new Product
            //{
            //    Id = 3,
            //    Name = "USB-C Hub",
            //    Description = "USB-C hub with multiple ports",
            //    Price = 1299.00m,
            //    StockQuantity = 30
            //}

            //};

            //[HttpGet]
            //public ActionResult<IEnumerable<Product>> GetProducts()
            //{
            //    return Ok(Products);
            //}

            //[HttpGet("{id:int}")]
            //public ActionResult<Product> GetProductById(int id)
            //{
            //    var product = Products.FirstOrDefault(p => p.Id == id);
            //    if(product is null)
            //    {
            //        return NotFound();
            //    }
            //    return Ok(product);
            //}
        }


    }
