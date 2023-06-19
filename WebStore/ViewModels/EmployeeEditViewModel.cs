using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class EmployeeEditViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }

    }
}
