namespace ProniaProject.Models
{
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        //relational
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ProductImage> Images { get; set; }

    }
}