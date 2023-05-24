using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PBL3.Common;
using PBL3.Data;
using PBL3.Data.Services;
using PBL3.Models;
using PBL3.Models.Entities;
using System;
using System.Configuration;

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

Global.ConnectionString = builder.Configuration.GetConnectionString("Account");
builder.Services.AddScoped<INotiService, NotiService>();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// Seed Database
await Seed.SeedRoleAndAdmin(app);
await Seed.SeedCategory(app);

//void ConfigureServices(IServiceCollection services)
//{
//    services.AddControllersWithViews();
//    Global.ConnectionString = builder.Configuration.GetConnectionString("Account");
//    services.AddScoped<INotiService, NotiService>();
//}

app.Run();
