using Microsoft.AspNetCore.Mvc;
using ProniaProject.Models;
using ProniaProject.ViewModels;

namespace ProniaProject.Conrollers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<Slider> sliders = new List<Slider>
            {
                new Slider
                {
                    Title = "title 1",
                    Subtitle = "Subtitle 1",
                    Desc = "Güllerden qalmadi",
                    Image = "/1-2-524x617.png",
                    Order = 1,

                },

                new Slider
                {
                    Title = "title 1",
                    Subtitle = "Subtitle 1",
                    Desc = "Güllerden qalmadi",
                    Image = "/1-2-524x617.png",
                    Order = 2,

                },
                 new Slider
                {
                    Title = "title 1",
                    Subtitle = "Subtitle 1",
                    Desc = "Güllerden qalmadi",
                    Image = "/1-2-524x617.png",
                    Order = 2,

                }
            };
            HomeVM homeVM = new HomeVM
            {
                Sliders = sliders
            };

          return View(homeVM);
        }

    }
}
