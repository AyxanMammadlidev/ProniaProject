namespace ProniaProject.Models
{
    public class Order : BaseEntity
    {
        public decimal TotalPrice { get; set; }
        //Relational
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public List<OrderItem> Items { get; set; }

        


    }
}
