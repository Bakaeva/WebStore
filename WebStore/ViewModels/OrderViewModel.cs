using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;

namespace WebStore.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        [Display(Name = "Название заказа")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Телефон заказчика")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }
    }
}
