using System.Collections.Generic;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static List<Employee> Employees { get; } = new List<Employee>()
        {
            new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 30, DateOfEmployment = new System.DateTime(2015, 9, 1) },
            new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 31, DateOfEmployment = new System.DateTime(2018, 2, 11) },
            new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 32, DateOfEmployment = new System.DateTime(2010, 12, 31) }
        };
    }
}
