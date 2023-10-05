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
        public void ProductIndexTest()
        {
            IActionResult result = controller.Index();
            Assert.IsNotNull(controller);
            List<Product> products = controller.GetProducts();
            Assert.AreEqual(5, products.Count);
        }

        [TestMethod]
        public void ProductCreateTest()
        {
            IActionResult result = controller.Create();
            Assert.IsNotNull(result);
            List<Product> products = controller.productsList;
            Assert.AreEqual(6, products.Count);
        }

        [TestMethod]
        public void ProductEditTest()
        {
            IActionResult result = controller.Edit();
            Assert.IsNotNull(result);
            List<Product> productsList = controller.productsList;
            Product product = productsList[4];
            Assert.AreEqual(5, product.Pid);
            Assert.AreEqual("Digital Keyboard", product.Name);
            Assert.AreEqual("76-key digital keyboard", product.Description);
            Assert.AreEqual((decimal)499.99, product.ListPrice);
            Assert.AreEqual(15, product.Stock);
        }

        [TestMethod]
        public void ProductDeleteTest()
        {
            IActionResult result = controller.Delete();
            Assert.IsNotNull(result);
            List<Product> productsList = controller.productsList;
            Assert.AreEqual(4, productsList.Count);
            Assert.AreEqual(4, productsList[productsList.Count - 1].Pid);
        }

        [TestMethod]
        public void ProductSearch()
        {
            string searchString = "guitar";
            IActionResult result = controller.Search(searchString);
            Assert.IsNotNull(result);
            List<Product> productsList = controller.resultsList;
            Assert.AreEqual(2, productsList.Count);
            foreach (Product product in productsList)
            {
                Assert.IsTrue(product.Name.ToLower().Contains(searchString) || product.Description.ToLower().Contains(searchString));
            }
        }

        [TestMethod]
        public void ProductSortNameAsc()
        {
            IActionResult result = controller.Sort("name");
            Assert.IsNotNull(result);
            List<Product> products = controller.productsList.OrderBy(p => p.Name).ToList();
            List<Product> results = controller.resultsList;
            CollectionAssert.AreEqual(results, products);
        }

        [TestMethod]
        public void ProductSortDescriptionAsc()
        {
            IActionResult result = controller.Sort("description");
            Assert.IsNotNull(result);
            List<Product> products = controller.productsList.OrderBy(p => p.Description).ToList();
            List<Product> results = controller.resultsList;
            CollectionAssert.AreEqual(results, products);
        }

        [TestMethod]
        public void ProductSortPriceAsc()
        {
            IActionResult result = controller.Sort("listPrice");
            Assert.IsNotNull(result);
            List<Product> products = controller.productsList.OrderBy(p => p.ListPrice).ToList();
            List<Product> results = controller.resultsList;
            CollectionAssert.AreEqual(results, products);
        }

        [TestMethod]
        public void ProductSortNameDesc()
        {
            IActionResult result = controller.Sort("name_desc");
            Assert.IsNotNull(result);
            List<Product> products = controller.productsList.OrderByDescending(p => p.Name).ToList();
            List<Product> results = controller.resultsList;
            CollectionAssert.AreEqual(results, products);
        }

        [TestMethod]
        public void ProductSortDescriptionDesc()
        {
            IActionResult result = controller.Sort("description_desc");
            Assert.IsNotNull(result);
            List<Product> products = controller.productsList.OrderByDescending(p => p.Description).ToList();
            List<Product> results = controller.resultsList;
            CollectionAssert.AreEqual(results, products);
        }

        [TestMethod]
        public void ProductSortPriceDesc()
        {
            IActionResult result = controller.Sort("listPrice_desc");
            Assert.IsNotNull(result);
            List<Product> products = controller.productsList.OrderByDescending(p => p.ListPrice).ToList();
            List<Product> results = controller.resultsList;
            CollectionAssert.AreEqual(results, products);
        }
    }
}