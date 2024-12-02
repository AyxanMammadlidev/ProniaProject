using ProniaProject.Models;
using System.ComponentModel.DataAnnotations;

namespace ProniaProject.Areas.Admin.ViewModels
{
    public class UpdateProductVM
    {
        public IFormFile? MainPhoto { get; set; }
        public IFormFile? HoverPhoto { get; set; }
        public List<IFormFile>? AdditionalPhotos { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<Color>? Colors { get; set; }
        public List<int>? ColorIds { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<int>? ImageIds { get; set; }
    }
}
