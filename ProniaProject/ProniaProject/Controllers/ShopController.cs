using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.Utils.Exceptions;
using ProniaProject.ViewModels;

namespace ProniaProject.Conrollers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context; 
        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id < 1) throw new BadRequestException();

            Product? product = await _context.Products.
                Include(p=>p.Images.OrderByDescending(pi=>pi.IsPrime)).Include(p=>p.ProductSizes).ThenInclude(ps=>ps.Size)
                .Include(p=>p.Category).Include(p => p.ColorProducts).ThenInclude(p=>p.Color).Include(p=>p.ProductTags).
                ThenInclude(pt=>pt.Tag).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) throw new NotFoundException();

            DetailVM detailVM = new DetailVM
            {
                Product = product,
                RelatedProducts = await _context.Products.Where(p=> p.CategoryId == product.CategoryId && p.Id != id).Include(p=>p.Images.Where(pi=>pi.IsPrime != null))
                .ToListAsync(),

            };
            return View(detailVM);

        }
    }
}
