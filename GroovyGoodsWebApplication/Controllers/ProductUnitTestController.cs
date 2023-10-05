using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GroovyGoodsWebApplication.Controllers
{
    public class ProductUnitTestController : Controller
    {
        public List<Product> productsList;
        public List<Product> resultsList;
        public List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Pid = 1,
                    Name = "Acoustic Guitar",
                    Description = "High-quality acoustic guitar with spruce top",
                    ListPrice = (decimal) 299.99,
                    Stock = 50
                },
                new Product
                {
                    Pid = 2,
                    Name = "Electric Keyboard",
                    Description = "61-key electric keyboard with built-in sounds",
                    ListPrice = (decimal) 199.99,
                    Stock = 30
                },
                new Product
                {
                    Pid = 3,
                    Name = "Drum Set",
                    Description = "Complete 5-piece drum set with cymbals",
                    ListPrice = (decimal) 499.99,
                    Stock = 20
                },
                new Product
                {
                    Pid = 4,
                    Name = "Bass Guitar",
                    Description = "Precision bass guitar with maple neck",
                    ListPrice = (decimal) 349.99,
                    Stock = 40
                },
                new Product
                {
                    Pid = 5,
                    Name = "Digital Piano",
                    Description = "88-key digital piano with weighted keys",
                    ListPrice = (decimal) 699.99,
                    Stock = 15
                }
            }; 
        }

        public IActionResult Index()
        {
            productsList = GetProducts();
            var products = from p in GetProducts()
                           select p;
            return View(productsList);
        }

        public IActionResult Create()
        {
            productsList = GetProducts();
            Product product = new Product { Pid = 6, Name = "Violin", Description = "Handcrafted violin with bow and case", ListPrice = (decimal)199.99, Stock = 1 };
            productsList.Add(product);
            return View(productsList);
        }

        public IActionResult Edit()
        {
            productsList = GetProducts();
            Product product = productsList[4];
            product.Name = "Digital Keyboard";
            product.Description = "76-key digital keyboard";
            product.ListPrice = (decimal)499.99;
            return View(productsList);
        }

        public IActionResult Delete()
        {
            productsList = GetProducts();
            int productID = 5;
            Product product = productsList.FirstOrDefault(p => p.Pid == productID);
            productsList.Remove(product);
            return View(productsList);
        }

        public IActionResult Search(string searchString)
        {
            List<Product> productsList = GetProducts();
            resultsList = productsList.Where(p => p.Name.ToLower().Contains(searchString) || p.Description.ToLower().Contains(searchString)).ToList();
            return View(resultsList);
        }

        public IActionResult Sort(string sortOrder)
        {
            productsList = GetProducts();
            switch (sortOrder)
            {
                case "name":
                    resultsList = productsList.OrderBy(p => p.Name).ToList();
                    break;
                case "description":
                    resultsList = productsList.OrderBy(p => p.Description).ToList();
                    break;
                case "listPrice":
                    resultsList = productsList.OrderBy(p => p.ListPrice).ToList();
                    break;
                case "stock":
                    resultsList = productsList.OrderBy(p => p.Stock).ToList();
                    break;
                case "name_desc":
                    resultsList = productsList.OrderByDescending(p => p.Name).ToList();
                    break;
                case "description_desc":
                    resultsList = productsList.OrderByDescending(p => p.Description).ToList();
                    break;
                case "listPrice_desc":
                    resultsList = productsList.OrderByDescending(p => p.ListPrice).ToList();
                    break;
                case "stock_desc":
                    resultsList = productsList.OrderByDescending(p => p.Stock).ToList();
                    break;
                default:
                    break;
            }
            return View(resultsList);
        }
    }
}
