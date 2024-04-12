using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuroraBricks.Models;
using Microsoft.EntityFrameworkCore;
using System;
using AuroraBricks.Models.ViewModels;

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

    public IActionResult ProductList(int pageNum, int pageSize = 5)
    {
        // Prepare the base query to be filtered
        var baseQuery = _context.BrixProducts.AsQueryable();

        var viewModel = new ProductsListViewModel
        {
            Product = baseQuery
                .OrderBy(x => x.Name)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList(),

            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = _context.BrixProducts.Count()
            }
        };
        return View(viewModel);
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