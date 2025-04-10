using System.ComponentModel.DataAnnotations.Schema;

namespace AUCTIONHIVE.Models
{
    public class Category : BaseClass
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();


    }
    public class SubCategory:BaseClass
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        [ForeignKey("Categories")]
        public string CategoryId { get; set; }
        public Category Category { get; set; }

    }

}
