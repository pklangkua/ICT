using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ict.Models
{
    public class StateModel
    {
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public int S_Id { get; set; }
        public string S_Name { get; set; }
    }
}
