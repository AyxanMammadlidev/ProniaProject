using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.ViewModels;

namespace ProniaProject.Conrollers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) {
            _context = context;
        }

        public IActionResult Index()
        {

            HomeVM homeVM = new HomeVM
            {
                Sliders = _context.Slides.OrderBy(s=>s.Order).ToList(),
                Products = _context.Products.Include(p=>p.Images.Where(p=>p.IsPrime != null)).Take(8).ToList()

            };

          return View(homeVM);
        }

    }
}
