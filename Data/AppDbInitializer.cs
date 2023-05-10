using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using officehours_management.Data.Static;
using officehours_management.Models;

namespace officehours_management.Data
{
    public class AppDbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder ApplicationBuilder)
        {
            using(var ServiceScope = ApplicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = ServiceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if(! await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                
                if(! await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if(! await roleManager.RoleExistsAsync(UserRoles.SuperUser))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.SuperUser));
                
                if(! await roleManager.RoleExistsAsync(UserRoles.Student))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Student));
                
                if(! await roleManager.RoleExistsAsync(UserRoles.TeacherAssistant))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.TeacherAssistant));

                if(! await roleManager.RoleExistsAsync(UserRoles.Doctor))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor));

                var userManager = ServiceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string AdminUserEmail = "admin@staff.com";

                var adminUser = await userManager.FindByEmailAsync(AdminUserEmail);

                if(adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FullName = "Admin User",
                        UserName = "admin-user",
                        is_active = true,
                        is_staff = true,
                        Email = AdminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                var superUser = userManager.Users.Where(u => u.is_superuser == true).FirstOrDefault();
                if(superUser != null)
                    if(!await userManager.IsInRoleAsync(superUser, UserRoles.SuperUser))
                        await userManager.AddToRoleAsync(superUser, UserRoles.SuperUser);
            }
        }
    }
}