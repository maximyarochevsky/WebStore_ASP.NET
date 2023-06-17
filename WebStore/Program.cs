using System;

namespace WebStore
{
    internal partial class Program
    {
        public static void Main(string[] args)
        {
            //билдер приложения
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            services.AddControllersWithViews();
            //создание приложения
            var app = builder.Build();
           
            if (app.Environment.IsDevelopment())
            {
            app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
       
            app.MapGet("/throw", () =>
            {
                throw new Exception("Ошибка!");
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}


