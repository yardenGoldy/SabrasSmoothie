using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabrasSmoothie.Controllers
{
    public class MyOrdersController : Controller
    {
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return Redirect("/Login/Index");
            }
            else if (Session["User"] != null)
            {
                
            }

            ViewBag.Message = "Your orders";
            return View();
        }
    }
}