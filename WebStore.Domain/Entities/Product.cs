using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        /// <summary>Категория товара</summary>
        public int SectionId​​ { get; set; }

        /// <summary>Бренд товара</summary>
        public int? BrandId​​ { get; set; }

        /// <summary>URL на изображение товара</summary>
        public string ImageUrl​​ { get; set; }

        /// <summary>Цена товара</summary>
        public decimal Price { get; set; }
    }
}
