using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GroovyGoodsWebApplication.Controllers
{
    public class SupplierProductsUnitTestController : Controller
    {
        public List<SupplierProduct> supplierProductsList;
        public List<Product> productsList;
        public List<Supplier> suppliersList;
        public List<SupplierProduct> resultsList;
        public List<Supplier> GetSuppliers()
        {
            return new List<Supplier>
            {
                new Supplier
                {
                    Sid = 1,
                    Company = "SoundWave Supplies",
                    ContactName = "John Smith",
                    Email = "john@example.com",
                    Phone = "0211234567",
                    Address = "123 Music Street",
                    City = "Auckland",
                    Postcode = 1010,
                    Country = "New Zealand"
                },
                new Supplier
                {
                    Sid = 2,
                    Company = "Harmony Audio",
                    ContactName = "Sarah Johnson",
                    Email = "sarah@example.com",
                    Phone = "0279876543",
                    Address = "456 Audio Avenue",
                    City = "Wellington",
                    Postcode = 6011,
                    Country = "New Zealand"

                }
            };
        }

        public List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Pid = 1,
                    Name = "Acoustic Guitar",
                    Description = "High-quality acoustic guitar with spruce top",
                    ListPrice = (decimal)299.99,
                    Stock = 50
                },
                new Product
                {
                    Pid = 2,
                    Name = "Electric Keyboard",
                    Description = "61-key electric keyboard with built-in sounds",
                    ListPrice = (decimal)199.99,
                    Stock = 30
                }
            };
        }

        public List<SupplierProduct> GetSupplierProducts()
        {
            return new List<SupplierProduct>
            {
                new SupplierProduct
                {
                    Spid = 1,
                    Pid = 1,
                    Sid = 1,
                    Cost = (decimal)149.99
                },
                new SupplierProduct
                {
                    Spid = 2,
                    Pid = 2,
                    Sid = 2,
                    Cost = (decimal) 86.99
                },
                new SupplierProduct
                {
                    Spid = 3,
                    Pid = 2,
                    Sid = 2,
                    Cost = (decimal)99.99
                }         
            };
        }

        public SelectList[] PopulateLists()
        {
            //Get list of products with id and name
            var productsList = GetProducts().Select(p => new SelectListItem
            {
                Value = p.Pid.ToString(),
                Text = $"{p.Pid} - {p.Name}"
            }).ToList();
            SelectList products = new SelectList(productsList, "Value", "Text");

            //Get list of suppliers with id and company name
            var suppliersList = GetSuppliers().Select(s => new SelectListItem
            {
                Value = s.Sid.ToString(),
                Text = $"{s.Sid} - {s.Company}"
            }).ToList();
            SelectList suppliers = new SelectList(suppliersList, "Value", "Text");
            SelectList[] listarray = new SelectList[] { products, suppliers };
            return listarray;
        }

        public IActionResult Index()
        {
            supplierProductsList = GetSupplierProducts();
            productsList = GetProducts();
            suppliersList = GetSuppliers();
            return View(supplierProductsList);
        }

        public IActionResult Create()
        {
            supplierProductsList = GetSupplierProducts();
            SupplierProduct supplierProduct = new SupplierProduct { Spid = 4, Pid = 1, Sid = 2, Cost = (decimal)153.99 };
            supplierProductsList.Add(supplierProduct);
            return View(supplierProductsList);
        }

        public IActionResult Edit()
        {
            supplierProductsList = GetSupplierProducts();
            SupplierProduct supplierProduct = supplierProductsList.First();
            supplierProduct.Sid = 2;
            supplierProduct.Cost = (decimal)153.99;
            return View(supplierProduct);
        }

        public IActionResult Delete()
        {
            int id = 3;
            supplierProductsList = GetSupplierProducts();
            SupplierProduct supplierProduct = supplierProductsList.FirstOrDefault(s => s.Spid == id);
            supplierProductsList.Remove(supplierProduct);
            return View(supplierProductsList);
        }
    }
}
