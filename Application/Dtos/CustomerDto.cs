using System.ComponentModel.DataAnnotations;

namespace Application.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
    }

}
