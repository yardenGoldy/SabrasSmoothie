using SabrasSmoothie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SabrasSmoothie.Controllers
{
    public class CleanUpController : Controller
    {
        // GET: /CleanUp/ 

        public string Index()
        {
            SeedData.CleanUp();
            return "System entities cleaned up...";
        }

        // 
        // GET: /CleanUp/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }
    }
}