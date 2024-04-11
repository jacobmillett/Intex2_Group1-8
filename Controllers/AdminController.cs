using System.Diagnostics;
using AuroraBricks.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using AuroraBricks.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;


namespace AuroraBricks.Controllers;

[Authorize(Policy = "AdminPolicy")]
public class AdminController : Controller
{
        private IBrixRepository _repo;
        
           
        public AdminController(IBrixRepository temp)
        {
                _repo = temp;
        }
        
        [HttpGet]
        public IActionResult EditUser(int id)
        {
                var recordToEdit = _repo.Customers
                        .Single(x => x.CustomerId == id);

                return View("~/Views/Home/CustomerSignUp.cshtml", recordToEdit);
        }
        
        [HttpPost]
        public IActionResult EditUser(BrixCustomer customer)
        {
                _repo.EditUser(customer);
                return RedirectToAction("UserCrud");
        }
        
        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
                var recordToDelete = _repo.Customers
                        .Single(x => x.CustomerId == id);
                return View("DeleteUser", recordToDelete);
        }

        [HttpPost]
        public IActionResult DeleteUser(BrixCustomer customer)
        {
                _repo.RemoveUser(customer);
                return RedirectToAction("UserCrud");
        }




   
        [HttpGet]
        public IActionResult ProductCrud()
        {
                var products = _repo.Products
                        .OrderBy(x => x.Name).ToList();
                return View(products);
        }
        
        

        [HttpGet]
        public IActionResult OrderReview()
        {
                var orders = _repo.Orders
                        .OrderByDescending(x => x.Date)
                        .ThenByDescending(x=>x.Time)
                        .ToList();
                return View(orders);
        }
        


        [HttpGet]
        public IActionResult UserCrud()
        {
                var customers = _repo.Customers
                        .OrderBy(x => x.FirstName)
                        .ThenBy(x=>x.LastName)
                        .ToList();
                return View(customers);
        }



        [HttpGet]

        public IActionResult AddNewProduct()
        {
                var lastRecord = _repo.GetLastProduct();

                // Calculate the new CustomerId value
                int newProductId = (lastRecord != null) ? lastRecord.ProductId + 1 : 1;

                // Create a new BrixCustomer object with the calculated CustomerId value
                var model = new BrixProduct { ProductId = newProductId };
                return View(model);
        }



        [HttpPost]

        public IActionResult AddNewProduct(BrixProduct product)
        {
                _repo.AddProduct(product);
                return RedirectToAction("ProductCrud");
        }

        

        

        [HttpGet]
        public IActionResult Edit(int id)
        {
                var recordToEdit = _repo.Products
                        .Single(x => x.ProductId == id);

                return View("AddNewProduct", recordToEdit);
        }


        [HttpPost]
         public IActionResult Edit(BrixProduct product)
         {
                 _repo.EditProduct(product);
                 return RedirectToAction("ProductCrud");
         }



         [HttpGet]
         public IActionResult DeleteProduct(int id)
         {
                 var recordToDelete = _repo.Products
                         .Single(x => x.ProductId == id);
                 return View("DeleteProduct", recordToDelete);
         }

         [HttpPost]
         public IActionResult DeleteProduct(BrixProduct product)
         {
                 _repo.RemoveProduct(product);
                 return RedirectToAction("ProductCrud");
         }
                 
 }
//
