using System;
using System.Linq;
using System.Collections.Generic;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using WebStore.Data;

namespace WebStore.Infrastructure.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        readonly List<Employee> _employees = TestData.Employees;

        public IEnumerable<Employee> Get() => _employees;

        public Employee Get(int id) => _employees.FirstOrDefault(item => item.Id == id);

        public int Add(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (_employees.Contains(employee))
                return employee.Id;

            employee.Id = _employees.Select(item => item.Id).DefaultIfEmpty().Max() + 1;
            _employees.Add(employee);

            return employee.Id;
        }

        public void Update(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (_employees.Contains(employee))
                return;

            var db_item = Get(employee.Id);
            if (db_item == null)
                return;

            db_item.LastName = employee.LastName;
            db_item.FirstName = employee.FirstName;
            db_item.Patronymic = employee.Patronymic;
            db_item.Age = employee.Age;
            db_item.DateOfEmployment = employee.DateOfEmployment;
        }

        public bool Delete(int id)
        {
            var employee = Get(id);
            if (employee == null)
                return false;
            
            return _employees​.Remove​(employee​);
        }
    }
}
