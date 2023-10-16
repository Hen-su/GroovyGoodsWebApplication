using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GroovyGoodsWebApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly GroovyGoodsContext _db;

        public LoginController(ILogger<LoginController> logger, GroovyGoodsContext db)
        {
            _logger = logger;
            _db = db;
        }

        //Load webpage and redirects if user is already authenticated
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Products");
            }
            return View();
        }

        //hash a string using sha256 and convert to base64 string
        private string HashPassword(string password)
        {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    string hashString = Convert.ToBase64String(hashValue);
                    return hashString;
                }
        }

        //login user with credentials and redirect to landing page
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
                foreach(Administrator administrator in _db.Administrators)
                {
                    if (username == administrator.Username)
                    {
                        password = password + administrator.Salt;
                        string hashString = HashPassword(password);
                            if (hashString == administrator.Hash)
                            {
                                List<Claim> claims = new List<Claim>()
                                {
                                    new Claim(ClaimTypes.Name, username),
                                };
                                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
                                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                                HttpContext.SignInAsync(claimsPrincipal);
                                return RedirectToAction("Index", "Products");
                            }
                        
                    }
                }
            return View();
        }

        //logout user and redirect to login page
        [Authorize]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
