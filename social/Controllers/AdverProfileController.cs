using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace social.Controllers
{
    public class AdverProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        } 
        public IActionResult ArchiveAdd()
        {
            return View();
        } 
        public IActionResult CategoryItem()
        {
            return View();
        }    
        public IActionResult Details()
        {
            return View();
        }     
        public IActionResult favorite()
        {
            return View();
        }      
        public IActionResult MyAdds()
        {
            return View();
        }  
        public IActionResult MyProfile()
        {
            return View();
        }   
        public IActionResult PendingAds()
        {
            return View();
        }
        public IActionResult Pending()
        {
            return View();
        }
        public IActionResult Published()
        {
            return View();
        }
    }
}
