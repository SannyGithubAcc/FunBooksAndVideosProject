using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Dtos
{
    public class CustomerMembershipDto
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}
