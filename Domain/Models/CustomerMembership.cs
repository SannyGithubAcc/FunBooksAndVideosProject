namespace Domain.Models
{
    public class CustomerMembership
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
