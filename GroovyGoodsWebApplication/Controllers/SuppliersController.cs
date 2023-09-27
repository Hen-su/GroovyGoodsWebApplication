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
    public class SuppliersController : Controller
    {
        private readonly GroovyGoodsContext _context;

        public SuppliersController(GroovyGoodsContext context)
        {
            _context = context;
        }


        [HttpGet("Suppliers/Index")]
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            // Sorting parameters
            ViewBag.CompanySortParm = string.IsNullOrEmpty(sortOrder) ? "company_desc" : "";
            ViewBag.ContactNameSortParm = sortOrder == "contactName" ? "contactName_desc" : "contactName";
            ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
            ViewBag.PhoneSortParm = sortOrder == "phone" ? "phone_desc" : "phone";
            ViewBag.AddressSortParm = sortOrder == "address" ? "address_desc" : "address";
            ViewBag.CitySortParm = sortOrder == "city" ? "city_desc" : "city";
            ViewBag.PostcodeSortParm = sortOrder == "postcode" ? "postcode_desc" : "postcode";
            ViewBag.CountrySortParm = sortOrder == "country" ? "country_desc" : "country";

            // Get the list of suppliers
            var suppliers = from s in _context.Suppliers
                            select s;

            // Filter by search string
            if (!string.IsNullOrEmpty(searchString))
            {
                suppliers = suppliers.Where(s =>
                    s.Company.Contains(searchString) ||
                    s.ContactName.Contains(searchString) ||
                    s.Email.Contains(searchString) ||
                    s.Phone.Contains(searchString) ||
                    s.Address.Contains(searchString) ||
                    s.City.Contains(searchString) ||
                    s.Postcode.ToString().Contains(searchString) ||
                    s.Country.Contains(searchString));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "company":
                    suppliers = suppliers.OrderBy(s => s.Company);
                    break;
                case "contactName":
                    suppliers = suppliers.OrderBy(s => s.ContactName);
                    break;
                case "email":
                    suppliers = suppliers.OrderBy(s => s.Email);
                    break;
                case "phone":
                    suppliers = suppliers.OrderBy(s => s.Phone);
                    break;
                case "address":
                    suppliers = suppliers.OrderBy(s => s.Address);
                    break;
                case "city":
                    suppliers = suppliers.OrderBy(s => s.City);
                    break;
                case "postcode":
                    suppliers = suppliers.OrderBy(s => s.Postcode);
                    break;
                case "country":
                    suppliers = suppliers.OrderBy(s => s.Country);
                    break;
                case "contactName_desc":
                    suppliers = suppliers.OrderByDescending(s => s.ContactName);
                    break;
                case "email_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Email);
                    break;
                case "phone_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Phone);
                    break;
                case "address_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Address);
                    break;
                case "city_desc":
                    suppliers = suppliers.OrderByDescending(s => s.City);
                    break;
                case "postcode_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Postcode);
                    break;
                case "country_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Country);
                    break;
                default:
                    break;
            }

            // Execute the query and return the sorted and filtered list of suppliers
            return View(await suppliers.ToListAsync());
        }



        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
              return _context.Suppliers != null ? 
                          View(await _context.Suppliers.ToListAsync()) :
                          Problem("Entity set 'GroovyGoodsContext.Suppliers'  is null.");
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.Sid == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Sid,Company,ContactName,Email,Phone,Address,City,Postcode,Country")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Sid,Company,ContactName,Email,Phone,Address,City,Postcode,Country")] Supplier supplier)
        {
            if (id != supplier.Sid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Sid))
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
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.Sid == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Suppliers == null)
            {
                return Problem("Entity set 'GroovyGoodsContext.Suppliers'  is null.");
            }
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
          return (_context.Suppliers?.Any(e => e.Sid == id)).GetValueOrDefault();
        }
    }
}
