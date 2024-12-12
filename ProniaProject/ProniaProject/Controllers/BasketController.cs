using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.ViewModels;

namespace ProniaProject.Controllers
{

    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<BasketCookieItemVM> cookieVM;
            string cookie = Request.Cookies["basket"];
            List<BasketItemVM> itemVM = new();


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


            return View(itemVM);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            List<BasketCookieItemVM> basket;


            if (id is null || id < 1) return BadRequest();


            bool result = await _context.Products.AnyAsync(p => p.Id == id);

            if (!result) return NotFound();


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

            basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookies);

            BasketCookieItemVM removeItem = basket.FirstOrDefault(p => p.Id == id);

            basket.Remove(removeItem);

            string updatedBasket = JsonConvert.SerializeObject(basket);

            Response.Cookies.Append("basket", updatedBasket);

            return RedirectToAction(nameof(Index), "Basket");

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
