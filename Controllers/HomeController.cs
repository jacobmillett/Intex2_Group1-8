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

    public IActionResult ProductList(int pageNum, string category, string primaryColor)
    {
        int pageSize = 5;

        // Prepare the base query to be filtered
        var baseQuery = _context.BrixProducts.AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(category))
        {
            baseQuery = baseQuery.Where(p => p.Category == category);
        }
        if (!string.IsNullOrEmpty(primaryColor))
        {
            baseQuery = baseQuery.Where(p => p.PrimaryColor == primaryColor);
        }

        // First, count all items for pagination (before paging the items)
        var totalCount = baseQuery.Count();

        // Retrieve the current page of filtered products
        var products = baseQuery
            .OrderBy(x => x.Name)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        // Prepare categories and primary colors for dropdowns, ideally cached if called frequently
        ViewBag.Categories = _context.BrixProducts.Select(p => p.Category).Distinct().ToList();
        ViewBag.PrimaryColors = _context.BrixProducts.Select(p => p.PrimaryColor).Distinct().ToList();

        // Pass current filter values back to the view to repopulate the form
        ViewBag.Category = category;
        ViewBag.PrimaryColor = primaryColor;

        // Prepare pagination data
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        ViewBag.CurrentPage = pageNum;

        return View(products);
    }




    public IActionResult ProductDetail(int ProductId)
    {
        var product = _context.BrixProducts.FirstOrDefault(p => p.ProductId == ProductId);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

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