using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroovyGoodsWebApplication.Models;

namespace GroovyGoodsWebApplication.Controllers
{
    public class AdministratorsController : Controller
    {
        private readonly GroovyGoodsContext _context;

        public AdministratorsController(GroovyGoodsContext context)
        {
            _context = context;
        }

        // GET: Administrators
        public async Task<IActionResult> Index()
        {
              return _context.Administrators != null ? 
                          View(await _context.Administrators.ToListAsync()) :
                          Problem("Entity set 'GroovyGoodsContext.Administrators'  is null.");
        }

        private bool AdministratorExists(int id)
        {
          return (_context.Administrators?.Any(e => e.Aid == id)).GetValueOrDefault();
        }
    }
}
