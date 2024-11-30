using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.Areas.Admin.ViewModels;
using ProniaProject.DAL;
using ProniaProject.Models;

namespace ProniaProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public async Task<IActionResult> Index()
        {
            List<GetProductAdminVM> productsVM = await _context.Products.Include(p => p.Category).Include(p => p.Images.Where(pi => pi.IsPrime == true)).
                  Select(
                     p => new GetProductAdminVM
                     {
                         Id = p.Id,
                         Name = p.Name,
                         CategoryName = p.Category.Name,
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
                Tags = await _context.Tags.ToListAsync(),
                Categories = await _context.Categories.ToListAsync()
            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            productVM.Categories = await _context.Categories.ToListAsync();
            productVM.Tags = await _context.Tags.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(productVM);
            }
            

            bool result = productVM.Categories.Any(c => c.Id == productVM.CategoryId);

            if (!result)
            {
                ModelState.AddModelError(nameof(CreateProductVM), "Category does not exist");
                productVM.Categories = await _context.Categories.ToListAsync();
                return View(productVM);
            }

            if(productVM.TagIds is null)
            {
                productVM.TagIds = new List<int>();
            }

            bool tagResult = productVM.TagIds.Any(tId => !productVM.Tags.Exists(t => t.Id == tId));

            if (tagResult)
            {
                ModelState.AddModelError(nameof(CreateProductVM.TagIds),"not exists");
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
                IsDeleted = false,
                ProductTags = productVM.TagIds.Select(tId=>new ProductTag { TagId = tId }).ToList()
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id < 1) return BadRequest();

            Product product = await _context.Products.Include(p=>p.ProductTags).FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();


            UpdateProductVM productVM = new()
            {
                Name = product.Name,
                SKU = product.SKU,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Description = product.Description,
                Categories = await _context.Categories.ToListAsync(),
                Tags = await _context.Tags.ToListAsync(),
                TagIds = product.ProductTags.Select(pt=>pt.TagId).ToList()
                
            };

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, UpdateProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                return View(productVM);
            }

            Product existed = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (existed.CategoryId != productVM.CategoryId)
            {
                bool result = _context.Products.Any(c => c.Id == productVM.CategoryId);

                if (!result)
                {
                    productVM.Categories = await _context.Categories.ToListAsync();
                    return View(productVM);
                }
            }

            existed.CategoryId = productVM.CategoryId.Value;
            existed.Price = productVM.Price;
            existed.Description = productVM.Description;
            existed.Name = productVM.Name;
            existed.SKU = productVM.SKU;

            _context.Products.Update(existed);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
