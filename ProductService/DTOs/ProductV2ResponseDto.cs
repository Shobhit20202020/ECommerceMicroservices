namespace ProductService.DTOs
{
    public class ProductV2ResponseDto
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int AvailableStock { get; set; }
    }
}
