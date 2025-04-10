using AUCTIONHIVE.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace AUCTIONHIVE.Models
{
    public class SearchedHistory:BaseClass {

        [ForeignKey("AspNetUsers")]

        public string UserId { get; set; } // Foreign Key to User
        public ApplicationUser User { get; set; }

        [ForeignKey("Categories")]
        public string? CategoryId { get; set; }
        public Category Category { get; set; }
        [ForeignKey("SubCategories")]
        public string? SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }


    }

    public class Product : BaseClass
    {
        [ForeignKey("AspNetUsers")]

        public string SellerId { get; set; } // Foreign Key to User
        public ApplicationUser Seller { get; set; }
        public string Title { get; set; }
        public bool IsDiscount { get; set; } = false;
        public decimal DiscountedPrice { get; set; } = 0;
        [Range(1,100)]
        public decimal DiscountPercentage { get; set; } = 0;
        public string Description { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("Categories")]
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        [ForeignKey("SubCategories")]
        public string SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }

        public List<Images> Images { get; set; }
        public string Location { get; set; }
        public string Condition { get; set; }  // New, Used
        public string Status { get; set; }  // Active, Sold, Expired
        public bool IsAuction { get; set; }
        public bool IsVideoStreaming { get; set; }
        //[ForeignKey("ScheduledAuctions")]
        //public string? ScheduledAuctionId { get; set; }
        public ScheduledAuction? ScheduledAuction { get; set; }

        public double BiddingFee { get; set; } = 0;

        public int ProductViews { get;set; } = 0;
    }

   
    public class Images : BaseClass
    {

        public string Image { get; set; }
        [ForeignKey("Products")]

        public string ProductId { get; set; }
        public Product Product { get; set; }

    }

}
