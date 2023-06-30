using System;
using WebStore.Infastructure.Conventions;
using WebStore.Infastructure.Middleware;
using WebStore.Services;
using WebStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Interfaces.Services;
using WebStore.Interfaces.TestAPI;
using WebStore.Services.Services;
using WebStore.Services.Services.InCookies;
using WebStore.Services.Services.InSQL;
using WebStore.WebAPI.Clients.Employees;
using WebStore.WebAPI.Clients.Orders;
using WebStore.WebAPI.Clients.Products;
using WebStore.WebAPI.Clients.Values;

namespace WebStore
{
    internal partial class Program
    {
        public static async Task Main(string[] args)
        {
            //билдер приложения
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            services.AddControllersWithViews(opt =>
            {
                opt.Conventions.Add(new TestConvention());
            });

            services.AddDbContext<WebStoreDB>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));
            services.AddTransient<IDbInitializer, DbInitializer>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
            #if DEBUG
                opt.Password.RequireDigit = false; //необязательные цифры
                opt.Password.RequireLowercase = false; //необязательный нижиний регистр
                opt.Password.RequireUppercase = false; //необязательные врехний регистр
                opt.Password.RequireNonAlphanumeric = false; //необязательные неалфавитные символы
                opt.Password.RequiredLength = 3; //необязательные неалфавитные символы
                opt.Password.RequiredUniqueChars = 3; //необязательные неалфавитные символы
            #endif

                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.ConfigureApplicationCookie(opt=>
            {
                opt.Cookie.Name = "WebStore.GB";
                opt.Cookie.HttpOnly = true;

                opt.ExpireTimeSpan = TimeSpan.FromDays(10);
				//opt.Cookie.Expiration = TimeSpan.FromDays(10); устарело

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            //services.AddSingleton<IProductData, InMemoryProductData>();

            //services.AddScoped<IProductData, SqlProductData>();
            //services.AddScoped<IEmployeesData, SqlEmployeeData>();
            services.AddScoped<ICartService, InCookiesCartService>();
            //services.AddScoped<IOrderService, SqlOrderService>();

            var configuration = builder.Configuration;
            services.AddHttpClient<IValuesService, ValuesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
            services.AddHttpClient<IEmployeesData, EmployeesClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
            services.AddHttpClient<IProductData, ProductsClient>(client => client.BaseAddress = new(configuration["WebAPI"]));
            services.AddHttpClient<IOrderService, OrdersClient>(client => client.BaseAddress = new(configuration["WebAPI"]));

            //создание приложения
            var app = builder.Build();

            await using (var scope = app.Services.CreateAsyncScope())
            {
                var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await db_initializer.InitializeAsync(RemoveBefore: true).ConfigureAwait(true);
            }

                if (app.Environment.IsDevelopment())
                    app.UseDeveloperExceptionPage();
          

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<TestMiddleware>();

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

			//app.MapDefaultControllerRoute();

            app.Run();
        }
    }
}


