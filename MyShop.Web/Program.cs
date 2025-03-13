using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Data;
using MyShop.DataAccess.Implementaions;
using MyShop.Entities.IRepositories;
using MyShop.Entities.Models;
using Stripe;
using Utilities;

namespace MyShop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // تسجيل DbContext أولًا
            builder.Services.AddDbContext<AppsDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



            // تسجيل الهوية Identity (مرة واحدة فقط)
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppsDbContext>().AddDefaultUI().
AddDefaultTokenProviders();
            builder.Services.AddScoped<UserManager<ApplicationUser>>();
            builder.Services.AddScoped<IShoppingCardRepository, ShoppingCardRepository>();
            builder.Services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();

            // تسجيل الـ UnitOfWork بعد DbContext
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.Configure<StripeData>(builder.Configuration.GetSection("stripe"));
            // تسجيل الخدمات الأخرى
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();
            app.UseAuthentication(); // تأكد من إضافة هذا السطر لتفعيل Authentication
            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "customer",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");


            app.Run();
        }
    }
}
