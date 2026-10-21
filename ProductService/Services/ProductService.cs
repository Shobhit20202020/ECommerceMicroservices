
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DTOs;
using ProductService.Models;
using Microsoft.Extensions.Logging;
using ProductService.Common;

namespace ProductService.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductDbContext context,ILogger<ProductService> logger)
        {

            _context = context;
            _logger = logger;
        }

        public async Task<PagedResult<Product>> GetAllAsync(
            string? search=null,
             int pageNumber = 1,
             int pageSize = 10)
        {
            _logger.LogInformation(
                "Fetching all products from the database.");
            try
            {
                IQueryable<Product> query = _context.Products
                    .AsNoTracking();

                // 1 Apply search filtration.

                if(!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                    query = query.Where(p => p.Name.Contains(search));
                }
                // count all the matching products before pagination
                var totalCount = await query.CountAsync();

                // calculate total pages

                var totalPages = (int)Math.Ceiling(
            (double)totalCount / pageSize);

                // 4. Retrieve only the requested page
                var products = await query
                    .OrderBy(p => p.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


                // 5. Return products with pagination metadata
                return new PagedResult<Product>
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    Items = products
                };

            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex, "An error occured while fetching products.");
                throw;
            }
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> CreateAsync(CreateProductDtos dto)
        {
            _logger.LogInformation("Creating a new product.");
            try
            {
                var product = new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity
                };


                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation(
           "Product created successfully with ID {ProductId}.",
           product.Id);

                return product;
               
            }

            catch (Exception ex)
            {
                _logger.LogError(
                    ex, "An error occured while Creating products.");
                throw;
            }



            
        }

        public async Task<bool> UpdateAsync(int id,CreateProductDtos dto)
        {
            _logger.LogInformation("Updating the product with new data entry.");
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return false;
                }
                product.Name = dto.Name;
                product.Description = dto.Description;
                product.Price = dto.Price;
                product.StockQuantity = dto.StockQuantity;

                await _context.SaveChangesAsync();
                _logger.LogInformation(
          "Product Updated successfully with ID {ProductId}.",
          product.Id);

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(
                    ex, "An error occured while updating the products.");
                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {


                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation(
         "Product Deleted successfully with ID {ProductId}.",
         product.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex, "An error occured while Deleting the products.");
                throw;
            }
        }
    }
}
