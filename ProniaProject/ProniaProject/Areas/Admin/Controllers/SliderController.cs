using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;
using ProniaProject.Models;

namespace ProniaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
             _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Slides.ToListAsync();
            return View(sliders);
        }


        public async Task<IActionResult> Create(Slider slider)
        {

            if (!ModelState.IsValid) return View();

            await _context.Slides.AddAsync(slider);
          await _context.SaveChangesAsync();

          return RedirectToAction(nameof(Index));

        }
    }

}
