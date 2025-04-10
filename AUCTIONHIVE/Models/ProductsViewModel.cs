namespace AUCTIONHIVE.Models
{
    public class ProductsViewModel
    {
        
        public List<Product> TopSuggestedProducts { get; set; } = new List<Product>();
        public List<ProductsByCategories> ProductByCategory { get; set; } = new List<ProductsByCategories>();
        public List<Product> OngoingAuctions { get; set; } = new List<Product>();

    }
    public class ProductsByCategories
    {
        public Category Category { get; set; } = new Category();
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
