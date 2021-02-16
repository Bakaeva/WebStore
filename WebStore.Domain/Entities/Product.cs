using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    public class Product : NamedEntity, IOrderedEntity
    {
        /// <summary>Порядковый номер в списке товаров</summary>
        [Display(Name = "Порядковый номер")]
        public int Order { get; set; }

        /// <summary>id категории товара</summary>
        public int? SectionId​​ { get; set; }

        [ForeignKey(nameof(SectionId​​))]
        [Display(Name = "Категория")]
        /// <summary>Навигационая сущность-категория товара</summary>
        public Section Section { get; set; }

        /// <summary>id бренда товара</summary>
        public int? BrandId​​ { get; set; }

        [ForeignKey(nameof(BrandId​​))]
        [Display(Name = "Бренд")]
        /// <summary>Навигационая сущность-бренд</summary>
        public Brand Brand { get; set; }

        /// <summary>URL на изображение товара</summary>
        [Display(Name = "Изображение товара")]
        public string ImageUrl​​ { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Цена")]
        /// <summary>Цена товара</summary>
        public decimal Price { get; set; }
    }
}
