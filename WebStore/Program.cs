using System;
using WebStore.Infastructure.Conventions;
using WebStore.Infastructure.Middleware;
using WebStore.Services.Interfaces;
using WebStore.Services;
using WebStore.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace WebStore
{
    internal partial class Program
    {
        public static void Main(string[] args)
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

            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();
            services.AddSingleton<IProductData, InMemoryProductData>();
            
            //создание приложения
            var app = builder.Build();
           
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


