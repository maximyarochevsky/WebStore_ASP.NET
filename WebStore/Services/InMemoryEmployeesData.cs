using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.Data;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ICollection<Employee> _Employees;

        private int _MaxFreeId;
        public InMemoryEmployeesData()
        {
            _Employees = TestData.__Employees;
            _MaxFreeId = _Employees.DefaultIfEmpty().Max(e => e?.Id ?? 0) + 1;
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
                return false;

            db_employee.Id = employee.Id;
            db_employee.FirstName = employee.FirstName;
            db_employee.LastName = employee.LastName;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;

            return true;
        }

        public bool Delete(int id)
        {
            var employee = GetById(id);

            if (employee is null)
                return false;

            _Employees.Remove(employee);
            return true;
        }

    }
}
