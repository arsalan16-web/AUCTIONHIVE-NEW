using AUCTIONHIVE.Data;
using AUCTIONHIVE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace AUCTIONHIVE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public  async Task< IActionResult> Index()
        {
            ProductsViewModel model = new ProductsViewModel();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in User ID


            var allProduct =  _context.Products.Include(x => x.Images)
                .Include(x=>x.Category)
                .Include(x => x.SubCategory).Where(x=>x.Status=="Active").GroupBy(x=>x.CategoryId);

            var topCategories = _context.SearchedHistories
    .Where(s => s.UserId == userId && s.CategoryId != null)
    .GroupBy(s => s.CategoryId)
    .Select(g => new
    {
        CategoryId = g.Key,
        SearchCount = g.Count()
    })
    .OrderByDescending(g => g.SearchCount)
    .Take(3)
    .Select(g => g.CategoryId)
    .ToList();
            if (topCategories.Count > 0)
            {
                var topProducts = _context.Products.Include(x => x.Images).Include(x => x.SubCategory)
               .Where(p => topCategories.Contains(p.CategoryId) && p.Status=="Active")
               .GroupBy(p => p.CategoryId)
             
               .SelectMany(g => g
                   .OrderByDescending(p => p.ProductViews)
                   .Take(5)
               )
               .ToList();


                model.TopSuggestedProducts = topProducts;
            }
            else {
                model.TopSuggestedProducts = _context.Products.Include(x => x.SubCategory).Include(x => x.Images).Where(x => x.Status == "Active").OrderByDescending(x => x.ProductViews).Take(15).ToList();
                    }

                // Step 2: Get top 5 products for each category based on ProductViewCount



                foreach (var category in allProduct)
                {
                    model.ProductByCategory.Add(new ProductsByCategories()
                    {
                        Category = category.FirstOrDefault().Category,
                        Products = category.OrderByDescending(x=>x.ProductViews).Take(15).ToList(),

                    });
                }
          


            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
