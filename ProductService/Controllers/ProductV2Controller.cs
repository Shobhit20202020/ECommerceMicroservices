using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using ProductService.DTOs;
using ProductService.Services;

namespace ProductService.Controllers
{
    [ApiController]
    [ApiVersion(2.0)]
    [Route("api/v{version:apiVersion}/product")]
    public class ProductV2Controller : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductV2Controller(
            IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductV2ResponseDto>>>
            GetProducts()
        {
            var products = await _productService.GetAllAsync();

            var response = products.Items.Select(p =>
                new ProductV2ResponseDto
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Price = p.Price,
                    AvailableStock = p.StockQuantity
                }).ToList();

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductV2ResponseDto>>
            GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var response = new ProductV2ResponseDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                AvailableStock = product.StockQuantity
            };

            return Ok(response);
        }
    }
}