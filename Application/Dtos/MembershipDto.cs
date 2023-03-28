using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class MembershipDto
    {
        public int Id { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
