using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PBL3.Data;
using PBL3.Models;
using PBL3.Models.Entities;
using System;

var builder = WebApplication.CreateBuilder(args);

//Add Service
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibraryManagementContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("Account")));
builder.Services.AddScoped<QL>();
builder.Services.AddIdentity<UserIdentity, IdentityRole>(opts =>
{
    opts.User.RequireUniqueEmail= true;
    opts.Password.RequiredLength = 5;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
}).AddEntityFrameworkStores<LibraryManagementContext>().
AddTokenProvider<DataProtectorTokenProvider<UserIdentity>>(TokenOptions.DefaultProvider);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();//????????????????????????????????????????????????????????????????????????????
 // phải add authentication trước authorization?????????????????????????????????????????
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
await Seed.SeedRoleAndAdmin(app);
// Hàm seed vai trò và tk admin


app.Run();
