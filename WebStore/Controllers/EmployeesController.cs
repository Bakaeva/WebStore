using Microsoft.AspNetCore.Mvc;
using WebStore.Models;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;
using System;
using Microsoft.AspNetCore.Authorization;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Controllers
{
    //[Route("Users")]
    [Authorize]
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

        [Authorize(Roles = Role.Administrator)]
        public IActionResult Create() => View("Edit", new EmployeesViewModel());

        #region Edit
        [Authorize(Roles = Role.Administrator)]
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
        [Authorize(Roles = Role.Administrator)]
        public IActionResult Edit(EmployeesViewModel model)
        {
            DateTime dateOfRegistration = new DateTime(2000, 9, 1); // дата регистрации фирмы
            if (model.DateOfEmployment < dateOfRegistration || model.DateOfEmployment > DateTime.Today.Date)
               ModelState.AddModelError("DateOfEmployment", "Дата устройства на работу должна быть не ранее "
                   + dateOfRegistration.ToShortDateString() + " и не позднее сегодняшней даты");
            //ModelState.AddModelError("", "Текст сообщения об ошибке"); // если в I параметре св-во не указано, то сообщение применимо ко всей модели

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!ModelState.IsValid) return View(model);

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
        [Authorize(Roles = Role.Administrator)]
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
        [Authorize(Roles = Role.Administrator)]
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
