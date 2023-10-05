using GroovyGoodsWebApplication.Controllers;
using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroovyGoodsTestProject
{
    [TestClass]
    public class SupplierProductUnitTest
    {
        SupplierProductsUnitTestController controller;
        [TestInitialize]
        public void initTest()
        {
            controller = new SupplierProductsUnitTestController();
        }

        [TestCleanup]
        public void cleanupTest()
        {
            controller.Dispose();
        }

        [TestMethod]
        public void SupplierProductPopulateListsTest()
        {
            SupplierProductsUnitTestController supplierProductsUnitTestController = new SupplierProductsUnitTestController();
            SelectList[] selectListItems = supplierProductsUnitTestController.PopulateLists();
            Assert.IsNotNull(selectListItems);
            Assert.AreEqual(2, selectListItems.Length);
            SelectList products = selectListItems[0];
            SelectList suppliers = selectListItems[1];
            Assert.IsNotNull(products.Where(m => m.Text == "1 - Acoustic Guitar"));
            Assert.IsNotNull(products.Where(m => m.Text == "2 - Electric Keyboard"));
            Assert.IsNotNull(products.Where(m => m.Value == "1"));
            Assert.IsNotNull(products.Where(m => m.Value == "2"));
            Assert.IsNotNull(suppliers.Where(m => m.Text == "1 - SoundWave Supplies"));
            Assert.IsNotNull(suppliers.Where(m => m.Text == "2 - Harmony Audio"));
            Assert.IsNotNull(suppliers.Where(m => m.Value == "1"));
            Assert.IsNotNull(suppliers.Where(m => m.Value == "2"));
        }

        [TestMethod]
        public void SupplierProductsIndexTest()
        {
            IActionResult result = controller.Index();
            Assert.IsNotNull(result);
            List<SupplierProduct> supplierProducts = controller.supplierProductsList;
            List<Product> products = controller.productsList;
            List<Supplier> suppliers = controller.suppliersList;
            Assert.AreEqual(3, supplierProducts.Count);
            Assert.AreEqual("Acoustic Guitar", products.Where(p => p.Pid == supplierProducts.First().Pid).First().Name);
            Assert.AreEqual("SoundWave Supplies", suppliers.Where(s => s.Sid == supplierProducts.First().Sid).First().Company);
        }

        [TestMethod]
        public void SupplierProductsCreateTest()
        {
            IActionResult result = controller.Create();
            Assert.IsNotNull(result);
            List<SupplierProduct> supplierProducts = controller.supplierProductsList;
            Assert.AreEqual(4, supplierProducts.Count);
        }

        [TestMethod]
        public void SupplierProductsEditTest()
        {
            IActionResult result = controller.Edit();
            Assert.IsNotNull(result);
            List<SupplierProduct> supplierProducts = controller.supplierProductsList;
            Assert.AreEqual(2, supplierProducts.First().Sid);
            Assert.AreEqual((decimal)153.99, supplierProducts.First().Cost);
        }

        [TestMethod]
        public void SupplierProductsDeleteTest()
        {
            IActionResult result = controller.Delete();
            Assert.IsNotNull(result);
            List<SupplierProduct> supplierProducts = controller.supplierProductsList;
            Assert.AreEqual(2, supplierProducts.Count);
        }
    }
}
