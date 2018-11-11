using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SabrasSmoothie.Models;

namespace SabrasSmoothie.Controllers
{
    public class AboutController : Controller
    {
        private CustomerDbContext db = new CustomerDbContext();

        public ActionResult Index()
        {
            ViewBag.CustomersToGraph = new[] { 2, 3, 4 };//db.GetCustomersWithOrders().ToArray();
            
            ViewBag.Message = "Our application description page.";
            return View();
        }
    }
}