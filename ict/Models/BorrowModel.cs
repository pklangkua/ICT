using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ict.Models
{
    public class BorrowModel
    {
        [Key]
        public int B_Id { get; set; }
        public string B_number { get; set; }
        public string B_Name { get; set; }
        public int B_mor { get; set; }
        public int BH_Id { get; set; }
        public int TypeID { get; set; }
        public int GroupID { get; set; }
        public int B_after { get; set; }
    }
}
