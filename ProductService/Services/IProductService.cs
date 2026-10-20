using ProductService.DTOs;
using ProductService.Models;

namespace ProductService.Services
{
    public interface IProductService
    {

        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(int id);

        Task<Product> CreateAsync(CreateProductDtos dto);

        Task<bool> UpdateAsync(int id, CreateProductDtos dto);

        Task<bool> DeleteAsync(int id);
    }
}
