using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerID { get; set; }
        public DateTime Date { get; set; }
        public decimal price { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }

    }
}
