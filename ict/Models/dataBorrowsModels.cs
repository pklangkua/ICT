using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ict.Models
{
    public class dataBorrowsModels
    {
        public int Id { get; set; }
        public int Bor_id { get; set; }
        public int B_Id { get; set; }
        public int Status { get; set; }
        public DateTime StartBor { get; set; }
        public DateTime EndBor { get; set; }
    }
}
