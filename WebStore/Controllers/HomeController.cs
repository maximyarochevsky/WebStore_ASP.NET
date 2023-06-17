using Microsoft.AspNetCore.Mvc;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        public static readonly List<Employee> __Employees = new()
        {
            new Employee{Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 18 },
            new Employee{Id = 2, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 19 },
            new Employee{Id = 3, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 20 }
        };
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Employees()
        {
            return View(__Employees);
        }
    }
}
