using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        readonly WebStoreDB _db;
        readonly ILogger<WebStoreDbInitializer> _logger;

        public WebStoreDbInitializer(WebStoreDB db, ILogger<WebStoreDbInitializer> logger)
        {
            _db = db;
            _logger = logger;
        }

        public void Initialize()
        {
            _logger.LogInformation("Инициализация БД...");

            var db = _db.Database;

            if (db.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Есть неприменённные миграции...");
                db.Migrate();
                _logger.LogInformation("Миграции БД выполнены успешно");
            }
            else
                _logger.LogInformation("Структура БД в актуальном состоянии");

            try
            {
                InitializeProducts();
            }
            catch (Exception e)
            {
                _logger.LogError("Ошибка при заполнении БД данными из TestData.cs");
                throw;
            }
        }

        void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();

            if (_db.Products.Any())
            {
                _logger.LogInformation("Добавление исходных данных в БД не требуется");
                return;
            }

            _logger.LogInformation("Добавление категорий товаров {0} мс...", timer.ElapsedMilliseconds);
            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _db.Database.CommitTransaction();
            }

            _logger.LogInformation("Добавление брендов...");
            using (_db.Database.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                _db.Database.CommitTransaction();
            }

            _logger.LogInformation("Добавление товаров...");
            using (_db.Database.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                _db.Database.CommitTransaction();
            }

            _logger.LogInformation("Добавление исходных данных выполнено успешно за {0} мс...", timer.ElapsedMilliseconds);
        }
    }
}
