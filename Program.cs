using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuroraBricks.Data;
using AuroraBricks.Areas.Identity.Data;
using AuroraBricks.Models;

internal class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var configuration = builder.Configuration;

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
        // Add services to the container.
        var identityConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<AuroraBricksIdentityDbContext>(options =>
            options.UseSqlite(identityConnectionString));

        var generalConnectionString = builder.Configuration.GetConnectionString("ABrixConnection") ??
                                      throw new InvalidOperationException("Connection string 'ABrixConnection' not found.");
        builder.Services.AddDbContext<AbrixContext>(options =>
            options.UseSqlite(generalConnectionString));

        builder.Services.AddScoped<IBrixRepository, EfBrixRepository>();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();




        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuroraBricksIdentityDbContext>();
        builder.Services.AddControllersWithViews();


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