using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Create(Slider slide)
        {
            //if (!ModelState.IsValid) return View();

            if (!slide.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "File type is incorrect");
                return View();
            }
            if (!slide.Photo.ValidateSize(Utils.Enums.FileSize.Mb, 2))
            {
                ModelState.AddModelError("Photo", "File size must be less than 2 mb");
                return View();
            }

            string originalFileName = slide.Photo.FileName;
            int lastDotIndex = originalFileName.LastIndexOf('.');
            string fileExtension = originalFileName.Substring(lastDotIndex);

            string fileName = string.Concat(Guid.NewGuid().ToString(), fileExtension);
            string path = Path.Combine(_env.WebRootPath,"assets", "images", "website-images", fileName );

            FileStream fileStream = new FileStream(path, FileMode.Create);

            await slide.Photo.CopyToAsync(fileStream);
            fileStream.Close();
            slide.Image = await slide.Photo.CreateFileAsync(_env.WebRootPath,"assets","images","website-images");

            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
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

