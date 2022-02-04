using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ict.Models
{
    public class BorrowsAddModel
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "เรื่อง")]
        public string Subject { get; set; }
        //[Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "หน่วยงาน")]
        public string Dept { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "วัตถุประสงค์")]
        public string Objectives { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "วันที่ยืม")]
        public DateTime Sdate { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "วันที่คืน")]
        public DateTime Edate { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "ชื่อ-นามสกุล ผู้ยืม")]
        public string NameAdd { get; set; }
        public string Checks { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        public string Cdetail { get; set; }
        public string Cname { get; set; }
        public string CheadCheck { get; set; }
        public string CheadDetail { get; set; }
        public string CheadName { get; set; }
        public string CeckCon { get; set; }
        public string CeckConDetail { get; set; }
        public string CeckConName { get; set; }
        public string CCHead { get; set; }
        public string CCHeadDetail { get; set; }
        public string CCHeadName { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "เวลาการยืม")]
        public string Dates { get; set; }
        public Boolean Notebook { get; set; }
        public Boolean Tablet { get; set; }
        [Display(Name = "กล้อง Conference")]
        public Boolean Camera { get; set; }
        [Display(Name = "ขาตั้งกล้อง")]
        public Boolean Tripod { get; set; }
        [Display(Name = "ลำโพง Bluetooth ")]
        public Boolean Speaker { get; set; }
        public Boolean Projector { get; set; }
        public string Users { get; set; }
        [Display(Name = "สถานะการยืม")]
        public int Status { get; set; }
        [Required(ErrorMessage = "กรุณากรอกข้อมูล")]
        [Display(Name = "โทรศัพท์")]
        public int Tel { get; set; }
        public int n_Notebook { get; set; }
        public int n_Tablet { get; set; }
        public int n_Camera { get; set; }
        public int n_Tripod { get; set; }
        public int n_Speaker { get; set; }
        public int n_Projector { get; set; }
    }
}
