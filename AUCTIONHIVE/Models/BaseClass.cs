namespace AUCTIONHIVE.Models

{
    

    public class BaseClass
    {
        public string Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
