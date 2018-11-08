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
            ViewBag.Message = "Our application my-orders page.";
            return View();
        }
    }
}