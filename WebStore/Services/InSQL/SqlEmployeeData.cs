using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL;

public class SqlEmployeeData : IEmployeesData
{
    private readonly WebStoreDB _db;
    private readonly ILogger<IEmployeesData> _Logger;
    public SqlEmployeeData(WebStoreDB db, ILogger<IEmployeesData> Logger)
    {
        _db = db;
        _Logger = Logger;
    }

    public IEnumerable<Employee> GetAll() => _db.Employees.AsEnumerable();
    public Employee GetById(int id)
    {
        //_db.Employees.FirstOrDefault(e => e.Id == id); // шлет запрос в БД

        return _db.Employees.Find(id); // находит в Кэше,экономит ресы
    }

    public int Add(Employee employee)
    {
       //_db.Employees.Add(employee);
       //_db.Add(employee);

       _db.Entry(employee).State = EntityState.Added;

        _db.SaveChanges();// только здесь объект получит свойство Id, здесь генерируется Sql запрос

        return employee.Id;
    }

    public bool Edit(Employee employee)
    {
        //_db.Entry(employee).State = EntityState.Modified;
        //return _db.SaveChanges() != 0;

        //_db.Update(employee);

        _db.Employees.Update(employee);

        return _db.SaveChanges() != 0;
    }

    public bool Delete(int id)
    {
        // в БД нет возможности удалять объект по Id, поэтому - (следующий код)
        //var employee = GetById(id); т.к объект может быть большим, нам не обязательно получать его весь, нужен лишь Id => (код)

        var employee = _db.Employees
            .Select(e => new Employee{Id = e.Id,}) //неполная проекция - для экномоии памяти и времени на передачу данных
            .FirstOrDefault(e => e.Id == id);

        if (employee is null)
        {
            return false;
        }

        //_db.Entry(employee).State = EntityState.Deleted;
        //_db.Remove(employee);
        _db.Employees.Remove(employee);

        _db.SaveChanges();

        return true;
    }
}
