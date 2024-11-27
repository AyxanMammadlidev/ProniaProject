using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using ProniaProject.Areas.Admin.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.Utils;

namespace ProniaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
             _context = context;
             _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Slides.ToListAsync();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM sliderVM)
        {
            //if (!ModelState.IsValid) return View();

            if (!sliderVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is incorrect");
                return View();
            }
            if (!sliderVM.Photo.ValidateSize(Utils.Enums.FileSize.Mb, 2))
            {
                ModelState.AddModelError("Photo", "File size must be less than 2 mb");
                return View();
            }

            Slider slide = new()
            {
                Title = sliderVM.Title,
                Subtitle = sliderVM.Subtitle,
                Desc = sliderVM.Description,
                Order = sliderVM.Order,
                Image = await sliderVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images"),
                IsDeleted = false,
                CreatedAt = DateTime.Now,
            };

            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Update(int? id, UpdateSliderVM slider)
        {
            if (id == null || id < 1) return BadRequest();

            Slider slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (slide == null) return NotFound();

            Slider sliderVM = new()
            {
                Title = slide.Title,
                Subtitle = slide.Subtitle,
                Desc = slide.Desc,
                Order = slide.Order,
                Image = slide.Image
            };

            return View(sliderVM);

        }



        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || id<1) return BadRequest();

            Slider slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if(slide == null) return NotFound();

            slide.Image.DeleteImage(_env.WebRootPath,"assets","images","website-images");

            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }

}

