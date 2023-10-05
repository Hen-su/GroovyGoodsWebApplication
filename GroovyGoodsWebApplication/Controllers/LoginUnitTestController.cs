using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GroovyGoodsWebApplication.Controllers
{
    public class LoginUnitTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hashString = Convert.ToBase64String(hashValue);
                return hashString;
            }
        }
    }
}
