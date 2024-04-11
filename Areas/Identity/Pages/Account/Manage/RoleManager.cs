namespace AuroraBricks.Areas.Identity.Pages.Account.Manage;
using Microsoft.AspNetCore.Identity;

public class RoleManager
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleManager(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task AssignRoleToUser(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID '{userId}' not found.");
        }

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            throw new ArgumentException($"Role '{roleName}' not found.");
        }

        await _userManager.AddToRoleAsync(user, roleName);
    }
}