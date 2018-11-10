using Newtonsoft.Json.Linq;
using SabrasSmoothie.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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