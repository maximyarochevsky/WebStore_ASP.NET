using System;
using WebStore.Infastructure.Conventions;
using WebStore.Infastructure.Middleware;
using WebStore.Services.Interfaces;
using WebStore.Services;
using WebStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;

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

            //services.AddSingleton<IProductData, InMemoryProductData>();
            services.AddScoped<IProductData, SqlProductData>();

            //создание приложения
            var app = builder.Build();

            await using (var scope = app.Services.CreateAsyncScope())
            {
                var db_initializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await db_initializer.InitializeAsync(RemoveBefore: true);
            }

                if (app.Environment.IsDevelopment())
                    app.UseDeveloperExceptionPage();
          

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<TestMiddleware>();

            //app.MapDefaultControllerRoute();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}


