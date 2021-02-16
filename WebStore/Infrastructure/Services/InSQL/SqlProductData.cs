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

        public Brand GetBrandByName(string name) => GetBrands()
            .FirstOrDefault(brand => brand.Name == name); // Brand.Name NOT NULL

        public IEnumerable<Section> GetSections() => _db.Sections.Include(section => section.Products);

        public Section GetSectionById(int id) => GetSections()
            .FirstOrDefault(section => section.Id == id);

        public Section GetSectionByName(string name) => GetSections()
            .FirstOrDefault(section => section.Name == name); // Section.Name NOT NULL

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

        int IProductData.AddProduct(Product product)
        {
            //if (product == null)
            //    throw new ArgumentNullException(nameof(product));

            //product.Id = _db.Products.Select(item => item.Id).DefaultIfEmpty().Max() + 1;
            //_db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
            _db.Products.Add(product); // class Product : NamedEntity : Entity (Key generated)
            //_db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");
            _db.SaveChanges();

            return product.Id;
        }

        void IProductData.UpdateProduct(Product product)
        {
            //if (product == null)
            //    throw new ArgumentNullException(nameof(product));

            var db_item = GetProductById(product.Id);
            if (db_item == null)
                return;

            db_item.Order = product.Order;
            db_item.Name = product.Name;
            //db_item.ImageUrl = product.ImageUrl;
            //db_item.Brand = product.;
            //db_item.Section = product.;
            db_item.Price = product.Price;

            _db.SaveChanges();
        }

        bool IProductData.DeleteProduct(int id)
        {
            var product = GetProductById(id);
            if (product == null)
                return false;

            _db.Products.Remove(product);
            _db.SaveChanges();
            return true;
        }
    }
}
