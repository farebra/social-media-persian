using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace social.Controllers
{
    public class AdverController : Controller
    {
        public IActionResult Advert()
        {
            return View();
        }
        public IActionResult Addpost()
        {
            return View();
        } 
        public IActionResult AddpostDetails()
        {
            return View();
        }
    }
}
