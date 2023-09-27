using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
                foreach(Administrator administrator in _db.Administrators)
                {
                    if (username == administrator.Username)
                    {
                        password = password + administrator.Salt;
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                            string hashString = Convert.ToBase64String(hashValue);
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
                }
            
            
            return View();
        }
        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
