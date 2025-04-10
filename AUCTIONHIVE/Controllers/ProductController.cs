using AUCTIONHIVE.Data;
using AUCTIONHIVE.Models;
using AUCTIONHIVE.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AUCTIONHIVE.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            try
            {


                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in User ID

                //var list= FakeProductGenerator.GenerateFakeProducts(200, userId);
                //List<ScheduledAuction> li=new List<ScheduledAuction>();
                //foreach(var item in list.Where(x => x.IsAuction))
                //{
                //    if (item.IsAuction)
                //    {
                //        ScheduledAuction scheduledAuction = new ScheduledAuction
                //        {
                //            ProductId = item.Id,
                //            Id = Guid.NewGuid().ToString(),
                //            CreatedAt = DateTime.Now,
                //            UpdateAt = DateTime.Now,
                //            CreatedBy = userId,
                //            UpdatedBy = userId,
                //            StartTime = DateTime.Now,
                //            EndTime = DateTime.Now.AddHours(2),
                //            StreamUrl = "http",
                //            StreamChannel = "http",
                //        };
                //        li.Add(scheduledAuction);
                //    }


                //}
                //_context.Products.AddRange(list);
                //await _context.SaveChangesAsync();

                //_context.ScheduledAuctions.AddRange(li);
                //await _context.SaveChangesAsync();



                var products = _context.Products.Include(x=>x.Images).Include(s=>s.Category).Include(s=>s.SubCategory).Where(s=>s.CreatedBy ==  userId && s.IsDeleted == false).ToList();
                ProductsModel model = new ProductsModel();
                List<ProductsModel> productsList = new List<ProductsModel>();
                ProductsModel obj;  
                foreach (var product in products) 
                {
                    obj = new ProductsModel();
                    obj.Id = product.Id;
                    obj.Title = product.Title;
                    obj.Description = product.Description;
                    obj.Price = product.Price;
                    obj.CategoryName = product.Category.Name;
                    obj.SubCategoryName = product.SubCategory.Name;
                    obj.Location = product.Location;
                    obj.Condition = product.Condition;
                    obj.Status = product.Status;
                    obj.IsAuction = product.IsAuction;
                    obj.IsVideoStreaming = product.IsVideoStreaming;
                  //  obj.ScheduledAuctionId = product.ScheduledAuctionId;
                    obj.BiddingFee = product.BiddingFee;
                    obj.Images = product.Images;
                    obj.CreatedBy = product.CreatedBy;
                    obj.CreatedAt = product.CreatedAt;
                    obj.UpdateAt = product.UpdateAt;
                    obj.UpdatedBy = product.UpdatedBy;
                    obj.IsDeleted = product.IsDeleted;
                    productsList.Add(obj);

                }
                return View(productsList);

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
            }
            return View();
        }
        public IActionResult AddEditProduct(string? id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Load dropdown data
                ViewBag.Categories = _context.Categories
                    .Where(c => c.IsDeleted == false)
                    .ToList();

                ViewBag.SubCategories = _context.SubCategories
                    .Where(s => s.IsDeleted == false )
                    .ToList();

                ViewBag.ScheduledAuctions = _context.ScheduledAuctions
                    .Where(a => a.IsDeleted == false && a.CreatedBy == userId)
                    .ToList();
                ProductsModel model = new ProductsModel();
                if (!string.IsNullOrEmpty(id))
                {
                    // Edit product
                    var product = _context.Products
                        .Include(p => p.Images).Include(s => s.Category).Include(s => s.SubCategory)
                        .FirstOrDefault(p => p.Id == id && p.IsDeleted == false && p.CreatedBy == userId);

                    if (product == null)
                    {
                        return NotFound();
                    }
                    model.Title = product.Title;
                    model.CategoryId = product.CategoryId;
                    model.SubCategoryId = product.SubCategoryId;
                    model.SellerId = product.SellerId;
                    model.Status = product.Status;
                    model.BiddingFee = product.BiddingFee;
                    model.Condition = product.Condition;
                    model.Description = product.Description;
                    model.Price = product.Price;
                    model.Location = product.Location;
                    model.Id = product.Id;
                    model.Images = product.Images;
                    model.IsAuction = product.IsAuction;
                    model.IsVideoStreaming = product.IsVideoStreaming;
                    return View(model);
                }
                else
                {
                    return View(new ProductsModel());
                }
                

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
                return View(new Product());
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditProduct(ProductsModel model, List<IFormFile> uploadedImages)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Create or update logic
                Product product;
                if (!string.IsNullOrEmpty(model.Id))
                {
                    // Update
                    product = _context.Products
                        .Include(p => p.Images)
                        .FirstOrDefault(p => p.Id == model.Id && p.CreatedBy == userId);

                    if (product == null)
                        return NotFound();
                }
                else
                {
                    // Create
                    product = new Product
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                        Images = new List<Images>()
                    };
                    _context.Products.Add(product);
                }

                // Update fields
                product.Title = model.Title;
                product.Price = model.Price;
                product.Description = model.Description;
                product.Location = model.Location;
                product.CategoryId = model.CategoryId;
                product.SubCategoryId = model.SubCategoryId;
                product.Condition = model.Condition;
                product.Status = model.Status;
                product.IsAuction = model.IsAuction;
                product.IsVideoStreaming = model.IsVideoStreaming;
                if (product.IsAuction)
                {
                    ScheduledAuction scheduledAuction = new ScheduledAuction
                    {
                        ProductId = product.Id,
                        Id = Guid.NewGuid().ToString(),
                        CreatedAt =DateTime.Now,
                        UpdateAt = DateTime.Now,
                        CreatedBy = userId,
                        UpdatedBy = userId,
                        StartTime =DateTime.Now,
                        EndTime = DateTime.Now.AddHours(1),
                        StreamUrl = "http",
                        StreamChannel = "http",
                    };
                    _context.ScheduledAuctions.Add(scheduledAuction);
                }
                product.BiddingFee = model.BiddingFee;
                product.UpdatedBy = userId;
                product.UpdateAt = DateTime.UtcNow;
                product.SellerId= "ecaff4b8-f5eb-426f-af6e-43c760753e64";

                // Save uploaded images
                if (uploadedImages != null && uploadedImages.Any())
                {
                    foreach (var file in uploadedImages)
                    {
                        if (file.Length > 0)
                        {
                            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                            if (!Directory.Exists(uploads))
                                Directory.CreateDirectory(uploads);

                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploads, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            product.Images.Add(new Images
                            {
                                Id = Guid.NewGuid().ToString(),
                                Image = "/uploads/" + fileName,
                                ProductId = product.Id,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = userId
                            });
                        }
                    }
                }

                await _context.SaveChangesAsync();

                TempData["Success"] = "Product saved successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error saving product: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult GetSubCategories(string categoryId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subCategories = _context.SubCategories
                .Where(s => s.CategoryId == categoryId && s.IsDeleted == false)
                .Select(s => new { id = s.Id, name = s.Name })
                .ToList();

            return Json(subCategories);
        }
        [HttpPost]
        public JsonResult Delete(string id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product != null)
                {
                    product.IsDeleted = true;
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Product not found." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                await _context.Products
      .Where(n => n.Id == id)
      .ExecuteUpdateAsync(setters => setters
          .SetProperty(n => n.ProductViews, n => n.ProductViews + 1)
          
      );

                var findProduct = await _context.Products.Include(x=>x.Seller)
                    .Include(x=>x.Category).Include(x => x.SubCategory).Include(x => x.ScheduledAuction).ThenInclude(x=>x.Bids).ThenInclude(x=>x.Bidder)
                    .Include(x => x.Images).FirstOrDefaultAsync(x=>x.Id==id);
                return View(findProduct);


            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
            }
            return RedirectToAction("index","Home");

        }

        public async Task<IActionResult> JoinAuction()
        {
            return View();
        }
    }
}
