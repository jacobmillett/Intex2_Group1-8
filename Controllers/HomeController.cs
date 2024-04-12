using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuroraBricks.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using AuroraBricks.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using System.Numerics.Tensors;
using AuroraBricks.Views.Home;
using Google.Protobuf.WellKnownTypes;
using Microsoft.ML;
using Microsoft.ML.Data;
using AuroraBricks.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace AuroraBricks.Controllers;

public class HomeController : Controller
{
    private readonly MLContext _mlContext;
    private IBrixRepository _repo;
    private Cart cart;
    private readonly InferenceSession _session;
    private readonly IWebHostEnvironment _environment;

    private readonly UserManager<IdentityUser> _userManager;
    public HomeController(IBrixRepository temp, UserManager<IdentityUser> userManager, MLContext mlContext, Cart cartService, IWebHostEnvironment environment)
    {
        _userManager = userManager;
        _repo = temp;
        _mlContext = mlContext;
        cart = cartService;
        _environment = environment;
        string modelPath;

        if (_environment.IsDevelopment())
        {
        
            modelPath = Path.Combine(_environment.ContentRootPath, "fraudModel.onnx");
        }
        else
        {

            modelPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "fraudModel.onnx");
        }
        _session = new InferenceSession(modelPath);
    }
    

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userEmail = _userManager.GetUserAsync(User).Result?.Email;
        var customer = await _repo.GetBrixCustomerByEmailAsync(userEmail);

        if (customer != null)
        {
            var userRecommendations = _repo.UserRecommendations
                .Where(x => x.CustomerId == 1)
                .FirstOrDefault();
        if (userRecommendations.CustomerId == 1)
        {
        
            var productInfo = new List<BrixProduct>();

            var product1 = _repo.Products.FirstOrDefault(x => x.Name == userRecommendations.Product1);
            if (product1 != null)
            {
                productInfo.Add(product1);
            }
            var product2 = _repo.Products.FirstOrDefault(x => x.Name == userRecommendations.Product2);
            if (product2 != null)
            {
                productInfo.Add(product2);
            }
            var product3 = _repo.Products.FirstOrDefault(x => x.Name == userRecommendations.Product3);
            if (product3 != null)
            {
                productInfo.Add(product3);
            }
            var product4 = _repo.Products.FirstOrDefault(x => x.Name == userRecommendations.Product4);
            if (product4 != null)
            {
                productInfo.Add(product4);
            }
            var product5 = _repo.Products.FirstOrDefault(x => x.Name == userRecommendations.Product5);
            if (product5 != null)
            {
                productInfo.Add(product5);
            }
        
            return View(productInfo);
        }
        else
        {
                return View();
             }
        }
        else
        {
            return View();
        }
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

        return Redirect("~/Identity/Account/Register");
      
    }

    public ActionResult ProductList(string Category, string PrimaryColor, int pageNum = 1, int pageSize = 5)
    {
        var query = _repo.Products.AsQueryable();

        // Apply filtering based on category and primary color
        if (!string.IsNullOrEmpty(Category))
        {
            query = query.Where(p => p.Category.Equals(Category, StringComparison.OrdinalIgnoreCase));
        }
        if (!string.IsNullOrEmpty(PrimaryColor))
        {
            query = query.Where(p => p.PrimaryColor.Equals(PrimaryColor, StringComparison.OrdinalIgnoreCase));
        }

        // Pagination calculations
        int totalItems = query.Count();
        var products = query
            .OrderBy(x => x.Name)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new ProductsListViewModel
        {
            Products = products,
            Categories = new SelectList(_repo.Products.Select(p => p.Category).Distinct().OrderBy(c => c)),
            PrimaryColors = new SelectList(_repo.Products.Select(p => p.PrimaryColor).Distinct().OrderBy(c => c)),
            SelectedCategory = Category,
            SelectedPrimaryColor = PrimaryColor,
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = pageNum,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            }
        };

        return View(model);
    }



    //public IActionResult ProductList(int pageNum, int pageSize = 5)
    //{
    //    // Prepare the base query to be filtered
    //    var baseQuery = _repo.Products.AsQueryable();

    //    var viewModel = new ProductsListViewModel
    //    {
    //        Product = baseQuery
    //            .OrderBy(x => x.Name)
    //            .Skip((pageNum - 1) * pageSize)
    //            .Take(pageSize)
    //            .ToList(),

    //        PaginationInfo = new PaginationInfo
    //        {
    //            CurrentPage = pageNum,
    //            ItemsPerPage = pageSize,
    //            TotalItems = _repo.Products.Count()
    //        }
    //    };
    //    return View(viewModel);
    //}




    public IActionResult ProductDetail(int ProductId)
    {
        var product = _repo.Products.FirstOrDefault(p => p.ProductId == ProductId);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
    
    
    [HttpGet]
    public IActionResult EditCustomerProfile(int id)
    {
        var recordToEdit = _repo.Customers
            .Single(x => x.CustomerId == id);

        return View("CustomerSignUp", recordToEdit);
    }

    
    
    [HttpPost]
    public IActionResult EditCustomerProfile(BrixCustomer customer)
    {

        _repo.EditUser(customer);
        return RedirectToAction("CustomerProfile");
        
    }
    
    [HttpGet]
    public async Task<IActionResult> CustomerProfile()
    {
        var userEmail = _userManager.GetUserAsync(User).Result?.Email;
        var customer = await _repo.GetBrixCustomerByEmailAsync(userEmail);

        return View(customer);
    }

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
    public IActionResult AddToCart(int productId, string returnUrl)
    {
        var cart = SessionCart.GetCart(HttpContext.RequestServices);
        var product = _repo.Products.FirstOrDefault(p => p.ProductId == productId);

        if (product != null)
        {
            cart.AddItem(product, 1);
        }

        return RedirectToAction("Cart", new { returnUrl });
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

    [HttpGet]
    public IActionResult Checkout()
    {
        var details = new CartViewModel
            { Cart = cart };
        
        return View(details);
    }



    [HttpPost]
        public IActionResult Checkout(CartViewModel details)
        {


            
            if (ModelState.IsValid)
            {
                //Calculate days since Jan 1 2022
                //var january12022 = new DateTime(2022, 1, 1);
                //var daysSinceJan2022 = Math.Abs((checkoutDetails.Order.OrderDate - january12022).Days);



                var classTypeDict = new Dictionary<int, string>
                {
                    { 0, "Not Fraud" },
                    { 1, "Fraud" }
                };

                details.Order.Amount = (float)cart.ComputeTotalValue();


                var input = new List<float>
                    {
                        details.Order.CustomerId,
                        (float)details.Order.Time,
				        (float)(details.Order.Amount),

				        //fix date
				        //daysSinceJan2022,

				        //check the dummy coded data
				        // order.DayOfWeek == "Mon" ? 1 : 0,
            //             order.DayOfWeek == "Sat" ? 1 : 0,
            //             order.DayOfWeek == "Sun" ? 1 : 0,
            //             order.DayOfWeek == "Thu" ? 1 : 0,
            //             order.DayOfWeek == "Tue" ? 1 : 0,
            //             order.DayOfWeek == "Wed" ? 1 : 0,
            //
            //             order.EntryMode == "Pin" ? 1 : 0,
            //             order.EntryMode == "Tap" ? 1 : 0,
            //
            //             order.TypeOfTransaction == "Online" ? 1 : 0,
            //             order.TypeOfTransaction == "POS" ? 1 : 0,
            //
            //             order.CountryOfTransaction == "India" ? 1 : 0,
            //             order.CountryOfTransaction == "Russia" ? 1 : 0,
            //             order.CountryOfTransaction == "USA" ? 1 : 0,
            //             order.CountryOfTransaction == "United Kingdom" ? 1 : 0,
            //             order.CountryOfTransaction == "China" ? 1 : 0,

				        //use countryoftransaction if shipping address is null
                        (details.Order.ShippingAddress ?? details.Order.CountryOfTransaction) == "United Kingdom" ? 1 : 0
                        //
                        // order.Bank == "HSBC" ? 1 : 0,
                        // order.Bank == "Halifax" ? 1 : 0,
                        // order.Bank == "Lloyds" ? 1 : 0,
                        // order.Bank == "Metro" ? 1 : 0,
                        // order.Bank == "Monzo" ? 1 : 0,
                        // order.Bank == "RBS" ? 1 : 0,
                        //
                        // order.TypeOfCard == "Visa" ? 1 : 0


                    };

                var inputTensor = new DenseTensor<float>(input.ToArray(), new[] { 1, input.Count });

                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
                };
                string predictionResult = null;
                //long predictionResultInt;

                using (var results = _session.Run(inputs))
                {
                    var prediction = results.FirstOrDefault(item => item.Name == "output_label")?.AsTensor<long>()
                        .ToArray();
                    predictionResult = prediction != null && prediction.Length > 0 ? classTypeDict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
                    
                }
                
                
                details.Order.Fraud = 0; 
                details.Order.FlagFraud = predictionResult;


                
                _repo.AddOrder(details.Order);

                foreach (var x in cart.Lines)
                {
                    var lineItem = new BrixLineItem
                    {
                        TransactionId = details.Order.TransactionId,
                        ProductId = x.Product.ProductId,
                        Qty  = x.Quantity,

                    };

                    _repo.AddLineItem(lineItem);
                }



                cart.Clear();

                return View("ConfirmOrder", details.Order);      
            }
            else
            {
                return View();
            }
        }
    }

