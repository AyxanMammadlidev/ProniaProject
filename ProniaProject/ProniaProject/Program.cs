using Microsoft.EntityFrameworkCore;
using ProniaProject.DAL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer("server=LAPTOP-04DP7Q7I\\SQLEXPRESS;database=ProniaDB;trusted_connection=true; integrated security=true;TrustServerCertificate=true"));
var app = builder.Build();
app.UseStaticFiles();

app.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");


app.Run();




