using Microsoft.EntityFrameworkCore;
using TallerMecanicoCore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext <TallerContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"))

);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
                            name: "create_presupuesto",
                            pattern: "Home/CreatePresupuesto/{id}",
                            defaults: new { controller = "Home", action = "CreatePresupuesto" });

app.Run();