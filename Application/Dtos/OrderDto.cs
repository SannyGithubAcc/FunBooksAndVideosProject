using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Dtos
{
    public class OrderDto
    {
        [JsonIgnore]
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
