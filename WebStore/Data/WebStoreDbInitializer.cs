﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        readonly WebStoreDB _db;
        readonly UserManager<User> _userManager;
        readonly RoleManager<Role> _roleManager;
        readonly ILogger<WebStoreDbInitializer> _logger;

        public WebStoreDbInitializer(
            WebStoreDB db,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<WebStoreDbInitializer> logger)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
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
                _logger.LogError(e, "Ошибка при заполнении БД данными из TestData.cs");
                throw;
            }

            try
            {
                InitializeIdentityAsync().Wait();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Ошибка при инициализации БД системы Identity");
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

        private async Task InitializeIdentityAsync()
        {
            async Task CheckRole(string RoleName)
            {
                if (!await _roleManager.RoleExistsAsync(RoleName))
                    await _roleManager.CreateAsync(new Role { Name = RoleName });
            }

            await CheckRole(Role.Administrator);
            await CheckRole(Role.User);

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User
                {
                    UserName = User.Administrator
                };
                var creation_result = await _userManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                    await _userManager.AddToRoleAsync(admin, Role.Administrator);
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"Ошибка при создании учётно записи администратора {string.Join(",", errors)}");
                }
            }
        }
    }
}
