using SabrasSmoothie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabrasSmoothie.Controllers
{
    public class LoginController : Controller
    {
        private CustomerDbContext _Customer = new CustomerDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Index(string userName, string password)
        {
            var user = _Customer.Customers.SingleOrDefault(customer => customer.UserName == userName && customer.Password == password);
            if (user == null)
            {
                ViewBag.Message = "Invalid username or password";
                return View();
            }
            else if (user.IsAdmin)
            {
                Session["Admin"] = true;
            }
            else
            {
                Session["User"] = true;
                Session["UserFullName"] = user.FirstName + " " + user.LastName;
            }
            
            return Redirect("/Home/"); ;
        }
    }
}