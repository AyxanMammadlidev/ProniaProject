namespace ProniaProject.Models
{
    public abstract class BaseEntity
    {
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
