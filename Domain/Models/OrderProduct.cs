using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int? ProductID { get; set; }
        public int? MembershipID { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public Membership Membership { get; set; }
        public Order Order { get; set; }
    }
}
