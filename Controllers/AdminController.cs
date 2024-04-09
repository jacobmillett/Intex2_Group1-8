using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuroraBricks.Models;

namespace AuroraBricks.Controllers;

public class AdminController : Controller
{
        private readonly ILogger<AdminController> _logger;
        
        
        public AdminController(ILogger<AdminController> logger)
        {
                _logger = logger;
        }
        
        
        [HttpGet]
        public IActionResult ProductCRUD()
        {
                return View();
        }
        
        [HttpPost]
        public IActionResult ProductCRUD()
        {
                return View();
        }
        
        [HttpGet]
        public IActionResult UserCRUD()
        {
                return View();
        }
        
        [HttpPost]
        public IActionResult UserCRUD()
        {
                return View();
        }

        public IActionResult OrderReview()
        {
                return View();
        }
                
                
                
}

