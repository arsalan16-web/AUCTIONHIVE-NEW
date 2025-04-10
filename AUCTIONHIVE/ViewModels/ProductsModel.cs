using AUCTIONHIVE.Data;
using AUCTIONHIVE.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AUCTIONHIVE.ViewModels
{
    public class ViewModel
    {
        public string Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class ProductsModel : ViewModel
    {
        public string SellerId { get; set; } // Foreign Key to User
        public ApplicationUser Seller { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public bool IsDiscount { get; set; } = false;

        public decimal DiscountedPrice { get; set; } = 0;
        public string Location { get; set; } //Address
        public string Condition { get; set; }  // New, Used
        public string Status { get; set; }  // Active, Sold, Expired
        public bool IsAuction { get; set; }
        public bool IsVideoStreaming { get; set; }
        public string? ScheduledAuctionId { get; set; }
        public double BiddingFee { get; set; } = 0;
        public List<Images> Images { get; set; }

    }
}
