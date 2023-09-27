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
using Microsoft.IdentityModel.Tokens;

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

        private void PopulateLists()
        {
            //Get list of products with id and name
            var productsList = _context.Products.Select(p => new SelectListItem
            {
                Value = p.Pid.ToString(),
                Text = $"{p.Pid} - {p.Name}"
            }).ToList();
            ViewData["Pid"] = new SelectList(productsList, "Value", "Text");

            //Get list of suppliers with id and company name
            var suppliersList = _context.Suppliers.Select(s => new SelectListItem
            {
                Value = s.Sid.ToString(),
                Text = $"{s.Sid} - {s.Company}"
            }).ToList();
            ViewData["Sid"] = new SelectList(suppliersList, "Value", "Text");
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
            PopulateLists();
            return View();
        }

        // POST: SupplierProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Spid,Sid,Pid,Cost")] SupplierProduct supplierProduct)
        {
            var sidExists = from s in _context.Suppliers
                                where s.Sid == supplierProduct.Sid
                                select s;

            var pidExists = from p in _context.Products
                                where  p.Pid == supplierProduct.Pid
                                select p;

            if (ModelState.IsValid || (!sidExists.IsNullOrEmpty() && !pidExists.IsNullOrEmpty()))
            {
                _context.Add(supplierProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateLists();
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
            PopulateLists();
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

            var sidExists = from s in _context.Suppliers
                            where s.Sid == supplierProduct.Sid
                            select s;

            var pidExists = from p in _context.Products
                            where p.Pid == supplierProduct.Pid
                            select p;
                
            if (ModelState.IsValid || (!sidExists.IsNullOrEmpty() && !pidExists.IsNullOrEmpty()))
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
            PopulateLists();
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
