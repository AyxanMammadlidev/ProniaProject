using Microsoft.AspNetCore.Mvc;

namespace ProniaProject.Areas.Admin.ViewModels
{
    public class UpdateSliderVM 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Desc { get; set; }
        public string? Image { get; set; }
        public int Order { get; set; }

        public IFormFile? Photo { get; set; }

    }
}
