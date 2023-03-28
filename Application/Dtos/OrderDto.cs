using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public List<OrderProductDto>? OrderProducts { get; set; }
    }
}
