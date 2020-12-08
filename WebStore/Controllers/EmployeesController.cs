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
            var employee = _employees.FirstOrDefault(item => item.Id == id);
            if (employee != null)
                return View(employee);

            return NotFound();
        }

        public IActionResult Remove(int id)
        {
            return;
        }

        public IActionResult Edit(int id)
        {
            return;
        }
    }
