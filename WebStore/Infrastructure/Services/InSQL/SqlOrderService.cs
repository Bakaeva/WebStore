using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SqlOrderService : IOrderService
    {
        readonly WebStoreDB _db;
        readonly UserManager<User> _userManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> userManager) 
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userName) => await _db.Orders
            .Where(order => order.User.UserName == userName)
            .Include(order => order.User)
            .Include(order => order.Items)
            .ToArrayAsync();
        
        public async Task<Order> GetOrderById(int id) => await _db.Orders
            .Where(order => order.Id == id)
            .Include(order => order.User)
            .Include(order => order.Items)
            .FirstOrDefaultAsync();

        public async Task<Order> CreateOrder(string userName, CartViewModel cart, OrderViewModel orderModel)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                throw new InvalidOperationException($"Пользователь {userName} не найден в БД!");

            await using var transaction = await _db.Database.BeginTransactionAsync();
            var order = new Order
            {
                Name = orderModel.Name,
                Phone = orderModel.Phone,
                Address = orderModel.Address,
                User = user,
                Date = DateTime.Now
            };

            foreach (var (productModel, quantity) in cart.Items)
            {
                var product = await _db.Products.FindAsync(productModel.Id);
                if (product != null)
                    order.Items.Add(new OrderItem
                    {
                        Order = order,
                        Product = product,
                        Price = product.Price, // цена на товар, действующая на момент оформления заказа. Может не совпадать с ценой в корзине productModel.Price (если в корзину товар добавляли раньше)
                        Quantity = quantity
                    });
            }

            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            return order;
        }
    }
}
