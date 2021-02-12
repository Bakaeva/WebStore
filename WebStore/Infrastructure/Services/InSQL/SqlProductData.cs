using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SqlProductData : IProductData
    {
        readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db) => _db = db;

        public IEnumerable<Brand> GetBrands() => _db.Brands.Include(brand => brand.Products);

        //public Brand GetBrandById(int id) => _db.Brands.Find(id);
        public Brand GetBrandById(int id) => GetBrands()
            .FirstOrDefault(brand => brand.Id == id);

        public IEnumerable<Section> GetSections() => _db.Sections.Include(section => section.Products);

        public Section GetSectionById(int id) => GetSections()
            .FirstOrDefault(section => section.Id == id);

        public IEnumerable<Product> GetProducts(ProductFilter filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Section);

            if (filter?.Ids?.Length > 0)
            {
                query = query.Where(product => filter.Ids.Contains(product.Id));
            }
            else
            {
                if (filter?.SectionId != null)
                    query = query.Where(product => product.SectionId == filter.SectionId);

                if (filter?.BrandId != null)
                    query = query.Where(product => product.BrandId == filter.BrandId);
            }

            return query;
        }

        public Product GetProductById(int id) => _db.Products
            .Where(p => p.Id == id)
            .Include(p => p.Brand)
            .Include(p => p.Section)            
            .FirstOrDefault();

        //int IProductData.AddProduct(Product product)
        //{
        //    if (product == null)
        //        throw new ArgumentNullException(nameof(product));

        //    if (_db.Products.Contains(product))
        //        return product.Id;

        //    product.Id = _db.Products.Select(item => item.Id).DefaultIfEmpty().Max() + 1;
        //    _db.Products.Add(product);

        //    return product.Id;
        //}

        void IProductData.UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (_db.Products.Contains(product))
                return;

            var db_item = GetProductById(product.Id);
            if (db_item == null)
                return;

            db_item.Name = product.Name;
            db_item.Price = product.Price;
        }

        bool IProductData.DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product == null)
                return false;

            _db.Products.Remove(product);
            return true;
        }

        //public void Update(Employee employee)
        //{
            
        //}
    }
}
