using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class OrderProductDto
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? MembershipId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(100)]
        public string MembershipName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public ProductDto? Product { get; set; }
        public MembershipDto? Membership { get; set; }
    }
}
