using GadgetsOnline.Models;
using GadgetsOnline.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")
});

// Add services to the container
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GadgetsOnlineEntities>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString(nameof(GadgetsOnlineEntities)));
});

// Seed data
using (var context = new GadgetsOnlineEntities(builder.Configuration.GetConnectionString(nameof(GadgetsOnlineEntities))))
{
    context.Database.EnsureCreated();
    context.SaveChanges();
}

builder.Services.AddScoped<IInventory, Inventory>();
builder.Services.AddScoped<IShoppingCart, ShoppingCart>();
builder.Services.AddScoped<IOrderProcessing, OrderProcessing>();

// Store configuration for legacy access
ConfigurationManager.Configuration = builder.Configuration;

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public class ConfigurationManager
{
    public static IConfiguration Configuration { get; set; }
}
