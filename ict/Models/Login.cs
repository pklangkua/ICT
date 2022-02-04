using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ict.Models
{
    public class Login
    {
        public int UserId { get; set; }
        [Display(Name = "ชื่อผู้ใช้")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string UserName { get; set; }
        [Display(Name = "รหัสผ่าน")]
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string Password { get; set; }
        [Display(Name = "ระดับผู้ใช้งาน")]
        public int IsActive { get; set; }
        [Display(Name = "หน่วยงาน")]
        public string OfficeName { get; set; }
        [Display(Name = "ชื่อ-สกุล")]
        public string fullname { get; set; }
        public string chiefoff { get; set; }
        
    }
}
