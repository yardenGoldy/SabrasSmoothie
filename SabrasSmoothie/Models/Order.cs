using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SabrasSmoothie.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public DateTime CreationDate { get; set; }

        public Customer Customer { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }

    public class OrderDbContext : SabrasDbContext
    {
    }
}