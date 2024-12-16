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

        public async Task<IActionResult> Index()
        {
            

            HomeVM homeVM = new HomeVM
            {
                Sliders = await _context.Slides.OrderBy(s=>s.Order).Take(2).ToListAsync(),

                NewProducts = await _context.Products.OrderByDescending(p=>p.CreatedAt).Take(8).Include(p => p.Images.Where(p => p.IsPrime != null)).ToListAsync()
            };

          return View(homeVM);
        }

        public IActionResult Error(string errorMessage)
        {
            return View(model: errorMessage); 
        }
    }
}
