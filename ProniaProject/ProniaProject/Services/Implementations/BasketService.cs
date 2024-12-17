using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.Services.Interfaces;
using ProniaProject.ViewModels;
using System.Security.Claims;

namespace ProniaProject.Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;
        private readonly ClaimsPrincipal _user;


        public BasketService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
            _user = http.HttpContext.User;

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

                itemVM = await _context.Products.Where(p => cookieVM.Select(c => c.Id).Contains(p.Id)).Select(p => new BasketItemVM
                {
                          Id = p.Id,
                          Name = p.Name,
                          Image = p.Images[0].Image,
                          Price = p.Price
                         
                }).ToListAsync();

                itemVM.ForEach(bi =>
                {
                    bi.Count = cookieVM.FirstOrDefault(c => c.Id == bi.Id).Count;
                    bi.SubTotal = bi.Count * bi.Count;

                });
                foreach (var item in cookieVM)
                {
                    
                 
                       


                    //if (product != null)
                    //{
                    //    itemVM.Add(new BasketItemVM
                    //    {
                    //        Id = product.Id,
                    //        Name = product.Name,
                    //        Image = product.Images[0].Image,
                    //        Price = product.Price,
                    //        Count = item.Count,
                    //        SubTotal = item.Count * product.Price
                    //    });
                    //}
                }


            }
            return itemVM;
        }
    }
}
