using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuroraBricks.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using AuroraBricks.ViewModels;

namespace AuroraBricks.Controllers;

public class HomeController : Controller
{

    private IBrixRepository _repo;

    public HomeController(IBrixRepository temp)
    {
        _repo = temp;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult CustomerSignUp()
    {
        var lastRecord = _repo.GetLastCustomer();

        // Calculate the new CustomerId value
        int newCustomerId = (lastRecord != null) ? lastRecord.CustomerId + 1 : 1;

        // Create a new BrixCustomer object with the calculated CustomerId value
        var model = new BrixCustomer { CustomerId = newCustomerId };
        return View(model);
    }

    [HttpPost]
    public IActionResult CustomerSignUp(BrixCustomer response)
    {
        _repo.AddCustomer(response);

        return View("~/Areas/Identity/Pages/Account/Register.cshtml");
      
    }
    
    public IActionResult ProductList(int pageNum, string category, string primaryColor)
    {
        int pageSize = 5;
        var query = _repo.Products
            .OrderBy(x => x.Name)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize);

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category == category);
        }

        if (!string.IsNullOrEmpty(primaryColor))
        {
            query = query.Where(p => p.PrimaryColor == primaryColor);
        }

        var products = query.OrderBy(p => p.ProductId).ToList();

        ViewBag.Category = category;
        ViewBag.PrimaryColor = primaryColor;
        ViewBag.Categories = _repo.Products.Select(p => p.Category).Distinct();
        ViewBag.PrimaryColors = _repo.Products.Select(p => p.PrimaryColor).Distinct();

        return View(products);
    }
    //
    // public IActionResult ProductDetail()
    // {
    //     return View();
    // }
    //
    //
    public IActionResult Cart(string returnUrl)
    {
        var cart = SessionCart.GetCart(HttpContext.RequestServices);
        var viewModel = new CartViewModel
        {
            Cart = cart ?? new Cart(), // Ensure Cart is not null
            ReturnUrl = returnUrl ?? "/"
        };

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId, string returnUrl)
    {
        var cart = SessionCart.GetCart(HttpContext.RequestServices);
        BrixProduct product = _repo.Products
                                .FirstOrDefault(p => p.ProductId == productId);

        if (product != null)
        {
            cart.RemoveLine(product);
        }

        return RedirectToAction("Cart", new { returnUrl = returnUrl });
    }

    public IActionResult AboutUs()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Checkout()
    {
        return RedirectToAction("Cart");
    }
}