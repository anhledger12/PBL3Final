using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PBL3.Models;
using PBL3.Models.Entities;
using System;

namespace PBL3.Data
{
    public class Seed
    {
        public static async Task SeedRoleAndAdmin(IApplicationBuilder init)
        { 
            using (var service = init.ApplicationServices.CreateScope())
            {
                // Tạo ra hai role trong cơ sở dữ liệu, table AspNetRole
                var RoleManager = service.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await RoleManager.RoleExistsAsync(Roles.Admin))
                    await RoleManager.CreateAsync(new IdentityRole(Roles.Admin));
                if (!await RoleManager.RoleExistsAsync(Roles.User))
                    await RoleManager.CreateAsync(new IdentityRole(Roles.User));

                //Seed tài khoản admin luôn
                var UM = service.ServiceProvider.GetRequiredService<UserManager<UserIdentity>>();
                var db = service.ServiceProvider.GetService<LibraryManagementContext>();

                var result = await UM.FindByNameAsync("admin");
                if (result == null)
                {
                    var NewAdmin = new UserIdentity()
                    {
                        UserName = "admin",
                        Email = "sontranviet21@gmail.com"            
                    };
                    await UM.CreateAsync(NewAdmin, "123456");
                    await UM.AddToRoleAsync(NewAdmin, Roles.Admin);
                    //thêm vào tài khoản một thông tin về admin nữa
                    Account DetailAdmin = new Account()
                    {
                        AccName = "admin",
                        Email = "sontranviet21@gmail.com"

                    };
                    db.Accounts.Add(DetailAdmin);
                    db.SaveChanges();
                }

            }
        }
       
    }
}
