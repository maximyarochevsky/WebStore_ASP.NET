using System;

namespace WebStore
{
    internal partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var services = builder.Services;
            services.AddControllersWithViews();
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

            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}


