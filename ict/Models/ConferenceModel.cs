using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ict.Models
{
    public class ConferenceModel
    {
        [Key]
        public int ConID { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string ConName { get; set; }
         [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
         [Display(Name = "หน่วยงาน")]
        public string ConDept { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string ConTel { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string Participant { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        //[DataType(DataType.Date)]
        [Display(Name = "วันที่ประชุม")]
        public DateTime DtDate { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtState { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtCuser { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "เรื่อง/หัวข้อประชุม")]  
        public string DtTitle { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtPres { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtPosition { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtTel { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtEmail { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtControl { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtPos2 { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string DtTel2 { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "วันที่สิ้นสุดการประชุม")]
        public DateTime DtDate2 { get; set; }
        public int Status { get; set; }
        public string Users { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string Reason { get; set; }
        public string URL { get; set; }
        public string Color { get; set; }
        [Display(Name = "ยืมครุภัณฑ์")]
        public Boolean Borrow { get; set; }

    }
}
