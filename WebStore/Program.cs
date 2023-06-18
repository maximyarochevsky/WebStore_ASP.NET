using System;
using WebStore.Infastructure.Conventions;
using WebStore.Infastructure.Middleware;

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

            
            //создание приложения
            var app = builder.Build();
           
            if (app.Environment.IsDevelopment())
            app.UseDeveloperExceptionPage();
          

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<TestMiddleware>();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}


