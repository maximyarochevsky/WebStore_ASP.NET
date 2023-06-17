using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        public static readonly List<Employee> __Employees = new()
        {
            new Employee{Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 18 },
            new Employee{Id = 2, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 19 },
            new Employee{Id = 3, LastName = "Петров", FirstName = "Петр", Patronymic = "Петрович", Age = 20 }
        };
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
            return View(employee);
        }
    }
}
