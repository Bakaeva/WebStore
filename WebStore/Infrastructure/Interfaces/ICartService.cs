﻿using WebStore.ViewModels;

namespace WebStore.Infrastructure.Interfaces
{
    public interface ICartService
    {
        /// <summary>Добавление товара в корзину</summary>
        /// <param name="id">ID товара</param>
        /// <param name="cnt">Кол-во добавляемых единиц товара</param>
        void AddToCart(int id, int cnt = 1);

        /// <summary>Удаление 1шт товара из корзины</summary>
        /// <param name="id">ID товара</param>
        void DecrementFromCart(int id);

        /// <summary>Удаление всего кол-ва товара из корзины</summary>
        /// <param name="id">ID товара</param>
        void RemoveFromCart(int id);

        /// <summary>Удаление всех товаров из корзины</summary>
        void Clear();

        /// <summary>Преобразование типа Cart к типу CartViewModel</summary>
        CartViewModel TransformFromCart();
    }
}