using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApplication.Areas.ViewModels;
using ProniaProject.Areas.Admin.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Models;


namespace ProniaApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    [AutoValidateAntiforgeryToken]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;


        public ColorController(AppDbContext context)
        {
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            List<Color> colors = await _context.Color.ToListAsync();
            return View(colors);
        }

        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id,CreateColorVM colorVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool result = await _context.Color.AnyAsync(c => c.Name.Trim() == colorVM.Name.Trim());

            if (result)
            {
                ModelState.AddModelError(nameof(colorVM.Name), $"{colorVM.Name} is already exist");
                return View();
            }

            Color color = new()
            {
                Name = colorVM.Name,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _context.Color.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

     
    }
}