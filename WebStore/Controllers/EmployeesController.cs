using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        List<Employee> _employees;

        public EmployeesController() => _employees = TestData.Employees;

        public IActionResult Index() => View(_employees);

        public IActionResult Details(int id)
        {
            var employee = GetById(id);
            if (employee != null)
                return View(employee);

            return NotFound();
        }

        Employee GetById(int id)
        {
            return _employees.FirstOrDefault(item => item.Id == id);
        }

        public IActionResult Remove(int id)
        {
            var employee = GetById(id);
            if (employee != null)
                _employees​.Remove​(employee​);
            return View("Index", _employees);
        }

        public IActionResult Edit(int id)
        {
            var employee = GetById(id);
            if (employee != null)
                return View("EmployeeView", employee); // вызов представления с формой редактирования сотрудника
            return NotFound();
        }

        public IActionResult Commit()
        {
            return NoContent(); // временная заглушка
        }
    }
}
