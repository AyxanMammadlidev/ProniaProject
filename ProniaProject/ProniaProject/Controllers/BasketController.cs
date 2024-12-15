using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using ProniaProject.DAL;
using ProniaProject.Models;
using ProniaProject.Services.Interfaces;
using ProniaProject.ViewModels;
using System.Security.Claims;

namespace ProniaProject.Controllers
{

    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IBasketService _basketService;

        public BasketController(AppDbContext context, IBasketService basketService, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _basketService = basketService;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _basketService.GetBasketAsync());
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

            return RedirectToAction(nameof(GetBasket));
            
        }



        public async Task<IActionResult> GetBasket()
        {
            return PartialView("BasketPartialView", await _basketService.GetBasketAsync());
        }



      
        public async Task<IActionResult> RemoveItemFromBasket(int? id)
        {
            if(id is null || id<1) return BadRequest();

            List<BasketCookieItemVM> basket = new();

            if (User.Identity.IsAuthenticated)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                AppUser? user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user is null) return NotFound(); 
   
                BasketItem? basketItem = user.BasketItems.FirstOrDefault(bi => bi.Id == id);

                if (basketItem is null) return NotFound();
               
                user.BasketItems.Remove(basketItem);
    
                await _context.SaveChangesAsync();
            }

            else
            {
                string cookies = Request.Cookies["basket"];

                if (cookies is null) return BadRequest();

                basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookies);

                if (basket is null) return BadRequest();

                BasketCookieItemVM? removeItem = basket.FirstOrDefault(p => p.Id == id);

                if (removeItem is null) return NotFound();

                basket.Remove(removeItem);

                string updatedBasket = JsonConvert.SerializeObject(basket);

                Response.Cookies.Append("basket", updatedBasket);
            }

            return RedirectToAction(nameof(Index));

        }




        [HttpPost]
        public async Task<IActionResult> IncreaseItemQuantity(int? id)
        {
            if (id is null || id<1) return BadRequest();

            List<BasketCookieItemVM>? basket = new();

            if (User.Identity.IsAuthenticated)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                AppUser? user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user is null) return NotFound();

                BasketItem? basketItem = user.BasketItems.FirstOrDefault(bi => bi.Id == id);

                if (basketItem is not null)
                {
                    basketItem.Count++;
                    await _context.SaveChangesAsync();
                }

            }
            else
            {
                string? cookies = Request.Cookies["basket"];

                if (string.IsNullOrEmpty(cookies)) return RedirectToAction(nameof(Index));

                basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookies);

                if (basket == null) return RedirectToAction(nameof(Index));

                BasketCookieItemVM? item = basket.FirstOrDefault(i => i.Id == id);

                if (item != null)
                {
                    item.Count++;

                    string updatedBasket = JsonConvert.SerializeObject(basket);

                    Response.Cookies.Append("basket", updatedBasket);
                }
                else
                {
                    return NotFound("Basket item not found in cookies.");
                }
            }

            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        public async Task<IActionResult> DecraseItemQuantity(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            List<BasketCookieItemVM> basket = new();

            if (User.Identity.IsAuthenticated)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                AppUser? user = await _userManager.Users
                    .Include(u => u.BasketItems)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user is null) return NotFound();

                BasketItem? basketItem = user.BasketItems.FirstOrDefault(bi => bi.Id == id);

                if (basketItem is null) return NotFound();

                if(basketItem.Count > 1)
                {
                    basketItem.Count--;
                    
                }
                else
                {               
                    user.BasketItems.Remove(basketItem);
                }

                await _context.SaveChangesAsync();


            }

            else
            {
                string cookies = Request.Cookies["basket"];

                basket = JsonConvert.DeserializeObject<List<BasketCookieItemVM>>(cookies);

                if (basket is null) return RedirectToAction(nameof(Index));

                BasketCookieItemVM item = basket.FirstOrDefault(i => i.Id == id);

                if (item.Count > 1)
                {
                    item.Count--;
                }
                else
                {
                    basket.Remove(item);
                }

                string updatedBasket = JsonConvert.SerializeObject(basket);

                Response.Cookies.Append("basket", updatedBasket);
            }

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Checkout()
        {
            OrderVM orderVM = new()
            {
                BasketOrderVMs = await _context.BasketItems
                .Where(bi => bi.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Select(bi=>new BasketOrderVM
                {
                    Count = bi.Count,
                    Name = bi.Product.Name,
                    Price = bi.Product.Price,
                    SubTotal = bi.Product.Price * bi.Count
                }).ToListAsync()
            };
            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderVM orderVM)
        {
            var basketItems = await _context.BasketItems
                 .Where(bi => bi.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                 .Include(bi=>bi.Product)
             .ToListAsync();

            if (!ModelState.IsValid)
            {
                orderVM.BasketOrderVMs = basketItems.Select(bi => new BasketOrderVM
                {
                    Count = bi.Count,
                    Name = bi.Product.Name,
                    Price = bi.Product.Price,
                    SubTotal = bi.Product.Price * bi.Count
                }).ToList();
                return View(orderVM);
            }

            Order order = new()
            {
                Adress = orderVM.Adress,
                Status = null,
                AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                Items = basketItems.Select(bi => new OrderItem
                {
                    AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Count = bi.Count,
                    Price = bi.Product.Price,
                    ProductId = bi.Product.Id



                }).ToList(),
                TotalPrice = basketItems.Sum(bi=>bi.Product.Price * bi.Count)
            };

            await _context.Order.AddAsync(order);
            _context.BasketItems.RemoveRange(basketItems);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index),"Home");





            


        }
    }
}
