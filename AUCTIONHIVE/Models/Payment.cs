using AUCTIONHIVE.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace AUCTIONHIVE.Models
{
    public class Payment : BaseClass
    {
        [ForeignKey("AspNetUsers")]

        public string UserId { get; set; }  // Foreign Key to User
        public ApplicationUser User { get; set; }  // Foreign Key to User
        [ForeignKey("Products")]

        public string ProductId { get; set; }  // Foreign Key to Product
        public Product Product { get; set; }  // Foreign Key to Product

        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }  // e.g., Pending, Completed, Failed
        public string PaymentMethod { get; set; }  // e.g., Credit Card, Stripe, PayPal
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string TransactionId { get; set; }  // Unique identifier for the transaction (e.g., Stripe payment ID)
    }
}
