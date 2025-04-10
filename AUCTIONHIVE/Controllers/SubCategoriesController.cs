using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AUCTIONHIVE.Data;
using AUCTIONHIVE.Models;

namespace AUCTIONHIVE.Controllers
{
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubCategories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SubCategories.Include(s => s.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SubCategories/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // GET: SubCategories/Create
        public IActionResult Create()
        {
            // Load dropdown data
            ViewBag.Categories = _context.Categories.Where(c => c.IsDeleted == false).ToList();
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategory subCategory)
        {
            subCategory.Id= Guid.NewGuid().ToString();
            subCategory.Description = subCategory.Description;
            subCategory.Name = subCategory.Name;
            subCategory.CategoryId = subCategory.CategoryId;
            subCategory.UpdateAt = DateTime.Now;
            subCategory.CreatedAt = DateTime.Now;
            _context.Add(subCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", subCategory.CategoryId);
            //return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _context.Categories.Where(c => c.IsDeleted == false).ToList();
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Description,CategoryId,Id,CreatedBy,UpdatedBy,CreatedAt,UpdateAt")] SubCategory subCategory)
        {
            if (id != subCategory.Id)
            {
                return NotFound();
            }


            try
            {
                _context.Update(subCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCategoryExists(subCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            //ViewBag.Categories = _context.Categories.Where(c => c.IsDeleted == false).ToList();
            //return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory != null)
            {
                _context.SubCategories.Remove(subCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(string id)
        {
            return _context.SubCategories.Any(e => e.Id == id);
        }
    }
}
