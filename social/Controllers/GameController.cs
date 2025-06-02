using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace social.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }  
        public IActionResult EnvGame()
        {
            return View();
        }
    }
}
