using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WebStore.Models;
using System.Linq;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        readonly IConfiguration _configuration;
        List<Employee> employees = null;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
            InitEmployeesList();
        }

        void InitEmployeesList()
        {
            employees = new List<Employee>()
            {
                new Employee { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 30, DateOfEmployment = new System.DateTime(2015, 9, 1) },
                new Employee { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 31, DateOfEmployment = new System.DateTime(2018, 2, 11) },
                new Employee { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 32, DateOfEmployment = new System.DateTime(2010, 12, 31) }
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Employees()
        {
            return View(employees);
        }

        public IActionResult Details(int id)
        {
            var employee = employees.FirstOrDefault(item => item.Id == id);
            //if (employees != null) гарантировано == true
                return View(employee);
        }
    }
}
