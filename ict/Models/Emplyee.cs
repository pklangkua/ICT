using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ict.Models
{
    public class Emplyee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }
        public string Office { get; set; }
        public DateTime HireDate { get; set; }
        public int Salary { get; set; }
    }
}
