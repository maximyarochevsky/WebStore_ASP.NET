using System;
using WebStore.Infastructure.Conventions;
using WebStore.Infastructure.Middleware;
using WebStore.Services.Interfaces;
using WebStore.Services;
using WebStore.DAL.Context;
using Microsoft.EntityFrameworkCore;
using WebStore.Services.InMemory;
using WebStore.Services.InSQL;
using WebStore.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace WebStore
{
    internal partial class Program
    {
        public static async Task Main(string[] args)
        {
            //������ ����������
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
                opt.Password.RequireDigit = false; //�������������� �����
                opt.Password.RequireLowercase = false; //�������������� ������� �������
                opt.Password.RequireUppercase = false; //�������������� ������� �������
                opt.Password.RequireNonAlphanumeric = false; //�������������� ������������ �������
                opt.Password.RequiredLength = 3; //�������������� ������������ �������
                opt.Password.RequiredUniqueChars = 3; //�������������� ������������ �������
            #endif

                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890";
            });

            //services.AddSingleton<IProductData, InMemoryProductData>();
            services.AddScoped<IProductData, SqlProductData>();
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();

            //�������� ����������
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


