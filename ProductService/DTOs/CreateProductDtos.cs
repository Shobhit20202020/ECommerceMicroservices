using System.ComponentModel.DataAnnotations;

namespace ProductService.DTOs
{
    public class CreateProductDtos
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;


        [Range(typeof(decimal),"0.01","999999")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }



    }
}
