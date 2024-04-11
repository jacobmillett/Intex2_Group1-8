using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuroraBricks.Data;
using AuroraBricks.Areas.Identity.Data;
using AuroraBricks.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

internal class Program
{
    public static async Task Main(string[] args)
    {
        // When we deploy, uncomment
        //var identityConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:DefaultConnection");
        //var generalConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings:ABrixConnection");
        
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;
        var env = builder.Environment;
        
        // Load user secrets
        var config = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>()
            .Build();

        //Add services to the container.
        var identityConnectionString = config["ConnectionStrings:DefaultConnection"] ??
                                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found in user secrets.");

        var generalConnectionString = config["ConnectionStrings:ABrixConnection"] ??
                                      throw new InvalidOperationException("Connection string 'ABrixConnection' not found in user secrets.");


        builder.Services.AddAuthentication().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireRole("Admin");
            });
        });

        builder.Services.AddDbContext<AuroraBricksIdentityDbContext>(options =>
            options.UseSqlite(identityConnectionString));


        builder.Services.AddDbContext<AbrixContext>(options =>
            options.UseSqlite(generalConnectionString));

        builder.Services.AddScoped<IBrixRepository, EfBrixRepository>();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(60);
            options.ExcludedHosts.Add("example.com");
        });


        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuroraBricksIdentityDbContext>();
        builder.Services.AddControllersWithViews();

        builder.Services.AddRazorPages();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession();
        builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
        builder.Services.AddSingleton<IHttpContextAccessor, 
            HttpContextAccessor>();

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

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager =
                scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }


        }
        app.Run();
    }
}