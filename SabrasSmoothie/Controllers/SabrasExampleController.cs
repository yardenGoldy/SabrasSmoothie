using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabrasSmoothie.Controllers
{
    public class SabrasExampleController : Controller
    {
        // GET: /SabrasExample/ 

        public string Index()
        {
            return "This is my <b>default</b> action...";
        }

        // 
        // GET: /SabrasExample/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }
    }
}