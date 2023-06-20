using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.Data;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ICollection<Employee> _Employees;
        private readonly ILogger<InMemoryEmployeesData> _Logger;
        private int _MaxFreeId;


        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> Logger)
        {
            _Employees = TestData.__Employees;
            _MaxFreeId = _Employees.DefaultIfEmpty().Max(e => e?.Id ?? 0) + 1;
            Logger = Logger;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _Employees;
        }

        public Employee GetById(int id)
        {
            return _Employees.FirstOrDefault(employee => employee.Id == id);
        }
        public int Add(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));
            if(_Employees.Contains(employee))
                return employee.Id;

            employee.Id = _MaxFreeId++;
            _Employees.Add(employee);

            return employee.Id;
        }

        public bool Edit(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee))
                return true;

            var db_employee = GetById(employee.Id);

            if (db_employee is null)
            {
                _Logger.LogWarning("Попытка редактирования несуществующего сотрудника с id: {0}", employee.Id);
                return false;
            }

            db_employee.Id = employee.Id;
            db_employee.FirstName = employee.FirstName;
            db_employee.LastName = employee.LastName;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;

            _Logger.LogInformation("Информация о сотруднике id {0} была изменена", employee.Id);

            return true;
        }

        public bool Delete(int id)
        {
            var employee = GetById(id);

            if (employee is null)
            {
                _Logger.LogWarning("Попытка удаления отсутствующего сотрудника с id {0}", id);
                return false;
            }

            _Employees.Remove(employee);
            _Logger.LogInformation("Успешное удаление сотрудника с id {0}", employee.Id);
            return true;
        }

    }
}
