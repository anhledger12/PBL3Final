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
                if (!await RoleManager.RoleExistsAsync(UserRole.Admin))
                    await RoleManager.CreateAsync(new IdentityRole(UserRole.Admin));
                if (!await RoleManager.RoleExistsAsync(UserRole.User))
                    await RoleManager.CreateAsync(new IdentityRole(UserRole.User));
                if(!await RoleManager.RoleExistsAsync(UserRole.Staff))
                    await RoleManager.CreateAsync(new IdentityRole(UserRole.Staff));
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
                    await UM.AddToRoleAsync(NewAdmin, UserRole.Admin);
                    //thêm vào tài khoản một thông tin về admin nữa
                    Account DetailAdmin = new Account()
                    {
                        AccName = "admin",
                        Email = "sontranviet21@gmail.com"

                    };
                    db.Accounts.Add(DetailAdmin);
                    db.SaveChanges();
                }

                var result2 = await UM.FindByNameAsync("staff1");
                if (result2 == null)
                {
                    var NewStaff = new UserIdentity()
                    {
                        UserName = "staff1",
                        Email = "staff1@gmail.com"
                    };
                    await UM.CreateAsync(NewStaff, "123456");
                    await UM.AddToRoleAsync(NewStaff, UserRole.Staff);
                    //thêm vào tài khoản một thông tin về admin nữa
                    Account DetailStaff = new Account()
                    {
                        AccName = "staff1",
                        Email = "staff1@gmail.com"

                    };
                    db.Accounts.Add(DetailStaff);
                    db.SaveChanges();
                }

            }
        }
       
    }
}
