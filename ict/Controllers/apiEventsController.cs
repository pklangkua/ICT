using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using ict.Models;
using ict.enums;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ict.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiEventsController : ControllerBase
    {
        SqlConnection con = new DBClass().SqlStrCon();

        [HttpGet]
        public string ConvertDataTabletoString()
        {
            DataTable dt = new DataTable();
            string sql = "SELECT e_id as id , e_name as title, e_sdate as start, e_edate as 'end',concat(URL,e_id) as url ,Color as color FROM Events";
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;

            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            con.Close();
            return JsonSerializer.Serialize(rows);


        }
    }
}
