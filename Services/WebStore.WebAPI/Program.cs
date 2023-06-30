using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Services.Services;
using WebStore.Services.Services.InCookies;
using WebStore.Services.Services.InSQL;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;    

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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

services.ConfigureApplicationCookie(opt =>
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

services.AddScoped<IProductData, SqlProductData>();
services.AddScoped<IEmployeesData, SqlEmployeeData>();
//services.AddScoped<ICartService, InCookiesCartService>();
services.AddScoped<IOrderService, SqlOrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
