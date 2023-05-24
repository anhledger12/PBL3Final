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
                    Account DetailAdmin = new Account()
                    {
                        AccName = "admin",
                        Email = "sontranviet21@gmail.com"

                    };
                    db.Accounts.Add(DetailAdmin);
                    db.SaveChanges();
                    var NewAdmin = new UserIdentity()
                    {
                        UserName = "admin",
                        Email = "sontranviet21@gmail.com"            
                    };
                    await UM.CreateAsync(NewAdmin, "123456");
                    await UM.AddToRoleAsync(NewAdmin, UserRole.Admin);
                    //thêm vào tài khoản một thông tin về admin nữa
                }

                var result2 = db.Accounts.Where(p=>p.AccName=="staff1").FirstOrDefault();
                if (result2 == null)
                {
                    Account DetailStaff = new Account()
                    {
                        AccName = "staff1",
                        Email = "staff1@gmail.com"

                    };
                    db.Accounts.Add(DetailStaff);
                    db.SaveChanges();
                    var NewStaff = new UserIdentity()
                    {
                        UserName = "staff1",
                        Email = "staff1@gmail.com"
                    };
                    await UM.CreateAsync(NewStaff, "123456");
                    await UM.AddToRoleAsync(NewStaff, UserRole.Staff);
                    //thêm vào tài khoản một thông tin về admin nữa
                }

            }
        }
        public static async Task SeedCategory(IApplicationBuilder init)
        {
            using (var service = init.ApplicationServices.CreateScope())
            {
                
                var db = service.ServiceProvider.GetService<LibraryManagementContext>();
                if (db.Categories.Count() != 0) return;
                List<Category> Categorys = new List<Category>{
                    new Category () {NameCategory = "Giáo khoa"},
                    new Category () {NameCategory = "Tài liệu tham khảo"},
                    new Category () {NameCategory = "Tiểu thuyết"},
                    new Category () {NameCategory = "Truyện ngắn"},
                    new Category () {NameCategory = "Kinh doanh và tài chính"},
                    new Category () {NameCategory = "Khoa học viễn tưởng"},
                    new Category () {NameCategory = "Huyền bí và siêu nhiên"},
                    new Category () {NameCategory = "Hài hước"},
                    new Category () {NameCategory = "Lãng mạn"},
                    new Category () {NameCategory = "Phiêu lưu"},
                    new Category () {NameCategory = "Kỹ năng sống"},
                    new Category () {NameCategory = "Kỹ năng quản lý và phát triển bản thân"},
                    new Category () {NameCategory = "Lịch sử"},
                    new Category () {NameCategory = "Tâm lý học"},
                    new Category () {NameCategory = "Chính trị và xã hội"},
                    new Category () {NameCategory = "Khoa học và tự nhiên"},
                    new Category () {NameCategory = "Văn học cổ điển"},
                    new Category () {NameCategory = "Văn học hiện đại"},
                    new Category () {NameCategory = "Thể thao và thể dục"},
                    new Category () {NameCategory = "Du lịch và phiêu lưu"},
                    new Category () {NameCategory = "Nấu ăn và ẩm thực"},
                    new Category () {NameCategory = "Y học và sức khỏe"}
                };
                db.Categories.AddRange(Categorys);
                await db.SaveChangesAsync();
                
            }
        }
    }
}
