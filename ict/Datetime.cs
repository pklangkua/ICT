using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ict
{
    class Datetime
    {
        //*****Date time****//
        public string changeDatetime(DateTime dpt) // แปลงวันที่เป็น 00/00/0000
        {
            string changeDate = "";
            string Year = (dpt.Year).ToString();
            string month = (dpt.Month).ToString("00");
            string day = (dpt.Day).ToString("00");
            //string month = dpt.Month.ToString("00");

            changeDate = Year + "-" + month + "-" + day;

            return changeDate;
        }

    }
}
