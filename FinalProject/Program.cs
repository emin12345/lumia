using FinalProject.DAL;
using FinalProject.Entities;
using FinalProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LumiaDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});





builder.Services.AddScoped<LayoutService>();

builder.Services.AddHttpContextAccessor();




builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.SignIn.RequireConfirmedAccount = false;
    opt.SignIn.RequireConfirmedEmail = true;
    opt.Password.RequiredUniqueChars = 4;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequiredLength = 6;
    opt.Password.RequireDigit = true;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = false;


    opt.User.RequireUniqueEmail = false;

    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_?><:~";

    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

}).AddDefaultTokenProviders().AddEntityFrameworkStores<LumiaDbContext>();

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

app.UseAuthentication();
app.UseAuthorization();
//app.UseSession();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

});

app.Run();