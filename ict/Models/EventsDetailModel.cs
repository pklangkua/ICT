using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ict.Models
{
    public class EventsDetailModel
    {
        public int ed_id { get; set; }
        [Display(Name = "เรื่อง")]
        public string ed_name { get; set; }
        [Display(Name = "รายละเอียด")]
        public string ed_detail { get; set; }
        [Display(Name = "ไฟล์")]
        public string ed_file { get; set; }
        public DateTime dateTime { get; set; }
        [Display(Name = "ระเบียบวาระ")]
        public int t_id { get; set; }
        [Display(Name = "วาระที่")]
        public string ed_num { get; set; }
        public int e_id { get; set; }
        public string t_fullname { get; set; }
    }
}
