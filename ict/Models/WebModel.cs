using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ict.Models
{
    public class WebModel
    {
        public int WebID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string WebSub { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string WebDetail { get; set; }
        public string WebFile { get; set; }
        public DateTime WebDate { get; set; }
        public string Users { get; set; }
    }
}
