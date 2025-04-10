using AUCTIONHIVE.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace AUCTIONHIVE.Models
{
    public class Message : BaseClass
    {
        [ForeignKey("AspNetUsers")]
        public string SenderId { get; set; }  // The user who sent the message
        public ApplicationUser Sender { get; set; }  // The user who sent the message
        [ForeignKey("AspNetUsers")]
        public string ReceiverId { get; set; }  // The user who receives the message
        public ApplicationUser Receiver { get; set; }  // The user who receives the message
        [ForeignKey("Products")]
        public string? ProductId { get; set; }  // The product being discussed (optional)
        public Product? Product { get; set; }  // The product being discussed (optional)
        public string Content { get; set; }  // The content of the message
        public DateTime SentAt { get; set; }  // Timestamp when the message was sent
        public bool IsRead { get; set; }  // Whether the message was read
    }
}
