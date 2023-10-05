using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace GroovyGoodsWebApplication.Controllers
{
    public class SupplierUnitTestController : Controller
    {
        public List<Supplier> suppliersList;
        public List<Supplier> resultsList;
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

                },
                new Supplier
                {
                    Sid = 3,
                    Company = "Rhythm House Music",
                    ContactName = "David Brown ",
                    Email = "david@example.com",
                    Phone = "0212345678",
                    Address = "789 Beat Road",
                    City = "Christchurch",
                    Postcode = 8011,
                    Country = "New Zealand"
                },
                new Supplier
                {
                    Sid = 4,
                    Company = "Melody World",
                    ContactName = "Emily Davis",
                    Email = "emily@example.com",
                    Phone = "0278765432",
                    Address = "321 Harmony Lane",
                    City = "Dunedin",
                    Postcode = 9016,
                    Country = "New Zealand"
                },
                new Supplier
                {
                    Sid = 5,
                    Company = "Sonic Sounds ",
                    ContactName = "Michael Lee",
                    Email = "michael@example.com",
                    Phone = "0213456789",
                    Address = "555 Groove Street",
                    City = "Hamilton",
                    Postcode = 3204,
                    Country = "New Zealand"
                }
            };
        }
        public IActionResult Index()
        {
            suppliersList = GetSuppliers().ToList();
            return View(suppliersList);
        }

        public IActionResult Create()
        {
            suppliersList = GetSuppliers();
            Supplier supplier = new Supplier
            {
                Sid = 6,
                Company = "Tempo Tunes",
                ContactName = "Lisa Wilson",
                Email = "lisa@example.com",
                Phone = "027-765-4321",
                Address = "888 Tempo Avenue",
                City = "Tauranga",
                Postcode = 3110,
                Country = "New Zealand"
            };
            suppliersList.Add(supplier);
            return View(suppliersList);
        }

        public IActionResult Edit()
        {
            suppliersList = GetSuppliers();
            Supplier supplier = suppliersList[4];
            supplier.Company = "Sanic Sounds";
            return View(suppliersList);
        }

        public IActionResult Delete()
        {
            suppliersList = GetSuppliers();
            int supplierID = 5;
            Supplier supplier = suppliersList.FirstOrDefault(s => s.Sid == supplierID);
            suppliersList.Remove(supplier);
            return View(suppliersList);
        }

        public IActionResult Search(string searchString)
        {
            suppliersList = GetSuppliers();
            resultsList = suppliersList.Where(s =>
                    s.Company.ToLower().Contains(searchString) ||
                    s.ContactName.ToLower().Contains(searchString) ||
                    s.Email.ToLower().Contains(searchString) ||
                    s.Phone.ToLower().Contains(searchString) ||
                    s.Address.ToLower().Contains(searchString) ||
                    s.City.ToLower().Contains(searchString) ||
                    s.Postcode.ToString().ToLower().Contains(searchString) ||
                    s.Country.ToLower().Contains(searchString)).ToList();
            return View(resultsList);
        }

        public IActionResult Sort(string sortOrder)
        {
            suppliersList = GetSuppliers();
            switch (sortOrder)
            {
                case "company":
                    resultsList = suppliersList.OrderBy(s => s.Company).ToList();
                    break;
                case "contactName":
                    resultsList = suppliersList.OrderBy(s => s.ContactName).ToList();
                    break;
                case "email":
                    resultsList = suppliersList.OrderBy(s => s.Email).ToList();
                    break;
                case "phone":
                    resultsList = suppliersList.OrderBy(s => s.Phone).ToList();
                    break;
                case "address":
                    resultsList = suppliersList.OrderBy(s => s.Address).ToList();
                    break;
                case "city":
                    resultsList = suppliersList.OrderBy(s => s.City).ToList();
                    break;
                case "postcode":
                    resultsList = suppliersList.OrderBy(s => s.Postcode).ToList();
                    break;
                case "country":
                    resultsList = suppliersList.OrderBy(s => s.Country).ToList();
                    break;
                case "company_desc":
                    resultsList = suppliersList.OrderByDescending(s => s.Company).ToList();
                    break;
                case "contactName_desc":
                    resultsList = suppliersList.OrderByDescending(s => s.ContactName).ToList();
                    break;
                case "email_desc":
                    resultsList = suppliersList.OrderByDescending(s => s.Email).ToList();
                    break;
                case "phone_desc":
                    resultsList = suppliersList.OrderByDescending(s => s.Phone).ToList();
                    break;
                case "address_desc":
                    resultsList = suppliersList.OrderByDescending(s => s.Address).ToList();
                    break;
                case "city_desc":
                    resultsList = suppliersList.OrderByDescending(s => s.City).ToList();
                    break;
                case "postcode_desc":
                    resultsList = suppliersList.OrderByDescending(s => s.Postcode).ToList();
                    break;
                case "country_desc":
                    resultsList = suppliersList.OrderByDescending(s => s.Country).ToList();
                    break;
                default:
                    break;
            }
            return View(resultsList);
        }
    }
}
