﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SabrasSmoothie.Models
{
    public class OrderProduct
    {
        [Key]
        [ForeignKey("Order")]
        [Column(Order = 1)]
        public int OrderId { get; set; }

        [Key]
        [ForeignKey("Product")]
        [Column(Order = 2)]
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
        public Order Order { get; set; }
    }

    public class OrderProductDbContext : SabrasDbContext
    {
    }
}