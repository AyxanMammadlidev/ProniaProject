

namespace ProniaProject.Areas.Admin.ViewModels
{
    public class CreateSliderVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public IFormFile Photo { get; set; }

    }
}
