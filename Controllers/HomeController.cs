using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuroraBricks.Models;

namespace AuroraBricks.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CustomerSignUp()
    {
        return View();
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