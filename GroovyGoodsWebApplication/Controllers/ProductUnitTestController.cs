using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace GroovyGoodsWebApplication.Controllers
{
    public class ProductUnitTestController : Controller
    {
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
            var products = from p in GetProducts()
                           select p;
            return View(products);
        }

        public IActionResult Create()
        {
            List<Product> productList = GetProducts();
            Product product = new Product { Pid = 6, Name = "Violin", Description = "Handcrafted violin with bow and case", ListPrice = (decimal)199.99, Stock = 1 };
            productList.Add(product);
            return View(productList);
        }

        public IActionResult Edit()
        {
            List<Product> productList = GetProducts();
            Product product = productList[5];
            product.Name = "Digital Keyboard";
            product.Description = "76-key digital keyboard";
            product.ListPrice = (decimal)499.99;
            return View(productList);
        }
    }
}
