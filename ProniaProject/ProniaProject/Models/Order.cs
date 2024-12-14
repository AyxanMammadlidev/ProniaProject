namespace ProniaProject.Models
{
    public class Order : BaseEntity
    {
        public string Adress { get; set; }
        public decimal TotalPrice { get; set; }
        //Relational
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<OrderItem> Items { get; set; }
        public bool? Status { get; set; }

    }
}
