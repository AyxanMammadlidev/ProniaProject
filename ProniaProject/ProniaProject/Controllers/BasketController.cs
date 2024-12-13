using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.ViewModels;
using System.Security.Claims;

namespace ProniaProject.Controllers
{

    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> itemVM = new();

            if (User.Identity.IsAuthenticated)
            {
                itemVM = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .Where(u => u.Id == User
                    .FindFirstValue(ClaimTypes.NameIdentifier))
                    .SelectMany(u => u.BasketItems).Select(

                    bi => new BasketItemVM
                    {
                        Count = bi.Count,
                        Image = bi.Product.Images.FirstOrDefault(pi=>pi.IsPrime==true).Image,
                        Name = bi.Product.Name,
                        Price = bi.Product.Price,
                        SubTotal = bi.Count * bi.Product.Price,
                        Id = bi.Id

                    }
                    ).ToListAsync();
                
                    
            }

            else
            {
                string cookie = Request.Cookies["basket"];
                
                List<BasketCookieItemVM> cookieVM;

                if (cookie is null) return View(itemVM);


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
           


            return View(itemVM);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            List<BasketCookieItemVM> basket;


            if (id is null || id < 1) return BadRequest();


            bool result = await _context.Products.AnyAsync(p => p.Id == id);

            if (!result) return NotFound();


            if (User.Identity.IsAuthenticated)
            {
                AppUser? user = await _userManager.Users.Include(u => u.BasketItems).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

               BasketItem item = user.BasketItems.FirstOrDefault(bi=>bi.ProductId==id);

              if (item is null)
                {
                    user.BasketItems.Add(new BasketItem {
                    
                        ProductId = id.Value,
                        Count = 1                  
                    
                    });
                }
                else
                {
                    item.Count++;
                }

              await _context.SaveChangesAsync();
            }

            else
            {
                string cookies = Request.Cookies["basket"];
                if (cookies is not null)
                {
                    basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookies);


                    BasketCookieItemVM existed = basket.FirstOrDefault(p => p.Id == id);
                    if (existed != null)
                    {
                        existed.Count++;
                    }
                    else
                    {

                        basket.Add(new BasketCookieItemVM()
                        {
                            Id = id.Value,
                            Count = 1
                        });
                    }
                }
                else
                {

                    basket = new List<BasketCookieItemVM>
            {
                new BasketCookieItemVM()
                {
                    Id = id.Value,
                    Count = 1
                }
            };
                }


                string basketJson = JsonConvert.SerializeObject(basket);
                Response.Cookies.Append("basket", basketJson);

            }

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult GetBasket()
        {
            var basket = Request.Cookies["basket"];
            return Content(basket);
        }

        public IActionResult RemoveItemFromBasket(int? id)
        {
           
            List<BasketCookieItemVM> basket;

            string cookies = Request.Cookies["basket"];

            if(cookies is null)  return NotFound();

            basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookies);

            if(basket is null) return NotFound();

            BasketCookieItemVM? removeItem = basket.FirstOrDefault(p => p.Id == id);

            basket.Remove(removeItem);

            string updatedBasket = JsonConvert.SerializeObject(basket);

            Response.Cookies.Append("basket", updatedBasket);

            return RedirectToAction(nameof(Index));

        }

        //public IActionResult IncreaseItemQuantity(int? id)
        //{
        //    List<BasketCookieItemVM> basket;

        //    string cookies = Request.Cookies["basket"];

        //    basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookies);

        //    if (basket is null) return RedirectToAction(nameof(Index));

        //    BasketCookieItemVM item = basket.FirstOrDefault(i => i.Id == id);

        //    item.Count++;

        //    string updatedBasket = JsonConvert.SerializeObject(basket);

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
