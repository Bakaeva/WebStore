using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Models;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;
using System;

namespace WebStore.Controllers
{
    //[Route("Users")]
    public class EmployeesController : Controller
    {
        readonly IEmployeesData _employeeService;
        public EmployeesController(IEmployeesData employees) => _employeeService = employees;

        //[Route("All")]
        public IActionResult Index()
        {
            var employees = _employeeService.Get();
            return View(employees);
        }

        //[Route("Info({id})")]
        public IActionResult Details(int id)
        {
            var employee = _employeeService.Get(id);
            if (employee != null)
                return View(employee);

            return NotFound();
        }

        public IActionResult Create() => View("Edit", new EmployeesViewModel());

        #region Edit
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return View(new EmployeesViewModel()); // вызов представления-формы добавления сотрудника

            if (id < 0)
                return BadRequest();

            var employee = _employeeService.Get((int)id);
            if (employee == null)
                return NotFound();

            return View(new EmployeesViewModel
            {
                Id = (int)id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                DateOfEmployment = employee.DateOfEmployment,
            }); // вызов представления-формы редактирования сотрудника
        }

        [HttpPost]
        public IActionResult Edit(EmployeesViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Patronymic = model.Patronymic,
                Age = model.Age,
                DateOfEmployment = model.DateOfEmployment,
            };

            if (employee.Id == 0)
                _employeeService.Add(employee);
            else
                _employeeService.Update(employee);

            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest();

            var employee = _employeeService.Get(id);
            if (employee == null)
                return NotFound();

            return View(new EmployeesViewModel
            {
                Id = id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
                DateOfEmployment = employee.DateOfEmployment,
            }); // вызов представления-формы с кнопкой подтверждения на удаление сотрудника
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id < 0)
                return BadRequest();

            _employeeService.Delete(id);
            return RedirectToAction("Index");
        }            
        #endregion

        public IActionResult Commit()
        {
            return NoContent(); // временная заглушка
        }
    }
}
