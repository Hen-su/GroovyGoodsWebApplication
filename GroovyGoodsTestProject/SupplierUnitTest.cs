using GroovyGoodsWebApplication.Controllers;
using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroovyGoodsTestProject
{
    [TestClass]
    public class SupplierUnitTest
    {
        SupplierUnitTestController controller;
        [TestInitialize]
        public void initTest()
        {
            controller = new SupplierUnitTestController();
        }

        [TestCleanup]
        public void cleanupTest()
        {
            controller.Dispose();
        }

        [TestMethod]
        public void SupplierIndexTest()
        {
            IActionResult result = controller.Index();
            Assert.IsNotNull(controller);
            List<Supplier> suppliers = controller.suppliersList;
            Assert.AreEqual(5, suppliers.Count);
        }

        [TestMethod]
        public void SupplierCreateTest()
        {
            IActionResult result = controller.Create();
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList;
            Assert.AreEqual(6, suppliers.Count);
        }

        [TestMethod]
        public void SupplierEditTest()
        {
            IActionResult result = controller.Edit();
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList;
            Assert.AreEqual("Sanic Sounds", suppliers[suppliers.Count-1].Company);
        }

        [TestMethod]
        public void SupplierDeleteTest()
        {
            IActionResult result = controller.Delete();
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList;
            Assert.AreEqual(4, suppliers.Count);
        }

        [TestMethod]
        public void SupplierSearchTest()
        {
            string searchString = "music";
            IActionResult result = controller.Search(searchString);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.resultsList;
            Assert.AreEqual(2, suppliers.Count);
            foreach (Supplier s in suppliers)
            {
                Assert.IsTrue(s.Company.ToLower().Contains(searchString) ||
                    s.ContactName.ToLower().Contains(searchString) ||
                    s.Email.ToLower().Contains(searchString) ||
                    s.Phone.ToLower().Contains(searchString) ||
                    s.Address.ToLower().Contains(searchString) ||
                    s.City.ToLower().Contains(searchString) ||
                    s.Postcode.ToString().ToLower().Contains(searchString) ||
                    s.Country.ToLower().Contains(searchString));
            }
        }

        [TestMethod]
        public void SupplierSortCompanyAsc()
        {
            string sortOrder = "company";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderBy(s => s.Company).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortContactAsc()
        {
            string sortOrder = "contactName";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderBy(s => s.ContactName).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortEmailAsc()
        {
            string sortOrder = "email";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderBy(s => s.Email).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortPhoneAsc()
        {
            string sortOrder = "phone";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderBy(s => s.Phone).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortAddressAsc()
        {
            string sortOrder = "address";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderBy(s => s.Address).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortCityAsc()
        {
            string sortOrder = "city";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderBy(s => s.City).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortPostcodeAsc()
        {
            string sortOrder = "postcode";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderBy(s => s.Postcode).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortCountryAsc()
        {
            string sortOrder = "country";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderBy(s => s.Country).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortCompanyDesc()
        {
            string sortOrder = "company_desc";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderByDescending(s => s.Company).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortContactDesc()
        {
            string sortOrder = "contactName_desc";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderByDescending(s => s.ContactName).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortEmailDesc()
        {
            string sortOrder = "email_desc";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderByDescending(s => s.Email).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortPhoneDesc()
        {
            string sortOrder = "phone_desc";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderByDescending(s => s.Phone).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortAddressDesc()
        {
            string sortOrder = "address_desc";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderByDescending(s => s.Address).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortCityDesc()
        {
            string sortOrder = "city_desc";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderByDescending(s => s.City).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortPostcodeDesc()
        {
            string sortOrder = "postcode_desc";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderByDescending(s => s.Postcode).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }

        [TestMethod]
        public void SupplierSortCountryDesc()
        {
            string sortOrder = "country_desc";
            IActionResult result = controller.Sort(sortOrder);
            Assert.IsNotNull(result);
            List<Supplier> suppliers = controller.suppliersList.OrderByDescending(s => s.Country).ToList();
            List<Supplier> results = controller.resultsList;
            CollectionAssert.AreEqual(results, suppliers);
        }
    }
}
