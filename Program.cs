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
        var identityConnectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection");
        var generalConnectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_ABrixConnection");
        
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
        //var identityConnectionString = config["ConnectionStrings:DefaultConnection"] ??
                                       //throw new InvalidOperationException("Connection string 'DefaultConnection' not found in user secrets.");

        //var generalConnectionString = config["ConnectionStrings:ABrixConnection"] ??
                                      //throw new InvalidOperationException("Connection string 'ABrixConnection' not found in user secrets.");


        builder.Services.AddAuthentication().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_PROVIDER_AUTHENTICATION_ID");
            googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_PROVIDER_AUTHENTICATION_SECRET");
            googleOptions.CallbackPath = "/Home/Index"; // Specify your custom callback path here

        });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireRole("Admin");
            });
        });

        builder.Services.AddDbContext<AuroraBricksIdentityDbContext>(options =>
            options.UseSqlServer(identityConnectionString));


        builder.Services.AddDbContext<AbrixContext>(options =>
            options.UseSqlServer(generalConnectionString));

        builder.Services.AddScoped<IBrixRepository, EfBrixRepository>();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromDays(60);
            options.ExcludedHosts.Add("example.com");
        });


        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
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

        app.Use(async (ctx, next) =>
        {
            ctx.Response.Headers.Add("Content-Security-Policy",
            "default-src 'self'");
            await next();
        });


        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSession();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] { "Admin", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager =
                scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string email = "admin@admin.com";
            string password = "Test123!";
            string email1 = "elizabethswannlover@gmail.com";
            string password1 = "Sparrow123!";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Admin");
            }
            if (await userManager.FindByEmailAsync(email1) == null)
            {
                var user = new IdentityUser();
                user.UserName = email1;
                user.Email = email1;

                await userManager.CreateAsync(user, password1);

                await userManager.AddToRoleAsync(user, "Customer");
            }

        }
        app.Run();
    }
}