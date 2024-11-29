namespace ProniaProject.Models
{
    public class ProductImage : BaseEntity
    {

        public string Image { get; set; }
        public bool? IsPrime { get; set; }

        //relational
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}