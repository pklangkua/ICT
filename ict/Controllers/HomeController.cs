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

namespace ict.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection sqlconn = new DBClass().SqlStrCon();
        public IActionResult Index()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            TempData["URL"] = "Home";
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }public IActionResult Sign()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Privacy(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String.Split(',')[1]);
            string sql = "INSERT INTO tblFiles VALUES( @Data)";
            using (SqlCommand cmd = new SqlCommand(sql, sqlconn))
            {
                cmd.Parameters.AddWithValue("@Data", bytes);
                sqlconn.Open();
                cmd.ExecuteNonQuery();
                sqlconn.Close();
            }
            return View();
        }
    }
}
