using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class CustomerMembershipDto
    {
        public int Id { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}
