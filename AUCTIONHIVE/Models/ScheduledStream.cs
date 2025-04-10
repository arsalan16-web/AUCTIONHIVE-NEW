using AUCTIONHIVE.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace AUCTIONHIVE.Models
{
    public class ScheduledAuction:BaseClass
    {
        public DateTime StartTime { get; set; } // When the stream will start
        public DateTime EndTime { get; set; }   // When the stream will end
        public string StreamUrl { get; set; }   // URL for accessing the stream
        public string StreamChannel { get; set; } // E.g., "Scheduled", "Active", "Ended"
        public List<StreamingUsers> StreamingUsers { get; set; }
        public List<Bid> Bids { get; set; }

        [ForeignKey("Products")]
        public string ProductId { get; set; }
        public Product Product { get; set; }

    }
    public class StreamingUsers : BaseClass
    {
        [ForeignKey("AspNetUsers")]

        public string UserId { get; set; }  // Foreign key to User
        public ApplicationUser User { get; set; }  // Foreign key to User
        [ForeignKey("ScheduledAuctions")]
        public string ScheduledAuctionId { get; set; }  // Foreign key to ScheduledStream
        public ScheduledAuction ScheduledAuction { get; set; }  // Foreign key to ScheduledStream
        public DateTime JoinedAt { get; set; }  // Timestamp for when the user joined the stream
        public string StreamStatus { get; set; }  // E.g., "Watching", "Bidding", etc.
    }

    public class Bid : BaseClass
    {
        //[ForeignKey("Products")]

        //public string ProductId { get; set; }
        //public Product Product { get; set; }


        [ForeignKey("ScheduledAuctions")]

        public string StreamId { get; set; }
        public ScheduledAuction Stream { get; set; }
        [ForeignKey("AspNetUsers")]

        public string BidderId { get; set; }
        public ApplicationUser Bidder { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }  // Active, Won, Outbid
    }
}
