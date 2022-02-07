using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ict.Models
{
    public class OfficeModel
    {
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public int OrganizationID { get; set; }
        [Display(Name = "หน่วยงาน")]
        public string OfficeName { get; set; }
    }
}
