using Mono.Linq.Expressions;
using Newtonsoft.Json.Linq;
using SabrasSmoothie.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SabrasSmoothie.Controllers
{
    public class HomeController : Controller
    {
        private ProductDbContext Product = new ProductDbContext();
        private CustomerDbContext _Customer = new CustomerDbContext();

        public ActionResult Index()
        {
            // Debugging Mode! Delete In Production!
            var user = _Customer.Customers.SingleOrDefault(customer => customer.UserName == "Admin" && customer.Password == "Admin");
            Session["Admin"] = true;
            var searchQuery = TempData["search_query"] != null ? ((Expression<Func<Product, bool>>)TempData["search_query"]) : PredicateBuilder.True<Product>();
            var paramQuery = TempData["param_query"] != null ? ((Expression<Func<Product, bool>>)TempData["param_query"]) : PredicateBuilder.True<Product>();
            var query = searchQuery.AndAlso(paramQuery);
            var products = Product.SortByOrders(query);

            var productsByCalories = Product.GroupByCalories(query);
            var productsByPrice = Product.GroupByPrices(query);
            //var productsByVegan = Product.GroupByVegan(_query);

            ViewBag.Products = products;
            ViewBag.productsByCalories = productsByCalories;
            ViewBag.productsByPrice = productsByPrice;
            //ViewBag.productsByVegan = productsByVegan;
            return View();
        }

        [HttpPost]
        public ActionResult Search(string message)
        {
            var query = TempData["param_query"] != null ? ((Expression<Func<Product, bool>>)TempData["param_query"]) : PredicateBuilder.True<Product>();
            TempData["search_query"] = Product.FindByAll(query, message);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult SendParams(int minPrice = int.MinValue, int maxPrice = int.MaxValue, int minCal = int.MinValue, int maxCal = int.MaxValue, string isVegan = null)
        {
            var query = TempData["search_query"] != null ? ((Expression<Func<Product, bool>>)TempData["search_query"]) : PredicateBuilder.True<Product>();
            TempData["param_query"] = Product.RangeCalories(Product.RangePrice(query, minPrice, maxPrice), minCal, maxCal);
            return RedirectToAction("Index");
        }

        public static void DupPrices(Product[] products) {
            
            JObject Answer = JObject.Parse(GetCurrency("http://data.fixer.io/api/latest?access_key=ae8df3ea35ce6a2a4c670b1b6407a86d&symbols=USD,ILS&format=1"));

            int currToCalc = (int)float.Parse(Answer["rates"]["USD"].ToString());
            //if (Session["Currency"] == null)
            //{
            //    currToCalc = (int)float.Parse(Answer["rates"]["ILS"].ToString());
            //}

            foreach (Product product in products)
            {
                product.Price *= currToCalc;
            }
        }

        public static string GetCurrency(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}