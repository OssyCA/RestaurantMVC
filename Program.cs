using Microsoft.AspNetCore.Authentication.Cookies;
using RestaurantMVC.Helper;

using RestaurantMVC.Services;
using RestaurantMVC.Services.Iservices;
using System.Net;

namespace RestaurantMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddTransient<JwtTokenHandler>();

            builder.Services.AddHttpClient("RestaurantAPI", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7099/api/");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true
            })
            .AddHttpMessageHandler<JwtTokenHandler>(); 

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = "/Employee/Login";
                });

            builder.Services.AddAuthorization();
            builder.Services.AddScoped<IGetMenu, GetMenu>();
            builder.Services.AddScoped<IGetTables,GetTables>(); 
            builder.Services.AddScoped<IGetBookings,GetBookings>(); 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=index}/{id?}");

            app.Run();
        }
    }
}