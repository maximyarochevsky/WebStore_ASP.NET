using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    //[Route("Staff/{action=Index}/{Id?}")]
    [Authorize]
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
        [Authorize(Roles = "Administrators")]
        public IActionResult Create() => View("Edit", new EmployeeViewModel());
        [Authorize(Roles = "Administrators")]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());

            var employee = _EmployeeData.GetById((int)id);
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

        [Authorize(Roles = "Administrators")]
        [HttpPost]
        public IActionResult Edit(EmployeeViewModel Model)
        {
            if (Model.LastName == "Асама" && Model.FirstName == "Бин" && Model.Patronymic == "Ладен")
                ModelState.AddModelError("", "Террористов на работу не берём!");

            if(!ModelState.IsValid)
                return View(Model);

            var employee = new Employee
            {
                Id = Model.Id,
                LastName = Model.LastName,
                FirstName = Model.FirstName,
                Patronymic = Model.Patronymic,
                Age = Model.Age,
            };
            if(Model.Id == 0)
            {
                _EmployeeData.Add(employee);
            }
            else if(!_EmployeeData.Edit(employee))
                return NotFound();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrators")]
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
