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

            var productsByCalories = Product.GroupByCalories();
            var productsByPrice = Product.GroupByPrices();
            var productsByVegan = Product.GroupByVegan();

            ViewBag.Products = products;
            ViewBag.productsByCalories = productsByCalories;
            ViewBag.productsByPrice = productsByPrice;
            ViewBag.productsByVegan = productsByVegan;
            return View();
        }

        public ActionResult Search(string message)
        {
            var products = Product.FindByAll(message);

            ViewBag.Products = products;
            return View();
        }
    }
}