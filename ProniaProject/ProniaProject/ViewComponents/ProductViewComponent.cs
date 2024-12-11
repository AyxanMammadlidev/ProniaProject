using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.Utils.Enums;

namespace ProniaProject.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(SortType sortType)
        {
            List<Product> products = null;
            switch(sortType)
            {
                case SortType.Name:
                    products = await _context.Products.Take(8)
                        .OrderBy(p => p.Name)
                        .Include(p => p.Images.Where(pi => pi.IsPrime != null)).ToListAsync();
                    break;

                case SortType.Price:
                    products = await _context.Products.Take(8)
                        .OrderByDescending(p => p.Price)
                        .Include(p => p.Images.Where(pi => pi.IsPrime != null)).ToListAsync();
                    break;

                case SortType.Date:
                    products = await _context.Products.Take(8)
                        .OrderByDescending(p => p.CreatedAt)
                        .Include(p => p.Images.Where(pi => pi.IsPrime != null)).ToListAsync();
                    break;
            }

            return View(products);
        }
    }
}
