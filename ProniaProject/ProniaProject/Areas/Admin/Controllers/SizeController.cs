using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApplication.Areas.ViewModels;
using ProniaProject.Areas.Admin.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Models;

namespace ProniaProject.Areas.Admin.Controllers
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
            List<Size> sizes = await _context.Sizes.ToListAsync();
            return View(sizes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? id, CreateSizeVM sizeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(sizeVM);
            }

            bool result = await _context.Color.AnyAsync(c => c.Name.Trim() == sizeVM.Name.Trim());

            if (result)
            {
                ModelState.AddModelError(nameof(sizeVM.Name), $"{sizeVM.Name} is already exist");
                return View();
            }

            Size size = new()
            {
                Name = sizeVM.Name,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };

            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View();
            }
            Size Size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
            if (Size is null) return NotFound();
            UpdateSizeVM SizeVM = new UpdateSizeVM
            {
                Name = Size.Name
            };
            return View(SizeVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateSizeVM SizeVM)
        {
            if (id == null || id < 1) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View();
            }
            Size Size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
            if (Size is null) return NotFound();
            if (_context.Sizes.Any(s => s.Name == SizeVM.Name && s.Id != SizeVM.Id))
            {
                ModelState.AddModelError(nameof(UpdateSizeVM.Name), "Size must be unique");
                return View(SizeVM);
            }
            Size.Name = SizeVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Size Size = await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);

            if (Size is null) return NotFound();
            _context.Sizes.Remove(Size);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }

}

