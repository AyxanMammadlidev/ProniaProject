using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;
using ProniaProject.Middlewares;
using ProniaProject.Models;
using ProniaProject.Services.Implementations;
using ProniaProject.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ILayoutService, LayoutService>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
    opt.User.RequireUniqueEmail = true;
    opt.Lockout.MaxFailedAccessAttempts = 3;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
}
).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

//app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseStaticFiles();

app.MapControllerRoute("admin", "{area:exists}/{controller=home}/{action=index}/{id?}");
app.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");


app.Run();




