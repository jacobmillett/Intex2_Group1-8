using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuroraBricks.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuroraBricks.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly AbrixContext _context;

    public HomeController(ILogger<HomeController> logger, AbrixContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CustomerSignUp()
    {
        return View();
    }

    //public IActionResult ProductList(int pageNum)
    //{
    //    int pageSize= 5;
    //    var query = _context.BrixProducts.AsQueryable()
    //        .OrderBy(x => x.Name)
    //        .Skip((pageNum - 1) * pageSize)
    //        .Take(pageSize);
    //}

    public IActionResult ProductList(int pageNum, string category, string primaryColor)
    {
        int pageSize = 5;
        var query = _context.BrixProducts.AsQueryable()
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
        ViewBag.Categories = _context.BrixProducts.Select(p => p.Category).Distinct();
        ViewBag.PrimaryColors = _context.BrixProducts.Select(p => p.PrimaryColor).Distinct();

        return View(products);
    }


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