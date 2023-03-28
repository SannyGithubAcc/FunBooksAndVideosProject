namespace Domain.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int Order_ID { get; set; }
        public int Product_ID { get; set; }
        public int Membership_ID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public Membership Membership { get; set; }
        public Order Order { get; set; }
    }
}
