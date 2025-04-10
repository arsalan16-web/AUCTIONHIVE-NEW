using AUCTIONHIVE.Data;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace AUCTIONHIVE.Models
{
    public static class STATIC
    {
        public static string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow - dateTime.ToUniversalTime();

            if (timeSpan.TotalSeconds < 60)
                return $"{timeSpan.Seconds} seconds ago";
            if (timeSpan.TotalMinutes < 60)
                return $"{timeSpan.Minutes} minutes ago";
            if (timeSpan.TotalHours < 24)
                return $"{timeSpan.Hours} hours ago";
            if (timeSpan.TotalDays < 7)
                return $"{timeSpan.Days} days ago";
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} weeks ago";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} months ago";

            return $"{(int)(timeSpan.TotalDays / 365)} years ago";
        }

        public static List<Product> GenerateFakeProducts(int count, string userId)
        {
            var productFaker = new Faker<Product>()
                .RuleFor(p => p.Id, f => Guid.NewGuid().ToString())
                .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
                .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
                .RuleFor(p => p.CategoryId, f => "345678987654567")
                .RuleFor(p => p.SubCategoryId, f => "8c7922a4-66ec-4222-8c87-21974880ec06")
                .RuleFor(p => p.Condition, f => f.PickRandom(new[] { "New", "Used", "Refurbished" }))
                .RuleFor(p => p.Status, f => "Active")
                .RuleFor(p => p.IsAuction, f => f.Random.Bool())
                .RuleFor(p => p.IsVideoStreaming, f => f.Random.Bool())
                .RuleFor(p => p.BiddingFee, f => f.Random.Double(1, 50))
                .RuleFor(p => p.Location, f => f.Address.City())
                .RuleFor(p => p.CreatedAt, f => DateTime.UtcNow)
                .RuleFor(p => p.CreatedBy, f => userId)
                .RuleFor(p => p.UpdateAt, f => DateTime.UtcNow)
                .RuleFor(p => p.UpdatedBy, f => userId)
                .RuleFor(p => p.SellerId, f => userId)

                .RuleFor(p => p.Images, f =>
                {
                    var images = new List<Images>();
                    for (int i = 0; i < f.Random.Int(1, 3); i++)
                    {
                        images.Add(new Images
                        {
                            Id = Guid.NewGuid().ToString(),
                            Image = "/uploads/1654e896-bef7-49b4-840b-3de701257c96.jpg",
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = userId
                        });
                    }
                    return images;
                });

            return productFaker.Generate(count);
        }
    }
    //public class CombinedService
    //{

    //    public ApplicationDbContext _context;
    //    public CombinedService(ApplicationDbContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task<List<Category>> GetCategories()
    //    {
    //        List<Category> categories = await _context.Categories.Include(x => x.SubCategories).ToList() ;


    //    }


    //}
}
