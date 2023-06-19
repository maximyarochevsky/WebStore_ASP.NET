using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    //[Route("Staff/{action=Index}/{Id?}")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeeData;

        public EmployeesController(IEmployeesData employeesData)
        {
            _EmployeeData = employeesData;
        }
        public IActionResult Index()
        {
            var result = _EmployeeData.GetAll();
            return View(result);
        }

        public IActionResult Details(int id)
        {
            var employee = _EmployeeData.GetById(id);

           
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
            var employee = _EmployeeData.GetById(id);
            if (employee is null)
                return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel Model)
        {

            var employee = new Employee
            {
                Id = Model.Id,
                LastName = Model.LastName,
                FirstName = Model.FirstName,
                Patronymic = Model.Patronymic,
                Age = Model.Age,
            };
            if (!_EmployeeData.Edit(employee))
                return NotFound();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (id < 0) 
                return BadRequest();

            var employee = _EmployeeData.GetById(id);
            if (employee is null)
                return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age,
            };

            return View(model);
        }    

        public IActionResult DeleteConfirmed(int id)
        {

            if(!_EmployeeData.Delete(id))
                return NotFound();

            return RedirectToAction("Index");
        }
    }
}
