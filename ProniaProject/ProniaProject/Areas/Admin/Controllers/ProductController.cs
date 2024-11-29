using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.Areas.Admin.ViewModels;
using ProniaProject.Areas.Admin.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Models;

namespace ProniaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext  _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public async Task<IActionResult> Index()
        {
          List<GetProductAdminVM> productsVM = await _context.Products.Include(p => p.Category).Include(p=>p.Images.Where(pi=>pi.IsPrime == true)).
                Select(
                   p=>new GetProductAdminVM
                   {   Id = p.Id,
                       Name = p.Name,
                       CategoryName  = p.Category.Name,
                       Price = p.Price,
                       Image = p.Images[0].Image
                   }
                )
                .ToListAsync();

            return View(productsVM);
        }

        public async Task<IActionResult> Create()
        {
            CreateProductVM productVM = new()
            {
                Categories = await _context.Categories.ToListAsync(),
            };

            return View(productVM);
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            productVM.Categories = await _context.Categories.ToListAsync();

            bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);

            if (!result)
            {
                ModelState.AddModelError(nameof(CreateProductVM), "Category does not exist");
                return View(productVM);
            }

            Product product = new()
            {
                Name = productVM.Name,
                SKU = productVM.SKU,
                CategoryId = productVM.CategoryId,
                Price = productVM.Price,
                Description = productVM.Description,
                CreatedAt = DateTime.Now,
                IsDeleted = false

            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
