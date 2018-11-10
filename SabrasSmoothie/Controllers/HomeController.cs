using SabrasSmoothie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabrasSmoothie.Controllers
{
    public class HomeController : Controller
    {
        private ProductDbContext Product = new ProductDbContext();
        public ActionResult Index()
        {
            var products = Product.SortByOrders();

            ViewBag.Products = products;
            return View();
        }
    }
}