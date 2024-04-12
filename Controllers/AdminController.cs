using System.Diagnostics;
using AuroraBricks.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using AuroraBricks.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Drawing;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using System.Numerics.Tensors;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace AuroraBricks.Controllers;

[Authorize(Policy = "AdminPolicy")]
public class AdminController : Controller
{
        
        private readonly UserManager<IdentityUser> _userManager;
        private IBrixRepository _repo;
        // private readonly string _onnxModelPath;
        
           
        public AdminController(IBrixRepository temp, UserManager<IdentityUser> userManager, MLContext mlContext)
        {
                _repo = temp;
                _userManager = userManager;
                // _onnxModelPath = System.IO.Path.Combine(HostEnvironment.ContentRootPath, "fraudModel.onnx");
                // var _session = new InterfaceSession(_onnxModelPath);
                // _mlContext = mlContext;
        }
        
        [HttpGet]
        public IActionResult EditUser(int id)
        {
                var recordToEdit = _repo.Customers
                        .Single(x => x.CustomerId == id);

                return View("EditUser", recordToEdit);
        }
        
        [HttpPost]
        public async Task<IActionResult>EditUser(BrixCustomer customer)
        {
                _repo.EditUser(customer);

                if (customer.Email != null)
                {
                        var user = await _userManager.FindByEmailAsync(customer.Email);
                        var userRoles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, userRoles.ToArray());
                        await _userManager.AddToRoleAsync(user, customer.Role);
                }


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
                        .ThenByDescending(x => x.Time)
                        .ToList();
                return View(orders);
        }




        
        
        //
        // public IActionResult Predict(int time, int amount, int country)
        // {
        //         // Load the trained pipeline
        //         var pipeline = GetPredictedPipeline(_mlContext);
        //
        //         // Create a new OnnxInput object with the input data
        //         var input = new OnnxInput { time = time, amount = amount, contryoftransactionUnitedKingdom = country };
        //
        //         // Use the trained pipeline to make predictions
        //         var prediction = pipeline.Transform(_mlContext.Data.LoadFromEnumerable(new[] { input }));
        //
        //         // Extract the predicted fraud values
        //         var predictedFraud = prediction.GetColumn<float[]>(nameof(OnnxOutput.PredictedFraud)).First();
        //
        //         // Do something with the predicted fraud values
        //         return View();
        // }
        // static ITransformer GetPredictedPipeline(MLContext mlContext)
        // {
        //         var inputColumns = new string []
        //         {
        //                 "time", "amount", "country_of_transaction_United Kingdom"
        //         };
        //
        //         var outputColumns = new string [] { "Predicted_Fraud" };  
        //         
        //         var onnxPredictionPipeline =
        //                 mlContext.Transforms.ApplyOnnxModel(outputColumnNames: outputColumns, inputColumnNames: inputColumns, ONNX_MODEL_PATH);
        //         var emptyDv = mlContext.Data.LoadFromEnumerable(new OnnxInput[] {});
        //
        //         return onnxPredictionPipeline.Fit(emptyDv);
        // }

                
                
                
                
                
                
                
                
        // var predictions = new List<OnnxInput>(); //This is the ViewModel for the view
        //
        // var class_type_dict = new Dictionary<int, string>
        //         {
        //             { 0, "Not Fraud" },
        //             { 1, "Fraud" }
        //         };
        // foreach (var order in orders)
        // {
        //     //Calculate days since
        //     var jan1_22 = new DateTime(2022, 1, 1);
        //     var daysSinceJan1_22 = order.Date.HasValue ? Math.Abs((order.Date.Value - jan1_22).Days) : 0;
        //
        //     //Preprocess features to make compatible with model
        //     var input = new List<float>
        //     {
        //         (float)order.CustomerId,
        //         (float)order.Time,
        //         //Fix amount if not null
        //         (float)(order.Amount ?? 0),
        //
        //         //Fix date
        //         daysSinceJan1_22,
        //
        //         //Fill in here with dummy code data
        //         
        //
        //         //Use CountryOfTransaction if ShippingAddress is null
        //         (order.ShippingAddress ?? order.CountryOfTransaction) == "India" ? 1 : 0,
        //         (order.ShippingAddress ?? order.CountryOfTransaction) == "Russia" ? 1 : 0,
        //         (order.ShippingAddress ?? order.CountryOfTransaction) == "USA" ? 1 : 0,
        //         (order.ShippingAddress ?? order.CountryOfTransaction) == "UnitedKingdom" ? 1 : 0,
        //
        //         //More dummy code
        //
        //         order.TypeOfCard == "Visa" ? 1 : 0
        //     };
        //     var inputTensor = new DenseTensor<float>(input.ToArray(), new[] {1, input.Count });
        //
        //     var inputs = new List<NamedOnnxValue>
        //     {
        //         NamedOnnxValue.CreateFromTensor("float_input", inputTensor)
        //     };
        //
        //     string predictionResult;
        //     using (var results = _session.Run(inputs))
        //     {
        //         var prediction = results.FirstOrDefault(BrixLineItem => BrixLineItem.Name == "output_label")?.AsTensor<long>();
        //         predictionResult = prediction != null && prediction.Length > 0 ? class_type_dict.GetValueOrDefault((int)prediction[0], "Unknown") : "Error in prediction";
        //     }
        //
        //     predictions.Add(new OnnxInput { Orders = order, Prediction = predictionResult });
        // }
        //
        // return View(predictions);
        //
        // }
        //
        //
        //
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
