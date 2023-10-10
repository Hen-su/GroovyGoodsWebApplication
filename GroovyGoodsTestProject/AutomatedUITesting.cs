using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroovyGoodsTestProject
{
    [TestClass]
    public class AutomatedUITesting
    {
        private readonly IWebDriver _driver;
        public AutomatedUITesting()
        {
            _driver = new ChromeDriver();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _driver.Dispose();
        }

        [TestMethod]
        public void Launchbrowser()
        {
            bool navItemsDisplayed;
            _driver.Navigate().GoToUrl("https://localhost:44314/");
            Assert.AreEqual("Login Page - GroovyGoodsWebApplication", _driver.Title);
            Assert.AreEqual("https://localhost:44314/Login?ReturnUrl=%2F", _driver.Url);
            var navItems = _driver.FindElements(By.ClassName("nav-item"));
            var logoutBTN = _driver.FindElements(By.Name("logout"));
            Assert.AreEqual(0, navItems.Count);
            Assert.AreEqual(0, logoutBTN.Count);
        }

        [TestMethod]
        public void UnauthenticatedNavigation()
        {
            _driver.Navigate().GoToUrl("https://localhost:44314/Products/");
            Assert.AreEqual("Login Page - GroovyGoodsWebApplication", _driver.Title);
            Assert.IsTrue(_driver.Url.Contains("https://localhost:44314/Login?ReturnUrl=%2F"));
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/");
            Assert.AreEqual("Login Page - GroovyGoodsWebApplication", _driver.Title);
            Assert.IsTrue(_driver.Url.Contains("https://localhost:44314/Login?ReturnUrl=%2F"));
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/");
            Assert.AreEqual("Login Page - GroovyGoodsWebApplication", _driver.Title);
            Assert.IsTrue(_driver.Url.Contains("https://localhost:44314/Login?ReturnUrl=%2F"));
        }

        [TestMethod]
        public void Login()
        {
            _driver.Navigate().GoToUrl("https://localhost:44314/");
            var usernameTXT = _driver.FindElement(By.Name("username"));
            var passwordTXT = _driver.FindElement(By.Name("password"));
            var loginBTN = _driver.FindElement(By.Name("login"));
            usernameTXT.SendKeys("Administrator");
            passwordTXT.SendKeys("Password123");
            loginBTN.Click();
            var navItems = _driver.FindElements(By.ClassName("nav-item"));
            var logoutBTN = _driver.FindElement(By.Name("logout"));
            Assert.AreEqual("https://localhost:44314/Products/Index", _driver.Url);
            foreach ( var navItem in navItems )
            {
                Assert.IsTrue(navItem.Displayed);
            }
            Assert.IsTrue(logoutBTN.Displayed);
        }

        [TestMethod]
        public void BlankLogin()
        {
            _driver.Navigate().GoToUrl("https://localhost:44314/");
            string loginUrl = _driver.Url;
            var usernameTXT = _driver.FindElement(By.Name("username"));
            var passwordTXT = _driver.FindElement(By.Name("password"));
            var loginBTN = _driver.FindElement(By.Name("login"));
            loginBTN.Click();
            Assert.AreEqual(loginUrl, _driver.Url);
        }

        [TestMethod]
        public void InvalidLogin()
        {
            _driver.Navigate().GoToUrl("https://localhost:44314/");
            string loginUrl = "https://localhost:44314/Login";
            string username = "Administrator";
            string password = "WrongPassword123";
            var usernameTXT = _driver.FindElement(By.Name("username"));
            var passwordTXT = _driver.FindElement(By.Name("password"));
            var loginBTN = _driver.FindElement(By.Name("login"));

            usernameTXT.SendKeys(username);
            passwordTXT.SendKeys(password);
            loginBTN.Click();
            Assert.AreEqual(loginUrl, _driver.Url);
        }

        [TestMethod]
        public void ProductIndex()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.IsNotNull(table);
            Assert.IsNotNull(rows);
        }

        [TestMethod]
        public void ProductSearch()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var searchTXT = _driver.FindElement(By.Name("SearchString"));
            var searchBTN = _driver.FindElement(By.Name("search"));
            string searchString = "guitar";
            searchTXT.SendKeys(searchString);
            searchBTN.Click();
            var searchTable = _driver.FindElement(By.ClassName("table"));
            var searchResults = searchTable.FindElements(By.TagName("tr"));
            List<string> results = new List<string>();
            foreach ( var row in searchResults )
            {
                results.Add((row.Text));
            }
            results.RemoveAt(0);
            foreach ( var row in searchResults )
            {
                Assert.IsTrue(results.Contains(searchString));
            }
        }

        [TestMethod]
        public void ProductSortNameAsc()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var nameHeading = table.FindElement(By.LinkText("Name"));
            nameHeading.Click();
            var sortedName = _driver.FindElement(By.ClassName("table"));
            var sortNameResults = sortedName.FindElements(By.TagName("tr"));
            List<string> names = new List<string>();
            for ( int i = 1; i < sortNameResults.Count; i++ )
            {
                names.Add(sortNameResults[i].FindElement(By.TagName("td")).Text);
            }
            List<string> orderedName = names.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedName, names);
        }

        [TestMethod]
        public void ProductSortNameDesc()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var nameHeading = table.FindElement(By.LinkText("Name"));
            nameHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            nameHeading = table.FindElement(By.LinkText("Name"));
            nameHeading.Click();
            var sortedName = _driver.FindElement(By.ClassName("table"));
            var sortNameResults = sortedName.FindElements(By.TagName("tr"));
            List<string> names = new List<string>();
            for (int i = 1; i < sortNameResults.Count; i++)
            {
                names.Add(sortNameResults[i].FindElement(By.TagName("td")).Text);
            }
            List<string> orderedName = names.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedName, names);
        }

        [TestMethod]
        public void ProductSortDescriptionAsc()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var descriptionHeading = table.FindElement(By.LinkText("Description"));
            descriptionHeading.Click();
            var sortedDescription = _driver.FindElement(By.ClassName("table"));
            var sortDescriptionResults = sortedDescription.FindElements(By.TagName("tr"));
            List<string> descriptions = new List<string>();
            for (int i = 1; i < sortDescriptionResults.Count; i++)
            {
                descriptions.Add(sortDescriptionResults[i].FindElements(By.TagName("td"))[1].Text);
            }
            List<string> orderedDescription = descriptions.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedDescription, descriptions);
        }

        [TestMethod]
        public void ProductSortDescriptionDesc()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var descriptionHeading = table.FindElement(By.LinkText("Description"));
            descriptionHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            descriptionHeading = table.FindElement(By.LinkText("Description"));
            descriptionHeading.Click();
            var sortedDescription = _driver.FindElement(By.ClassName("table"));
            var sortDescriptionResults = sortedDescription.FindElements(By.TagName("tr"));
            List<string> descriptions = new List<string>();
            for (int i = 1; i < sortDescriptionResults.Count; i++)
            {
                descriptions.Add(sortDescriptionResults[i].FindElements(By.TagName("td"))[1].Text);
            }
            List<string> orderedName = descriptions.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedName, descriptions);
        }

        [TestMethod]
        public void ProductSortListPriceAsc()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var priceHeading = table.FindElement(By.LinkText("List Price"));
            priceHeading.Click();
            var sortedPrice = _driver.FindElement(By.ClassName("table"));
            var sortPriceResults = sortedPrice.FindElements(By.TagName("tr"));
            List<decimal> prices = new List<decimal>();
            for (int i = 1; i < sortPriceResults.Count; i++)
            {
                prices.Add(decimal.Parse( sortPriceResults[i].FindElements(By.TagName("td"))[2].Text));
            }
            List<decimal> orderedPrices = prices.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedPrices, prices);
        }

        [TestMethod]
        public void ProductSortListPriceDesc()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var pricesHeading = table.FindElement(By.LinkText("List Price"));
            pricesHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            pricesHeading = table.FindElement(By.LinkText("List Price"));
            pricesHeading.Click();
            var sortedPrice = _driver.FindElement(By.ClassName("table"));
            var sortPriceResults = sortedPrice.FindElements(By.TagName("tr"));
            List<decimal> prices = new List<decimal>();
            for (int i = 1; i < sortPriceResults.Count; i++)
            {
                prices.Add(decimal.Parse(sortPriceResults[i].FindElements(By.TagName("td"))[2].Text));
            }
            List<decimal> orderedPrices = prices.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedPrices, prices);
        }

        [TestMethod]
        public void ProductSortStockAsc()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var priceHeading = table.FindElement(By.LinkText("Stock"));
            priceHeading.Click();
            var sortedStock = _driver.FindElement(By.ClassName("table"));
            var sortStockResults = sortedStock.FindElements(By.TagName("tr"));
            List<int> stock = new List<int>();
            for (int i = 1; i < sortStockResults.Count; i++)
            {
                stock.Add(int.Parse(sortStockResults[i].FindElements(By.TagName("td"))[3].Text));
            }
            List<int> orderedStock = stock.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedStock, stock);
        }

        [TestMethod]
        public void ProductSortStockDesc()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var stockHeading = table.FindElement(By.LinkText("Stock"));
            stockHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            stockHeading = table.FindElement(By.LinkText("Stock"));
            stockHeading.Click();
            var sortedStock = _driver.FindElement(By.ClassName("table"));
            var sortStockResults = sortedStock.FindElements(By.TagName("tr"));
            List<int> stock = new List<int>();
            for (int i = 1; i < sortStockResults.Count; i++)
            {
                stock.Add(int.Parse(sortStockResults[i].FindElements(By.TagName("td"))[3].Text));
            }
            List<int> orderedStock = stock.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedStock, stock);
        }

        [TestMethod]
        public void ProductCreateNewPage()
        {
            Login();
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            string createURL = "https://localhost:44314/Products/Create";
            Assert.AreEqual(createURL, _driver.Url);
        }

        [TestMethod]
        public void ProductCreateNewPageBacktoList()
        {
            Login();
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            var backLNK = _driver.FindElement(By.LinkText("Back to List"));
            backLNK.Click();
            string productIndexURL = "https://localhost:44314/Products/Index";
            Assert.AreEqual(productIndexURL, _driver.Url);
        }

        [TestMethod]
        public void ProductCreateNewProduct()
        {
            Login();
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            string name = "6' B8 BRONZE TRIANGLE";
            string description = "SABIAN 6 INCH BRONZE TRIANGLE";
            decimal price = (decimal) 109.00;
            int stock = 5;

            var nameTXT = _driver.FindElement(By.Name("Name"));
            var descriptionTXT = _driver.FindElement(By.Name("Description"));
            var listPriceTXT = _driver.FindElement(By.Name("ListPrice"));
            var stockTXT = _driver.FindElement(By.Name("Stock"));
            var createBTN = _driver.FindElement(By.Name("create"));

            nameTXT.SendKeys(name);
            descriptionTXT.SendKeys(description);
            listPriceTXT.SendKeys(price.ToString());
            stockTXT.SendKeys(stock.ToString());
            createBTN.Click();
            Thread.Sleep(3000);
            string productIndexURL = "https://localhost:44314/Products/Index";
            Assert.AreEqual(productIndexURL, _driver.Url);
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(name, rows[rows.Count - 1].FindElements(By.TagName("td"))[0].Text);
            Assert.AreEqual(description, rows[rows.Count - 1].FindElements(By.TagName("td"))[1].Text);
            Assert.AreEqual(price, decimal.Parse(rows[rows.Count - 1].FindElements(By.TagName("td"))[2].Text));
            Assert.AreEqual(stock, int.Parse(rows[rows.Count - 1].FindElements(By.TagName("td"))[3].Text));
        }

        [TestMethod]
        public void ProductCreateNewProductBlank()
        {
            Login();
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            var createBTN = _driver.FindElement(By.Name("create"));
            createBTN.Click();
            string createURL = "https://localhost:44314/Products/Create";
            Assert.AreEqual(createURL, _driver.Url);
        }

        [TestMethod]
        public void ProductDetailsPage()
        {
            Login();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var detailsLNK = rows[1].FindElement(By.LinkText("Details"));
            detailsLNK.Click();
            string detailsUrl = "https://localhost:44314/Products/Details";
            Assert.IsTrue(_driver.Url.Contains(detailsUrl));
        }

        [TestMethod]
        public void ProductEditPage()
        {
            ProductCreateNewProduct();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var editBTN = rows[rows.Count-1].FindElement(By.LinkText("Edit"));
            editBTN.Click();
            string editURL = "https://localhost:44314/Products/Edit";
            Assert.IsTrue(_driver.Url.Contains(editURL));
        }

        [TestMethod]
        public void ProductEditPageBacktoList()
        {
            ProductEditPage();
            var backLNK = _driver.FindElement(By.LinkText("Back to List"));
            backLNK.Click();
            string productIndexURL = "https://localhost:44314/Products/Index";
            Assert.AreEqual(productIndexURL, _driver.Url);
        }

        [TestMethod]
        public void ProductEditProduct()
        {
            ProductEditPage();
            string name = "7' B8 GOLD TRIANGLE";
            string description = "SABIAN 7 INCH GOLD TRIANGLE";
            decimal price = (decimal)209.00;
            int stock = 5;

            var nameTXT = _driver.FindElement(By.Name("Name"));
            var descriptionTXT = _driver.FindElement(By.Name("Description"));
            var listPriceTXT = _driver.FindElement(By.Name("ListPrice"));
            var stockTXT = _driver.FindElement(By.Name("Stock"));
            var saveBTN = _driver.FindElement(By.Name("save"));

            nameTXT.Clear();
            descriptionTXT.Clear();
            listPriceTXT.Clear();
            stockTXT.Clear();

            nameTXT.SendKeys(name);
            descriptionTXT.SendKeys(description);
            listPriceTXT.SendKeys(price.ToString());
            stockTXT.SendKeys(stock.ToString());
            saveBTN.Click();
            Thread.Sleep(3000);
            string productIndexURL = "https://localhost:44314/Products/Index";
            Assert.AreEqual(productIndexURL, _driver.Url);
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(name, rows[rows.Count - 1].FindElements(By.TagName("td"))[0].Text);
            Assert.AreEqual(description, rows[rows.Count - 1].FindElements(By.TagName("td"))[1].Text);
            Assert.AreEqual(price, decimal.Parse(rows[rows.Count - 1].FindElements(By.TagName("td"))[2].Text));
            Assert.AreEqual(stock, int.Parse(rows[rows.Count - 1].FindElements(By.TagName("td"))[3].Text));
        }

        [TestMethod]
        public void ProductEditProductNoChange()
        {
            Login();
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            string name = "6' B8 BRONZE TRIANGLE";
            string description = "SABIAN 6 INCH BRONZE TRIANGLE";
            decimal price = (decimal)109.00;
            int stock = 5;

            var nameTXT = _driver.FindElement(By.Name("Name"));
            var descriptionTXT = _driver.FindElement(By.Name("Description"));
            var listPriceTXT = _driver.FindElement(By.Name("ListPrice"));
            var stockTXT = _driver.FindElement(By.Name("Stock"));
            var createBTN = _driver.FindElement(By.Name("create"));

            nameTXT.SendKeys(name);
            descriptionTXT.SendKeys(description);
            listPriceTXT.SendKeys(price.ToString());
            stockTXT.SendKeys(stock.ToString());
            createBTN.Click();
            Thread.Sleep(3000);
            string productIndexURL = "https://localhost:44314/Products/Index";
            Assert.AreEqual(productIndexURL, _driver.Url);
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(name, rows[rows.Count - 1].FindElements(By.TagName("td"))[0].Text);
            Assert.AreEqual(description, rows[rows.Count - 1].FindElements(By.TagName("td"))[1].Text);
            Assert.AreEqual(price, decimal.Parse(rows[rows.Count - 1].FindElements(By.TagName("td"))[2].Text));
            Assert.AreEqual(stock, int.Parse(rows[rows.Count - 1].FindElements(By.TagName("td"))[3].Text));

            table = _driver.FindElement(By.ClassName("table"));
            rows = table.FindElements(By.TagName("tr"));
            var editBTN = rows[rows.Count - 1].FindElement(By.LinkText("Edit"));
            editBTN.Click();
            var saveBTN = _driver.FindElement(By.Name("save"));
            saveBTN.Click();
            table = _driver.FindElement(By.ClassName("table"));
            rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(name, rows[rows.Count - 1].FindElements(By.TagName("td"))[0].Text);
            Assert.AreEqual(description, rows[rows.Count - 1].FindElements(By.TagName("td"))[1].Text);
            Assert.AreEqual(price, decimal.Parse(rows[rows.Count - 1].FindElements(By.TagName("td"))[2].Text));
            Assert.AreEqual(stock, int.Parse(rows[rows.Count - 1].FindElements(By.TagName("td"))[3].Text));
        }

        [TestMethod]
        public void ProductDeletePage()
        {
            ProductCreateNewProduct();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var deleteBTN = rows[rows.Count - 1].FindElement(By.LinkText("Delete"));
            deleteBTN.Click();
            string deleteURl = "https://localhost:44314/Products/Delete";
            Assert.AreEqual(deleteURl, _driver.Url);
        }

        [TestMethod]
        public void ProductDeletePageBacktoList()
        {
            ProductDeletePage();
            var backLNK = _driver.FindElement(By.LinkText("Back to List"));
            backLNK.Click();
            string productIndexURL = "https://localhost:44314/Products/Index";
            Assert.AreEqual(productIndexURL, _driver.Url);
        }

        [TestMethod]
        public void ProductDeleteProduct()
        {
            ProductCreateNewProduct();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            int initRows = rows.Count;
            var deleteLNK = rows[rows.Count - 1].FindElement(By.LinkText("Delete"));
            deleteLNK.Click();
            var deleteBTN = _driver.FindElement(By.Name("delete"));
            table = _driver.FindElement(By.ClassName("table"));
            rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(initRows - 1, rows.Count);
        }

        [TestMethod]
        public void SupplierIndex()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.IsNotNull(table);
            Assert.IsNotNull(rows);
        }

        [TestMethod]
        public void SupplierSearch()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var searchTXT = _driver.FindElement(By.Name("SearchString"));
            var searchBTN = _driver.FindElement(By.Name("search"));
            string searchString = "audio";
            searchTXT.SendKeys(searchString);
            searchBTN.Click();
            var searchTable = _driver.FindElement(By.ClassName("table"));
            var searchResults = searchTable.FindElements(By.TagName("tr"));
            List<string> results = new List<string>();
            foreach (var row in searchResults)
            {
                results.Add(row.Text.ToLower());
            }
            results.RemoveAt(0);
            foreach (var row in results)
            {
                Assert.IsTrue(row.Contains(searchString));
            }
        }

        [TestMethod]
        public void SupplierSortCompanyAsc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var heading = table.FindElement(By.LinkText("Company"));
            heading.Click();
            var sortedCompany = _driver.FindElement(By.ClassName("table"));
            var sortCompanyResults = sortedCompany.FindElements(By.TagName("tr"));
            List<string> company = new List<string>();
            for (int i = 1; i < sortCompanyResults.Count; i++)
            {
                company.Add(sortCompanyResults[i].FindElement(By.TagName("td")).Text);
            }
            List<string> orderedCompany = company.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedCompany, company);
        }

        [TestMethod]
        public void SupplierSortCompanyDesc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var CompanyHeading = table.FindElement(By.LinkText("Company"));
            CompanyHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            CompanyHeading = table.FindElement(By.LinkText("Company"));
            CompanyHeading.Click();
            var sortedCompany = _driver.FindElement(By.ClassName("table"));
            var sortCompanyResults = sortedCompany.FindElements(By.TagName("tr"));
            List<string> company = new List<string>();
            for (int i = 1; i < sortCompanyResults.Count; i++)
            {
                company.Add(sortCompanyResults[i].FindElement(By.TagName("td")).Text);
            }
            List<string> orderedCompany = company.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedCompany, company);
        }

        [TestMethod]
        public void SupplierSortContactNameAsc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var contactHeading = table.FindElement(By.LinkText("Contact Name"));
            contactHeading.Click();
            var sortedContact = _driver.FindElement(By.ClassName("table"));
            var sortContactResults = sortedContact.FindElements(By.TagName("tr"));
            List<string> contact = new List<string>();
            for (int i = 1; i < sortContactResults.Count; i++)
            {
                contact.Add(sortContactResults[i].FindElements(By.TagName("td"))[1].Text);
            }
            List<string> orderedContact = contact.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedContact, contact);
        }

        [TestMethod]
        public void SupplierSortContactNameDesc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var contactHeading = table.FindElement(By.LinkText("Contact Name"));
            contactHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            contactHeading = table.FindElement(By.LinkText("Contact Name"));
            contactHeading.Click();
            var sortedContact = _driver.FindElement(By.ClassName("table"));
            var sortContactResults = sortedContact.FindElements(By.TagName("tr"));
            List<string> contacts = new List<string>();
            for (int i = 1; i < sortContactResults.Count; i++)
            {
                contacts.Add(sortContactResults[i].FindElements(By.TagName("td"))[1].Text);
            }
            List<string> orderedName = contacts.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedName, contacts);
        }

        [TestMethod]
        public void SupplierSortEmailAsc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var emailHeading = table.FindElement(By.LinkText("Email"));
            emailHeading.Click();
            var sortedEmail = _driver.FindElement(By.ClassName("table"));
            var sortEmailResults = sortedEmail.FindElements(By.TagName("tr"));
            List<string> emails = new List<string>();
            for (int i = 1; i < sortEmailResults.Count; i++)
            {
                emails.Add(sortEmailResults[i].FindElements(By.TagName("td"))[2].Text);
            }
            List<string> orderedEmail = emails.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedEmail, emails);
        }

        [TestMethod]
        public void SupplierSortEmailDesc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var emailHeading = table.FindElement(By.LinkText("Email"));
            emailHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            emailHeading = table.FindElement(By.LinkText("Email"));
            emailHeading.Click();
            var sortedEmail = _driver.FindElement(By.ClassName("table"));
            var sortEmailResults = sortedEmail.FindElements(By.TagName("tr"));
            List<string> emails = new List<string>();
            for (int i = 1; i < sortEmailResults.Count; i++)
            {
                emails.Add(sortEmailResults[i].FindElements(By.TagName("td"))[2].Text);
            }
            List<string> orderedEmails = emails.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedEmails, emails);
        }

        [TestMethod]
        public void SupplierSortPhoneAsc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var phoneHeading = table.FindElement(By.LinkText("Phone"));
            phoneHeading.Click();
            var sortedPhone = _driver.FindElement(By.ClassName("table"));
            var sortPhoneResults = sortedPhone.FindElements(By.TagName("tr"));
            List<string> phone = new List<string>();
            for (int i = 1; i < sortPhoneResults.Count; i++)
            {
                phone.Add(sortPhoneResults[i].FindElements(By.TagName("td"))[3].Text);
            }
            List<string> orderedPhone = phone.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedPhone, phone);
        }

        [TestMethod]
        public void SupplierSortPhoneDesc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var phoneHeading = table.FindElement(By.LinkText("Phone"));
            phoneHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            phoneHeading = table.FindElement(By.LinkText("Phone"));
            phoneHeading.Click();
            var sortedPhone = _driver.FindElement(By.ClassName("table"));
            var sortPhoneResults = sortedPhone.FindElements(By.TagName("tr"));
            List<string> phone = new List<string>();
            for (int i = 1; i < sortPhoneResults.Count; i++)
            {
                phone.Add(sortPhoneResults[i].FindElements(By.TagName("td"))[3].Text);
            }
            List<string> orderedPhone = phone.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedPhone, phone);
        }

        [TestMethod]
        public void SupplierCreateNewPage()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            string createURL = "https://localhost:44314/Suppliers/Create";
            Assert.AreEqual(createURL, _driver.Url);
        }

        [TestMethod]
        public void SupplierCreateNewPageBacktoList()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            var backLNK = _driver.FindElement(By.LinkText("Back to List"));
            backLNK.Click();
            string supplierIndexURL = "https://localhost:44314/Suppliers/Index";
            Assert.AreEqual(supplierIndexURL, _driver.Url);
        }

        [TestMethod]
        public void SupplierCreateNewSupplier()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            string company = "Harmony Haven Music Emporium";
            string contactName = "John Smith";
            string email = "info@harmonyhavenmusic.com";
            string phone = "027-555-1234";
            string address = "123 Melody Lane";
            string city = "Crescendo City";
            int postcode = 5570;
            string country = "New Zealand";

            var companyTXT = _driver.FindElement(By.Name("Company"));
            var contactNameTXT = _driver.FindElement(By.Name("ContactName"));
            var emailTXT = _driver.FindElement(By.Name("Email"));
            var phoneTXT = _driver.FindElement(By.Name("Phone"));
            var addressTXT = _driver.FindElement(By.Name("Address"));
            var cityTXT = _driver.FindElement(By.Name("City"));
            var postcodeTXT = _driver.FindElement(By.Name("Postcode"));
            var countryTXT = _driver.FindElement(By.Name("Country"));
            var createBTN = _driver.FindElement(By.Name("create"));

            companyTXT.SendKeys(company);
            contactNameTXT.SendKeys(contactName);
            emailTXT.SendKeys(email);
            phoneTXT.SendKeys(phone);
            addressTXT.SendKeys(address);
            cityTXT.SendKeys(city);
            postcodeTXT.SendKeys(postcode.ToString());
            countryTXT.SendKeys(country);
            createBTN.Click();

            Thread.Sleep(3000);
            string supplierIndexURL = "https://localhost:44314/Suppliers/Index";
            Assert.AreEqual(supplierIndexURL, _driver.Url);
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(company, rows[rows.Count - 1].FindElements(By.TagName("td"))[0].Text);
            Assert.AreEqual(contactName, rows[rows.Count - 1].FindElements(By.TagName("td"))[1].Text);
            Assert.AreEqual(email, rows[rows.Count - 1].FindElements(By.TagName("td"))[2].Text);
            Assert.AreEqual(phone, rows[rows.Count - 1].FindElements(By.TagName("td"))[3].Text);
        }

        [TestMethod]
        public void SupplierCreateNewSupplierBlank()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            var createBTN = _driver.FindElement(By.Name("create"));
            createBTN.Click();
            string createURL = "https://localhost:44314/Suppliers/Create";
            Assert.AreEqual(createURL, _driver.Url);
        }

        [TestMethod]
        public void SupplierDetailsPage()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var detailsLNK = rows[1].FindElement(By.LinkText("Details"));
            detailsLNK.Click();
            string detailsUrl = "https://localhost:44314/Suppliers/Details";
            Assert.IsTrue(_driver.Url.Contains(detailsUrl));
        }

        [TestMethod]
        public void SupplierEditPage()
        {
            SupplierCreateNewSupplier();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var editBTN = rows[rows.Count - 1].FindElement(By.LinkText("Edit"));
            editBTN.Click();
            string editURL = "https://localhost:44314/Suppliers/Edit";
            Assert.IsTrue(_driver.Url.Contains(editURL));
        }

        [TestMethod]
        public void SupplierEditPageBacktoList()
        {
            SupplierEditPage();
            var backLNK = _driver.FindElement(By.LinkText("Back to List"));
            backLNK.Click();
            string supplierIndexURL = "https://localhost:44314/Suppliers/Index";
            Assert.AreEqual(supplierIndexURL, _driver.Url);
        }

        [TestMethod]
        public void SupplierEditProduct()
        {
            SupplierEditPage();
            string company = "Harmony Haven Music Emporium";
            string contactName = "Cassidy Smith";
            string email = "info@harmonyhavenmusic.com";
            string phone = "027-555-9999";
            string address = "123 Melody Road";
            string city = "Crescendo Town";
            int postcode = 5570;
            string country = "New Zealand";

            var companyTXT = _driver.FindElement(By.Name("Company"));
            var contactNameTXT = _driver.FindElement(By.Name("ContactName"));
            var emailTXT = _driver.FindElement(By.Name("Email"));
            var phoneTXT = _driver.FindElement(By.Name("Phone"));
            var addressTXT = _driver.FindElement(By.Name("Address"));
            var cityTXT = _driver.FindElement(By.Name("City"));
            var postcodeTXT = _driver.FindElement(By.Name("Postcode"));
            var countryTXT = _driver.FindElement(By.Name("Country"));
            var saveBTN = _driver.FindElement(By.Name("save"));

            companyTXT.Clear();
            contactNameTXT.Clear();
            emailTXT.Clear();
            phoneTXT.Clear();
            addressTXT.Clear();
            cityTXT.Clear();
            postcodeTXT.Clear();
            countryTXT.Clear();

            companyTXT.SendKeys(company);
            contactNameTXT.SendKeys(contactName);
            emailTXT.SendKeys(email);
            phoneTXT.SendKeys(phone);
            addressTXT.SendKeys(address);
            cityTXT.SendKeys(city);
            postcodeTXT.SendKeys(postcode.ToString());
            countryTXT.SendKeys(country);
            saveBTN.Click();

            Thread.Sleep(3000);
            string supplierIndexURL = "https://localhost:44314/Suppliers/Index";
            Assert.AreEqual(supplierIndexURL, _driver.Url);
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(company, rows[rows.Count - 1].FindElements(By.TagName("td"))[0].Text);
            Assert.AreEqual(contactName, rows[rows.Count - 1].FindElements(By.TagName("td"))[1].Text);
            Assert.AreEqual(email, rows[rows.Count - 1].FindElements(By.TagName("td"))[2].Text);
            Assert.AreEqual(phone, rows[rows.Count - 1].FindElements(By.TagName("td"))[3].Text);
        }

        [TestMethod]
        public void SupplierEditProductNoChange()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/Suppliers/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            string initRow = rows[rows.Count - 1].Text;
            var editBTN = rows[rows.Count - 1].FindElement(By.LinkText("Edit"));
            editBTN.Click();
            var saveBTN = _driver.FindElement(By.Name("save"));
            saveBTN.Click();
            table = _driver.FindElement(By.ClassName("table"));
            rows = table.FindElements(By.TagName("tr"));
            string editedRow = rows[rows.Count - 1].Text;
            Assert.AreEqual(initRow, editedRow);
        }

        [TestMethod]
        public void SupplierDeletePage()
        {
            SupplierCreateNewSupplier();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var deleteLNK = rows[rows.Count - 1].FindElement(By.LinkText("Delete"));
            deleteLNK.Click();
            string deleteURl = "https://localhost:44314/Suppliers/Delete";
            Assert.IsTrue(_driver.Url.Contains(deleteURl));
        }

        [TestMethod]
        public void SupplierDeletePageBacktoList()
        {
            SupplierDeletePage();
            var backLNK = _driver.FindElement(By.LinkText("Back to List"));
            backLNK.Click();
            string productIndexURL = "https://localhost:44314/Suppliers/Index";
            Assert.AreEqual(productIndexURL, _driver.Url);
        }

        [TestMethod]
        public void SupplierDeleteProduct()
        {
            SupplierCreateNewSupplier();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            int initRows = rows.Count;
            var deleteLNK = rows[rows.Count - 1].FindElement(By.LinkText("Delete"));
            deleteLNK.Click();
            var deleteBTN = _driver.FindElement(By.Name("delete"));
            deleteBTN.Click();
            table = _driver.FindElement(By.ClassName("table"));
            rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(initRows - 1, rows.Count);
        }

        [TestMethod]
        public void SupplierProductsIndex()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.IsNotNull(table);
            Assert.IsNotNull(rows);
        }

        [TestMethod]
        public void SupplierProductsSearch()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var searchTXT = _driver.FindElement(By.Name("SearchString"));
            var searchBTN = _driver.FindElement(By.Name("search"));
            string searchString = "guitar";
            searchTXT.SendKeys(searchString);
            searchBTN.Click();
            var searchTable = _driver.FindElement(By.ClassName("table"));
            var searchResults = searchTable.FindElements(By.TagName("tr"));
            List<string> results = new List<string>();
            foreach (var row in searchResults)
            {
                results.Add(row.Text.ToLower());
            }
            results.RemoveAt(0);
            foreach (var row in results)
            {
                Assert.IsTrue(row.Contains(searchString));
            }
        }

        [TestMethod]
        public void SupplierProductsSortNameAsc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var nameheading = table.FindElement(By.LinkText("Name"));
            nameheading.Click();
            var sortedName = _driver.FindElement(By.ClassName("table"));
            var sortNameResults = sortedName.FindElements(By.TagName("tr"));
            List<string> name = new List<string>();
            for (int i = 1; i < sortNameResults.Count; i++)
            {
                name.Add(sortNameResults[i].FindElement(By.TagName("td")).Text);
            }
            List<string> orderedName = name.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedName, name);
        }

        [TestMethod]
        public void SupplierProductsSortNameDesc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var nameHeading = table.FindElement(By.LinkText("Name"));
            nameHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            nameHeading = table.FindElement(By.LinkText("Name"));
            nameHeading.Click();
            var sortedName = _driver.FindElement(By.ClassName("table"));
            var sortNameResults = sortedName.FindElements(By.TagName("tr"));
            List<string> name = new List<string>();
            for (int i = 1; i < sortNameResults.Count; i++)
            {
                name.Add(sortNameResults[i].FindElement(By.TagName("td")).Text);
            }
            List<string> orderedName = name.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedName, name);
        }

        [TestMethod]
        public void SupplierProductsSortCompanyAsc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var companyHeading = table.FindElement(By.LinkText("Company"));
            companyHeading.Click();
            var sortedCompany = _driver.FindElement(By.ClassName("table"));
            var sortCompanyResults = sortedCompany.FindElements(By.TagName("tr"));
            List<string> company = new List<string>();
            for (int i = 1; i < sortCompanyResults.Count; i++)
            {
                company.Add(sortCompanyResults[i].FindElements(By.TagName("td"))[1].Text);
            }
            List<string> orderedCompany = company.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedCompany, company);
        }

        [TestMethod]
        public void SupplierProductsSortCompanyDesc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var companyHeading = table.FindElement(By.LinkText("Company"));
            companyHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            companyHeading = table.FindElement(By.LinkText("Company"));
            companyHeading.Click();
            var sortedCompany = _driver.FindElement(By.ClassName("table"));
            var sortCompanyResults = sortedCompany.FindElements(By.TagName("tr"));
            List<string> company = new List<string>();
            for (int i = 1; i < sortCompanyResults.Count; i++)
            {
                company.Add(sortCompanyResults[i].FindElements(By.TagName("td"))[1].Text);
            }
            List<string> orderedCompany = company.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedCompany, company);
        }

        [TestMethod]
        public void SupplierProductsSortCostAsc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var costHeading = table.FindElement(By.LinkText("Cost"));
            costHeading.Click();
            var sortedCost = _driver.FindElement(By.ClassName("table"));
            var sortCostResults = sortedCost.FindElements(By.TagName("tr"));
            List<decimal> costs = new List<decimal>();
            for (int i = 1; i < sortCostResults.Count; i++)
            {
                costs.Add(decimal.Parse(sortCostResults[i].FindElements(By.TagName("td"))[2].Text));
            }
            List<decimal> orderedCosts = costs.OrderBy(x => x).ToList();
            CollectionAssert.AreEqual(orderedCosts, costs);
        }

        [TestMethod]
        public void SupplierProductsSortCostDesc()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var costHeading = table.FindElement(By.LinkText("Cost"));
            costHeading.Click();
            table = _driver.FindElement(By.ClassName("table"));
            costHeading = table.FindElement(By.LinkText("Cost"));
            costHeading.Click();
            var sortedCost = _driver.FindElement(By.ClassName("table"));
            var sortCostResults = sortedCost.FindElements(By.TagName("tr"));
            List<decimal> costs = new List<decimal>();
            for (int i = 1; i < sortCostResults.Count; i++)
            {
                costs.Add(decimal.Parse(sortCostResults[i].FindElements(By.TagName("td"))[2].Text));
            }
            List<decimal> orderedCost = costs.OrderByDescending(x => x).ToList();
            CollectionAssert.AreEqual(orderedCost, costs);
        }

        [TestMethod]
        public void SupplierProductsCreateNewPage()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            string createURL = "https://localhost:44314/SupplierProducts/Create";
            Assert.AreEqual(createURL, _driver.Url);
        }

        [TestMethod]
        public void SupplierProductsCreateNewPageBacktoList()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            var backLNK = _driver.FindElement(By.LinkText("Back to List"));
            backLNK.Click();
            string supplierProductsIndexURL = "https://localhost:44314/SupplierProducts/Index";
            Assert.AreEqual(supplierProductsIndexURL, _driver.Url);
        }

        [TestMethod]
        public void SupplierProductsCreateNew()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var createNewBTN = _driver.FindElement(By.Name("createNew"));
            createNewBTN.Click();
            var sidList = _driver.FindElement(By.Name("Sid"));
            var pidList = _driver.FindElement(By.Name("Pid"));
            var costTXT = _driver.FindElement(By.Name("Cost"));
            var createBTN = _driver.FindElement(By.Name("create"));

            sidList.Click(); sidList.SendKeys(Keys.Enter);
            pidList.Click(); pidList.SendKeys(Keys.Enter);
            costTXT.SendKeys("999.99");

            string[] supplier = sidList.FindElement(By.TagName("option")).Text.Split('-');
            string[] product = pidList.FindElement(By.TagName("option")).Text.Split('-');

            createBTN.Click();

            Thread.Sleep(3000);
            string supplierIndexURL = "https://localhost:44314/SupplierProducts/Index";
            Assert.AreEqual(supplierIndexURL, _driver.Url);
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(product[1].Trim(), rows[rows.Count - 1].FindElements(By.TagName("td"))[0].Text);
            Assert.AreEqual(supplier[1].Trim(), rows[rows.Count - 1].FindElements(By.TagName("td"))[1].Text);
            Assert.AreEqual("999.99", rows[rows.Count - 1].FindElements(By.TagName("td"))[2].Text);
        }

        [TestMethod]
        public void SupplierProductsDetailsPage()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var detailsLNK = rows[1].FindElement(By.LinkText("Details"));
            detailsLNK.Click();
            string detailsUrl = "https://localhost:44314/SupplierProducts/Details";
            Assert.IsTrue(_driver.Url.Contains(detailsUrl));
        }

        [TestMethod]
        public void SupplierProductsEditPage()
        {
            SupplierProductsCreateNew();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var editBTN = rows[rows.Count - 1].FindElement(By.LinkText("Edit"));
            editBTN.Click();
            string editURL = "https://localhost:44314/SupplierProducts/Edit";
            Assert.IsTrue(_driver.Url.Contains(editURL));
        }

        [TestMethod]
        public void SupplierProductsEditPageBacktoList()
        {
            SupplierProductsEditPage();
            var backLNK = _driver.FindElement(By.LinkText("Back to List"));
            backLNK.Click();
            string supplierProductsIndexURL = "https://localhost:44314/SupplierProducts/Index";
            Assert.AreEqual(supplierProductsIndexURL, _driver.Url);
        }

        [TestMethod]
        public void SupplierProductsEditProduct()
        {
            SupplierProductsEditPage();
            var sidList = _driver.FindElement(By.Name("Sid"));
            var pidList = _driver.FindElement(By.Name("Pid"));
            var costTXT = _driver.FindElement(By.Name("Cost"));
            var saveBTN = _driver.FindElement(By.Name("save"));

            costTXT.Clear();
            costTXT.SendKeys("99.99");
            string[] supplier = sidList.FindElement(By.TagName("option")).Text.Split('-');
            string[] product = pidList.FindElement(By.TagName("option")).Text.Split('-');
            saveBTN.Click();

            Thread.Sleep(3000);
            string supplierProductsIndexURL = "https://localhost:44314/SupplierProducts/Index";
            Assert.AreEqual(supplierProductsIndexURL, _driver.Url);
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(product[1].Trim(), rows[rows.Count - 1].FindElements(By.TagName("td"))[0].Text);
            Assert.AreEqual(supplier[1].Trim(), rows[rows.Count - 1].FindElements(By.TagName("td"))[1].Text);
            Assert.AreEqual("99.99", rows[rows.Count - 1].FindElements(By.TagName("td"))[2].Text);
        }

        [TestMethod]
        public void SupplierProductsEditProductNoChange()
        {
            Login();
            _driver.Navigate().GoToUrl("https://localhost:44314/SupplierProducts/Index");
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            string initRow = rows[rows.Count - 1].Text;
            var editBTN = rows[rows.Count - 1].FindElement(By.LinkText("Edit"));
            editBTN.Click();
            var saveBTN = _driver.FindElement(By.Name("save"));
            saveBTN.Click();
            table = _driver.FindElement(By.ClassName("table"));
            rows = table.FindElements(By.TagName("tr"));
            string editedRow = rows[rows.Count - 1].Text;
            Assert.AreEqual(initRow, editedRow);
        }

        [TestMethod]
        public void SupplierProductsDeletePage()
        {
            SupplierProductsCreateNew();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            var deleteLNK = rows[rows.Count - 1].FindElement(By.LinkText("Delete"));
            deleteLNK.Click();
            string deleteURl = "https://localhost:44314/SupplierProducts/Delete";
            Assert.IsTrue(_driver.Url.Contains(deleteURl));
        }

        [TestMethod]
        public void SupplierProductsDeletePageBacktoList()
        {
            SupplierProductsDeletePage();
            var backLNK = _driver.FindElement(By.LinkText("Back to List"));
            backLNK.Click();
            string productIndexURL = "https://localhost:44314/SupplierProducts/Index";
            Assert.AreEqual(productIndexURL, _driver.Url);
        }

        [TestMethod]
        public void SupplierProductsDeleteProduct()
        {
            SupplierProductsCreateNew();
            var table = _driver.FindElement(By.ClassName("table"));
            var rows = table.FindElements(By.TagName("tr"));
            int initRows = rows.Count;
            var deleteLNK = rows[rows.Count - 1].FindElement(By.LinkText("Delete"));
            deleteLNK.Click();
            var deleteBTN = _driver.FindElement(By.Name("delete"));
            deleteBTN.Click();
            table = _driver.FindElement(By.ClassName("table"));
            rows = table.FindElements(By.TagName("tr"));
            Assert.AreEqual(initRows - 1, rows.Count);
        }
    }
}
