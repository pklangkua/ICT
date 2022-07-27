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
    public class RepairController : Controller
    {
        SqlConnection sqlconn = new DBClass().SqlStrCon();
        LineNotification sent = new LineNotification();
        Datetime date = new Datetime();
        public ActionResult Index()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");

            return View();
        }
        public ActionResult Create()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");

            return View();
        }
    }
}
