using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroovyGoodsWebApplication.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace GroovyGoodsWebApplication.Controllers
{
    [Authorize]
    public class SupplierProductsController : Controller
    {
        private readonly GroovyGoodsContext _context;

        public SupplierProductsController(GroovyGoodsContext context)
        {
            _context = context;
        }

        [HttpGet("SupplierProducts/Index")]
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            // Sorting parameters
            ViewBag.CostSortParm = string.IsNullOrEmpty(sortOrder) ? "cost_desc" : "";

            // Get the list of supplier products with related Product and Supplier data
            var supplierProducts = from sp in _context.SupplierProducts
                                   .Include(sp => sp.PidNavigation) // Include Product
                                   .Include(sp => sp.SidNavigation) // Include Supplier
                                   select sp;

            // Filter by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                supplierProducts = supplierProducts.Where(sp =>
                    sp.PidNavigation.Name.Contains(searchString) || // Access Product.Name
                    sp.SidNavigation.Company.Contains(searchString) || // Access Supplier.Company
                    sp.Cost.ToString().Contains(searchString));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "cost":
                    supplierProducts = supplierProducts.OrderBy(sp => sp.Cost);
                    break;
                case "cost_desc":
                    supplierProducts = supplierProducts.OrderByDescending(sp => sp.Cost);
                    break;
                default:
                    break;
            }

            // Execute the query and return the sorted and filtered list of supplier products
            return View(await supplierProducts.ToListAsync());
        }



        // GET: SupplierProducts
        public async Task<IActionResult> Index()
        {
            var groovyGoodsContext = _context.SupplierProducts.Include(s => s.PidNavigation).Include(s => s.SidNavigation);
            return View(await groovyGoodsContext.ToListAsync());
        }

        // GET: SupplierProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SupplierProducts == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SupplierProducts
                .Include(s => s.PidNavigation)
                .Include(s => s.SidNavigation)
                .FirstOrDefaultAsync(m => m.Spid == id);
            if (supplierProduct == null)
            {
                return NotFound();
            }

            return View(supplierProduct);
        }

        // GET: SupplierProducts/Create
        public IActionResult Create()
        {
            ViewData["Pid"] = new SelectList(_context.Products, "Pid", "Name");
            ViewData["Sid"] = new SelectList(_context.Suppliers, "Sid", "Company");
            return View();
        }

        // POST: SupplierProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Spid,Sid,Pid,Cost")] SupplierProduct supplierProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplierProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Pid"] = new SelectList(_context.Products, "Pid", "Pid", supplierProduct.Pid);
            ViewData["Sid"] = new SelectList(_context.Suppliers, "Sid", "Sid", supplierProduct.Sid);
            return View(supplierProduct);
        }

        // GET: SupplierProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SupplierProducts == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SupplierProducts.FindAsync(id);
            if (supplierProduct == null)
            {
                return NotFound();
            }
            ViewData["Pid"] = new SelectList(_context.Products, "Pid", "Pid", supplierProduct.Pid);
            ViewData["Sid"] = new SelectList(_context.Suppliers, "Sid", "Sid", supplierProduct.Sid);
            return View(supplierProduct);
        }

        // POST: SupplierProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Spid,Sid,Pid,Cost")] SupplierProduct supplierProduct)
        {
            if (id != supplierProduct.Spid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplierProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierProductExists(supplierProduct.Spid))
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
            ViewData["Pid"] = new SelectList(_context.Products, "Pid", "name", supplierProduct.Pid);
            ViewData["Sid"] = new SelectList(_context.Suppliers, "Sid", "company", supplierProduct.Sid);
            return View(supplierProduct);
        }

        // GET: SupplierProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SupplierProducts == null)
            {
                return NotFound();
            }

            var supplierProduct = await _context.SupplierProducts
                .Include(s => s.PidNavigation)
                .Include(s => s.SidNavigation)
                .FirstOrDefaultAsync(m => m.Spid == id);
            if (supplierProduct == null)
            {
                return NotFound();
            }

            return View(supplierProduct);
        }

        // POST: SupplierProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SupplierProducts == null)
            {
                return Problem("Entity set 'GroovyGoodsContext.SupplierProducts'  is null.");
            }
            var supplierProduct = await _context.SupplierProducts.FindAsync(id);
            if (supplierProduct != null)
            {
                _context.SupplierProducts.Remove(supplierProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierProductExists(int id)
        {
          return (_context.SupplierProducts?.Any(e => e.Spid == id)).GetValueOrDefault();
        }
    }
}
