using GroovyGoodsWebApplication.Controllers;
using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroovyGoodsTestProject
{
    [TestClass]
    public class ProductUnitTest
    {
        ProductUnitTestController controller;
        [TestInitialize]
        public void initTest()
        {
            controller = new ProductUnitTestController();
        }

        [TestCleanup]
        public void cleanupTest()
        {
            controller.Dispose();
        }

        [TestMethod]
        public void IndexTest()
        {
            IActionResult result = controller.Index();
            Assert.IsNotNull(controller);
            List<Product> products = controller.GetProducts();
            Assert.AreEqual(5, products.Count);
        }

        [TestMethod]
        public void CreateTest()
        {
            IActionResult result = controller.Create();
            Assert.IsNotNull(result);
            List<Product> products = controller.GetProducts();
            Assert.AreEqual(6, products.Count);
        }

        [TestMethod]
        public void EditTest()
        {
            IActionResult result = controller.Edit();
            Assert.IsNotNull(result);
            List <Product> products = controller.GetProducts();
            Product product = products[5];
            Assert.AreEqual(5, product.Pid);
            Assert.AreEqual("Digital Keyboard", product.Name);
            Assert.AreEqual("76-key digital keyboard", product.Description);
            Assert.AreEqual(499.99, product.ListPrice);
            Assert.AreEqual(15, product.Stock);
        }
    }
}