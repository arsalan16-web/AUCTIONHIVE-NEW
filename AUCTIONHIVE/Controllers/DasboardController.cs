using Microsoft.AspNetCore.Mvc;

namespace AUCTIONHIVE.Controllers
{
    public class DasboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
