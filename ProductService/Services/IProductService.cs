using ProductService.DTOs;
using ProductService.Models;
using ProductService.Common;

namespace ProductService.Services
{
    public interface IProductService
    {

        // Task<List<Product>> GetAllAsync();
        Task<PagedResult<Product>> GetAllAsync(
            string? search = null,
            int pageNumber=1,
            int pageSize=10
            );

        Task<Product?> GetByIdAsync(int id);

        Task<Product> CreateAsync(CreateProductDtos dto);

        Task<bool> UpdateAsync(int id, CreateProductDtos dto);

        Task<bool> DeleteAsync(int id);
    }
}
