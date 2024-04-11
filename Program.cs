using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuroraBricks.Data;
using AuroraBricks.Areas.Identity.Data;
using AuroraBricks.Models;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Google:ClientId"];
    googleOptions.ClientSecret = configuration["Google:ClientSecret"];
});
//Add services to the container.
var identityConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'app' not found.");
builder.Services.AddDbContext<AuroraBricksIdentityDbContext>(options =>
    options.UseSqlite(identityConnectionString));

var generalConnectionString = builder.Configuration.GetConnectionString("ABrixConnection") ??
                              throw new InvalidOperationException("Connection string 'ABrixConnection' not found.");
builder.Services.AddDbContext<AbrixContext>(options =>
    options.UseSqlite(generalConnectionString));

builder.Services.AddScoped<IBrixRepository, EfBrixRepository>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AuroraBricksIdentityDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, 
    HttpContextAccessor>();

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();