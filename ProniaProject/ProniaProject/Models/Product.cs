namespace ProniaProject.Models
{
    public class Product : BaseEntity
    {

        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        //relational
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<ProductImage>? Images { get; set; }
        public List<ProductTag> ProductTags { get; set; }

        public List<ColorProduct> ColorProducts { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        


    }
}