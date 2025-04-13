using AUCTIONHIVE.Data;
using AUCTIONHIVE.Models;
using AUCTIONHIVE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AUCTIONHIVE.Controllers
{
    public class BidingPercentageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BidingPercentageController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            try
            {
                var bidingPercentage = _context.BidingPercentages.Where(s => s.IsDeleted == false).ToList();
                return View(bidingPercentage);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View();
        }
        public IActionResult AddEdit(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new BidingPercentage());
            }

            var entity = _context.BidingPercentages.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (entity == null) return NotFound();

            return View(entity);
        }

        [HttpPost]
        public IActionResult AddEdit(BidingPercentage model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in User ID

            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = Guid.NewGuid().ToString();
                model.CreatedAt = DateTime.UtcNow;
                //model.PriceInPer = model;
                model.IsDeleted = false;
                model.UpdateAt = DateTime.UtcNow;
                model.CreatedBy = userId;
                model.UpdatedBy = userId;
                // model.CreatedBy = ...
                _context.BidingPercentages.Add(model);
            }
            else
            {
                var dbModel = _context.BidingPercentages.FirstOrDefault(x => x.Id == model.Id);
                if (dbModel == null) return NotFound();

                dbModel.PriceInPer = model.PriceInPer;
                dbModel.UpdateAt = DateTime.UtcNow;
                // dbModel.UpdatedBy = ...
            }

            _context.SaveChanges();
            return RedirectToAction("Index");


            return View(model);
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            var model = _context.BidingPercentages.FirstOrDefault(x => x.Id == id);
            if (model == null) return Json(new { success = false });

            model.IsDeleted = true;
            model.UpdateAt = DateTime.UtcNow;
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
