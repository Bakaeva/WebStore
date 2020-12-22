using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        /// <summary>Категория товара</summary>
        public int SectionId​​ { get; set; }

        [ForeignKey(nameof(SectionId​​))]
        /// <summary>Навигационая сущность-бренд</summary>
        public Section Section { get; set; }

        /// <summary>Бренд товара</summary>
        public int? BrandId​​ { get; set; }

        [ForeignKey(nameof(BrandId​​))]
        /// <summary>Навигационая сущность-бренд</summary>
        public Brand Brand { get; set; }

        /// <summary>URL на изображение товара</summary>
        public string ImageUrl​​ { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        /// <summary>Цена товара</summary>
        public decimal Price { get; set; }
    }
}
