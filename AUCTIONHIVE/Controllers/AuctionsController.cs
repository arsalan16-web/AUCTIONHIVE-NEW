using AspNetCoreHero.ToastNotification.Abstractions;
using AUCTIONHIVE.Data;
using AUCTIONHIVE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Security.Claims;
using System.Text.Json;

namespace AUCTIONHIVE.Controllers
{
    [Authorize]
    public class AuctionsController : Controller
    {
        public INotyfService _notifyService { get; }
        public ApplicationDbContext Context { get; set; }
        public AuctionsController(ApplicationDbContext applicationDbContext, INotyfService notifyService) 
        {
            _notifyService = notifyService;
            Context = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> JoinAuction(string auctionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in User ID
            if (string.IsNullOrEmpty(auctionId))
            {
                return RedirectToAction("index", "home");
            }
            var isUserPayed =await Context.StreamingUsers.AnyAsync(x=>x.ScheduledAuctionId==auctionId && x.UserId== userId);
            if (!isUserPayed)
            {

                return RedirectToAction("PayAuctionFee","auctions", new {auctionId=auctionId});
            }
            
            return View();
        }

        public async Task<IActionResult> PayAuctionFee(string auctionId)
        {

            var scheduled = new ScheduledAuction();
            if (string.IsNullOrEmpty(auctionId))
            {
                return RedirectToAction("index", "home");
            }
            scheduled = await Context.ScheduledAuctions.Include(x => x.Product).ThenInclude(x=>x.Category)
                .ThenInclude(x=>x.SubCategories)
                .Include(x=>x.StreamingUsers).FirstOrDefaultAsync(x => x.Id == auctionId);

            return View(scheduled);
        }


        [HttpGet]
        public async Task<IActionResult> CreateCheckoutSession(string auctionId)
        {
            var scheduled = new ScheduledAuction();
            if (string.IsNullOrEmpty(auctionId))
            {
                return RedirectToAction("index", "home");
            }
            scheduled = await Context.ScheduledAuctions.Include(x => x.Product).ThenInclude(x => x.Category)
                .ThenInclude(x => x.SubCategories)
                .Include(x => x.StreamingUsers).FirstOrDefaultAsync(x => x.Id == auctionId);


            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
            {
                new Stripe.Checkout.SessionLineItemOptions
                {
                    PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                    {
                        Currency = "pkr",
                        UnitAmount =10000000, // Convert 0.01 to decimal (0.01m)
                         ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Auction Fee of {scheduled.Product.Title}",
                            Description=scheduled.Product.Description,
                            
                        }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = "https://localhost:7039/payment-success?session_id={CHECKOUT_SESSION_ID}&auctionid=" + scheduled.Id,
                CancelUrl = "https://localhost:7039/error&auctionid=" + scheduled.Id,
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            return Redirect(session.Url);
        }

        public async Task<IActionResult> PaymentSuccess(string session_id, string auctionid)
        {
            try
            {
                if (string.IsNullOrEmpty(session_id) || string.IsNullOrEmpty(auctionid))
                {
                    _notifyService.Error("Something went wrong...");

                    return RedirectToAction("index", "home");
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in User ID

                StreamingUsers streamingUsers = new StreamingUsers();
                streamingUsers.ScheduledAuctionId = auctionid;
                streamingUsers.UserId = userId;
                streamingUsers.CreatedAt = DateTime.Now;
                streamingUsers.CreatedBy = User.Identity.Name;
                streamingUsers.StreamStatus = "Enrolled";

                Context.StreamingUsers.Add(streamingUsers);
                await Context.SaveChangesAsync();
                _notifyService.Success("Enrollment done Successfuly...");


            }
            catch (Exception e)
            {

            }
            



            return RedirectToAction("auctions", "index");


        }

        public async Task<IActionResult> Error(string auctionid)
        {
            if (!string.IsNullOrEmpty(auctionid))
            {
                _notifyService.Success("Payment processing failed...");

                return RedirectToAction("PayAuctionFee", "auctions", new { auctionid = auctionid });

            }
            return RedirectToAction("index", "home");

        }

        public async Task<decimal> GetUsdToPkrRateAsync()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("https://open.er-api.com/v6/latest/USD");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ExchangeRateResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return data.Rates["PKR"];
        }

        public class ExchangeRateResponse
        {
            public Dictionary<string, decimal> Rates { get; set; }
        }


    }
}
