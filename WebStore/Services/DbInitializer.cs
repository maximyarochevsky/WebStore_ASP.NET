using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Services.Interfaces;

namespace WebStore.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<DbInitializer> _Logger;

        public DbInitializer(WebStoreDB db, ILogger<DbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
        }

        public async Task<bool> RemoveAsync(CancellationToken Cancel = default)
        {
            _Logger.LogInformation("Удаление БД...");
            var result = await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false);
            if (result)
                _Logger.LogInformation("Удаление БД выполнено успешно");
            else
                _Logger.LogInformation("Удаление БД не требуется (отстутсвует)");
            return result;
        }

        public async Task InitializeAsync(bool RemoveBefore, CancellationToken Cancel)
        {
            _Logger.LogInformation("Инициализация БД...");
            if (RemoveBefore)
                await RemoveAsync(Cancel).ConfigureAwait(false);


            var pending_migrations = await _db.Database.GetPendingMigrationsAsync(Cancel);
            if (pending_migrations.Any())
            {
                _Logger.LogInformation("Выполнение миграции БД...");

                await _db.Database.MigrateAsync(Cancel).ConfigureAwait(false);

                _Logger.LogInformation("Выполнение миграции БД выполнено успешно");
            }
            //_db.Database.EnsureCreatedAsync();
            await _db.Database.MigrateAsync(Cancel).ConfigureAwait(false);

            await InitializeProductAsync(Cancel).ConfigureAwait(false);

            _Logger.LogInformation("Инициализация БД выполненв успешно");
        }

        private async Task InitializeProductAsync(CancellationToken Cancel)
        {
            if(_db.Sections.Any())
            {
                _Logger.LogInformation("Инициализация данных БД не требуется");
                return;
            }

            _Logger.LogInformation("Инициализация тестовых данных БД...");

            _Logger.LogInformation("Добавление секций в БД...");

            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Sections.AddRangeAsync(TestData.Sections, Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF", Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _Logger.LogInformation("Добавление брэндов в БД...");

            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Brands.AddRangeAsync(TestData.Brands, Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF", Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _Logger.LogInformation("Добавление товаров в БД...");

            await using (await _db.Database.BeginTransactionAsync(Cancel))
            {
                await _db.Products.AddRangeAsync(TestData.Products, Cancel);

                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON", Cancel);
                await _db.SaveChangesAsync(Cancel);
                await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF", Cancel);

                await _db.Database.CommitTransactionAsync(Cancel);
            }

            _Logger.LogInformation("Инициализаиця тестовых данных БД выполнена успешно");
        }
    }
}
