using AUCTIONHIVE.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AUCTIONHIVE.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<ScheduledAuction>  ScheduledAuctions { get; set; }
        public DbSet<StreamingUsers> StreamingUsers { get; set; }
        public DbSet<Bid>  Bids { get; set; }
        public DbSet<Message>  Messages { get; set; }
        public DbSet<Payment>  Payments { get; set; }
        public DbSet<SearchedHistory> SearchedHistories { get; set; }
        public DbSet<BidingPercentage> BidingPercentages { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
