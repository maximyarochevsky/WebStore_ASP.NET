﻿using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    //[Route("Staff/{action=Index}/{Id?}")]
    public class EmployeesController : Controller
    {
        private ICollection<Employee> __Employees;

        public EmployeesController()
        {
            __Employees = TestData.__Employees;
        }
        public IActionResult Index()
        {
            var result = __Employees;
            return View(result);
        }

        public IActionResult Details(int id)
        {
            var employee = __Employees.FirstOrDefault(e => e.Id == id);

           
            if(employee is null)
            {
                return NotFound();
            }
            ViewBag.SelectedEmployee = employee;
            return View(employee);
        }

        public IActionResult Create() => View();

        public IActionResult Edit(int id)
        {
            var employee = __Employees.FirstOrDefault(e => e.Id == id);
            if (employee is null)
                return NotFound();

            var model = new EmployeeEditViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            };

            return View();
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel Model)
        {

            if(Model is null)
                throw new ArgumentNullException(nameof(Model));

            Employee employee = new Employee();

            employee.Id = Model.Id;
            employee.LastName = Model.LastName;
            employee.FirstName = Model.FirstName;
            employee.Patronymic = Model.Patronymic;
            employee.Age = Model.Age;


            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id) => View();
    }
}
