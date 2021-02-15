using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.ViewModels
{
    public class EmployeesViewModel //: IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        /// <summary>Имя</summary>
        [Display(Name = "Имя")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Имя должно быть длиной от 2 до 20 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|([A-Z][a-z]+)", ErrorMessage = "Ошибка формата имени")]
        [Required(ErrorMessage = "Поле 'Имя' должно быть заполнено!")]
        public string FirstName { get; set; }

        /// <summary>Фамилия</summary>
        [Display(Name = "Фамилия")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Фамилия должна быть длиной от 2 до 20 символов")]
        [Required(ErrorMessage = "Поле 'Фамилия' должно быть заполнено!")]
        public string LastName { get; set; }

        /// <summary>Отчество</summary>
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        /// <summary>Возраст</summary>
        [Display(Name = "Возраст")]
        [Range(18, 80, ErrorMessage = "Возраст сотрудника должен быть от 18 до 80 лет")]
        public int Age { get; set; }

        /// <summary>Дата устройства на работу</summary>
        [Display(Name = "Дата трудоустройства")]
        public DateTime DateOfEmployment { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext context)
        //{
        //    yield return ValidationResult.Success;
        //}
    }
}
