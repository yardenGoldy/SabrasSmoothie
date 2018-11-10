using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace SabrasSmoothie.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
        public bool IsAdmin { get; set; }


        public ICollection<Order> Orders { get; set; }
    }

    public class CustomerDbContext : SabrasDbContext 
    {
        public IQueryable<Customer> GetCustomersWithOrders()
        {
            return this.Customers.Include(customer => customer.Orders);
        }
    }
}