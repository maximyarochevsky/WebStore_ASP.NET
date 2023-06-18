using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.Data;

namespace WebStore.Services
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ICollection<Employee> _Employees;
        public InMemoryEmployeesData()
        {
            _Employees = TestData.__Employees;
        }
        public int Add(Employee employee)
        {
            
        }

        public bool Delete(int id)
        {
           
        }

        public bool Edit(Employee employee)
        {
           
        }

        public IEnumerable<Employee> GetAll()
        {
            return _Employees;
        }

        public Employee GetById(int id)
        {
            return _Employees.FirstOrDefault(employee => employee.Id == id);
        }
    }
}
