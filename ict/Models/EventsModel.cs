using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ict.Models
{
    public class EventsModel
    {
        public int e_id { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "เรื่อง")]
        public string e_name { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "วันที่")]
        public DateTime e_sdate { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "ถึงวันที่")]
        public DateTime e_edate { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "ชื่อผู้สร้าง")]
        public string e_create { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "หน่วยงาน")]
        public string e_dept { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "รายละเอียดอื่นๆ")]
        public string e_other { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "ประธาน")]
        public string e_president { get; set; }
        public DateTime e_dateCreate { get; set; }
        public string Users  { get; set; }
        public string URL { get; set; }
        public string Color { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "ห้องประชุม")]
        public int e_state { get; set; }
        [Display(Name = "ห้องประชุม")]
        public string e_state_name { get; set; }
    }
}
