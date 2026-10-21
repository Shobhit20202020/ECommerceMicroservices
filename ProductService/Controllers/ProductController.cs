using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using ProductService.DTOs;
using ProductService.Services;
using ProductService.Common;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        public ProductController(IProductService productService )
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(
            [FromQuery] string? search=null,
             [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10     )
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(
                    "pageNumber must be at least 1, and pageSize must be between 1 and 100.");
            }
            
            var products = await _productService.GetAllAsync(
                search,pageNumber,pageSize);
               

            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
              

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        // for post the data
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(CreateProductDtos dto)
        {
            // Let SQL Server generate the ID.


            var product=await _productService.CreateAsync(dto);
            

            return Ok(product);

            //return CreatedAtAction(
            //    nameof(GetProductById),
            //    new { id = product.Id },
            //    product);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, CreateProductDtos
            dto)
        {
            var updated = await _productService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }


    

   
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _productService.DeleteAsync(id);

            if (!product)
                return NotFound();


           
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
