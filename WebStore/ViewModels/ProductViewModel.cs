using System.ComponentModel.DataAnnotations;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.ViewModels
{
    public class ProductViewModel : INamedEntity
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Порядковый номер")]
        /// <summary>Порядковый номер в списке товаров</summary>
        public int Order { get; set; }

        [Display(Name = "Изображение товара")]
        public string ImageUrl​​ { get; set; }

        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Название бренда")]
        public string Brand { get; set; }

        [Display(Name = "Название секции")]
        public string Section { get; set; }
    }
}
