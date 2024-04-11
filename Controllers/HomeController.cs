using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuroraBricks.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
namespace AuroraBricks.Controllers;

public class HomeController : Controller
{

    private IBrixRepository _repo;

    private readonly UserManager<IdentityUser> _userManager;
    public HomeController(IBrixRepository temp, UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
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
    
    
    public async Task<IActionResult> CustomerProfile()
    {
        var userEmail = _userManager.GetUserAsync(User).Result?.Email;
        var customer = await _repo.GetBrixCustomerByEmailAsync(userEmail);

        return View(customer);
    }
    
    
    
    
    
    
    

    //
    //
    // public IActionResult ProductList()
    // {
    //     return View();
    // }
    //
    // public IActionResult ProductDetail()
    // {
    //     return View();
    // }
    //
    //
    // public IActionResult Cart()
    // {
    //     return View();
    // }
    //
    //
    //
    //

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


}