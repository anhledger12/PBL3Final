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
        public static async Task SeedHashtag(IApplicationBuilder init)
        {
            using (var service = init.ApplicationServices.CreateScope())
            {
                var db = service.ServiceProvider.GetService<LibraryManagementContext>();
                if (db.Hashtags.Count() != 0) return;
                List<Hashtag> hashtags = new List<Hashtag>{
                    new Hashtag () {NameHashTag = "Giáo khoa"},
                    new Hashtag () {NameHashTag = "Tài liệu tham khảo"},
                    new Hashtag () {NameHashTag = "Tiểu thuyết"},
                    new Hashtag () {NameHashTag = "Truyện ngắn"},
                    new Hashtag () {NameHashTag = "Kinh doanh và tài chính"},
                    new Hashtag () {NameHashTag = "Khoa học viễn tưởng"},
                    new Hashtag () {NameHashTag = "Huyền bí và siêu nhiên"},
                    new Hashtag () {NameHashTag = "Hài hước"},
                    new Hashtag () {NameHashTag = "Lãng mạn"},
                    new Hashtag () {NameHashTag = "Phiêu lưu"},
                    new Hashtag () {NameHashTag = "Kỹ năng sống"},
                    new Hashtag () {NameHashTag = "Kỹ năng quản lý và phát triển bản thân"},
                    new Hashtag () {NameHashTag = "Lịch sử"},
                    new Hashtag () {NameHashTag = "Tâm lý học"},
                    new Hashtag () {NameHashTag = "Chính trị và xã hội"},
                    new Hashtag () {NameHashTag = "Khoa học và tự nhiên"},
                    new Hashtag () {NameHashTag = "Văn học cổ điển"},
                    new Hashtag () {NameHashTag = "Văn học hiện đại"},
                    new Hashtag () {NameHashTag = "Thể thao và thể dục"},
                    new Hashtag () {NameHashTag = "Du lịch và phiêu lưu"},
                    new Hashtag () {NameHashTag = "Nấu ăn và ẩm thực"},
                    new Hashtag () {NameHashTag = "Y học và sức khỏe"}
                };
                db.Hashtags.AddRange(hashtags);
                await db.SaveChangesAsync();
            }
        }
    }
}
