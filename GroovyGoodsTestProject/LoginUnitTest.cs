using GroovyGoodsWebApplication.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroovyGoodsTestProject
{
    [TestClass]
    public class LoginUnitTest
    {
        LoginUnitTestController controller;
        [TestInitialize]
        public void initTest()
        {
            controller = new LoginUnitTestController();
        }

        [TestCleanup]
        public void cleanupTest()
        {
            controller.Dispose();
        }

        [TestMethod]
        public void LoginIndexTest()
        {
            IActionResult result = controller.Index();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoginHashTest()
        { 
            string password = "password";
            string hashvalue = controller.HashPassword(password);
            Assert.AreEqual("XohImNooBHFR0OVvjcYpJ3NgPQ1qq73WKhHvch0VQtg=", hashvalue);
        }
    }
}
