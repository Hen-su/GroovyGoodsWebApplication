using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroovyGoodsWebApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace GroovyGoodsWebApplication.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly GroovyGoodsContext _context;

        public ProductsController(GroovyGoodsContext context)
        {
            _context = context;
        }

        [HttpGet("Products/Index")]
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            // Sorting parameters
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DescriptionSortParm = sortOrder == "description" ? "description_desc" : "description";
            ViewBag.ListPriceSortParm = sortOrder == "listPrice" ? "listPrice_desc" : "listPrice";
            ViewBag.StockSortParm = sortOrder == "stock" ? "stock_desc" : "stock";

            // Get the list of products
            var products = from p in _context.Products
                           select p;

            // Filter by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p =>
                    p.Name.Contains(searchString) ||
                    p.Description.Contains(searchString));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "description":
                    products = products.OrderBy(p => p.Description);
                    break;
                case "listPrice":
                    products = products.OrderBy(p => p.ListPrice);
                    break;
                case "stock":
                    products = products.OrderBy(p => p.Stock);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "description_desc":
                    products = products.OrderByDescending(p => p.Description);
                    break;
                case "listPrice_desc":
                    products = products.OrderByDescending(p => p.ListPrice);
                    break;
                case "stock_desc":
                    products = products.OrderByDescending(p => p.Stock);
                    break;
                default:
                    break;
            }

            // Execute the query and return the sorted and filtered list of products
            return View(await products.ToListAsync());
        }



        // GET: Products
        public async Task<IActionResult> Index()
        {
              return _context.Products != null ? 
                          View(await _context.Products.ToListAsync()) :
                          Problem("Entity set 'GroovyGoodsContext.Products'  is null.");
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Pid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pid,Name,Description,ListPrice,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Pid,Name,Description,ListPrice,Stock")] Product product)
        {
            if (id != product.Pid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Pid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Pid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'GroovyGoodsContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Pid == id)).GetValueOrDefault();
        }
    }
}
