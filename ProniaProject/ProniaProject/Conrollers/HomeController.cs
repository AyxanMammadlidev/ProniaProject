using Microsoft.AspNetCore.Mvc;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.ViewModels;

namespace ProniaProject.Conrollers
{
    public class HomeController : Controller
    {
        public readonly AppDbContext _context;
        public HomeController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {
            //List<Slider> sliders = new List<Slider>
            //{
            //    new Slider
            //    {
            //        Title = "title 1",
            //        Subtitle = "Subtitle 1",
            //        Desc = "Güllerden qalmadi",
            //        Image = "/1-2-524x617.png",
            //        Order = 1,
            //        IsDeleted = false,
            //        CreatedAt = DateTime.Now

            //    },

            //    new Slider
            //    {
            //        Title = "title 2",
            //        Subtitle = "Subtitle 2",
            //        Desc = "Sizden daha gözel güller",
            //        Image = "/1-2-524x617.png",
            //        Order = 2,
            //        IsDeleted = false,
            //        CreatedAt = DateTime.Now

            //    },
            //     new Slider
            //    {
            //        Title = "title 3",
            //        Subtitle = "Subtitle 3",
            //        Desc = "Bext açan güller",
            //        Image = "/1-2-524x617.png",
            //        Order = 3,
            //        IsDeleted = false,
            //        CreatedAt = DateTime.Now

            //    }


            //};

            //_context.Slides.AddRange(sliders);
            //_context.SaveChanges();
            HomeVM homeVM = new HomeVM
            {
                Sliders = _context.Slides.OrderBy(s=>s.Order).ToList(),
            };

          return View(homeVM);
        }

    }
}
