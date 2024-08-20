using Microsoft.EntityFrameworkCore;
using ProductDemo.Interface;
using ProductDemo.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ProductDB>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProConnection"));
});

builder.Services.AddHttpContextAccessor();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentFailureService, PaymentFailureService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseDeveloperExceptionPage();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
