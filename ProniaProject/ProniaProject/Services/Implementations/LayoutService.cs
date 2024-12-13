using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.Services.Interfaces;
using ProniaProject.ViewModels;
using System.Security.Claims;

namespace ProniaProject.Services.Implementations
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;
        

        public LayoutService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
            
        }

        public async Task<List<BasketItemVM>> GetBasketAsync()
        {
            List<BasketItemVM> itemVM = new();
            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                itemVM = await _context.BasketItems.Where(bi => bi.AppUserId == _http.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))

               .Select(

                    bi => new BasketItemVM
                    {
                        Count = bi.Count,
                        Image = bi.Product.Images.FirstOrDefault(pi => pi.IsPrime == true).Image,
                        Name = bi.Product.Name,
                        Price = bi.Product.Price,
                        SubTotal = bi.Count * bi.Product.Price,
                        Id = bi.Id

                    }
                    ).ToListAsync();


            }

            else
            {

                List<BasketCookieItemVM> cookieVM;
                string cookie = _http.HttpContext.Request.Cookies["basket"];


                if (cookie is null) return itemVM;


                else
                {

                }
                cookieVM = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookie);


                foreach (var item in cookieVM)
                {
                    Product product = await _context.Products
                        .Include(p => p.Images.Where(pi => pi.IsPrime == true))
                        .FirstOrDefaultAsync(p => p.Id == item.Id);


                    if (product != null)
                    {
                        itemVM.Add(new BasketItemVM
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Image = product.Images[0].Image,
                            Price = product.Price,
                            Count = item.Count,
                            SubTotal = item.Count * product.Price
                        });
                    }
                }


            }
                return itemVM;

        }

        public async Task<Dictionary<string,string>> GetSettingAsync()
        {
            Dictionary<string,string> settings = await _context.Settings.ToDictionaryAsync(s=>s.Key, s=>s.Value);

            return settings;
        }
    }
}
