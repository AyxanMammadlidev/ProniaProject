namespace ProniaProject.Models
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }
        public List<ColorProduct> ProductColors { get; set; }

    }
}
