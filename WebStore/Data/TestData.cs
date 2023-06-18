using WebStore.Models;

namespace WebStore.Data
{
    public class TestData
    {
        public static List<Employee> __Employees { get; } = new()
        {
            new Employee{Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 18 },
            new Employee{Id = 2, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 19 },
            new Employee{Id = 3, LastName = "Петров", FirstName = "Петр", Patronymic = "Петрович", Age = 20 }
        };
    }
}
