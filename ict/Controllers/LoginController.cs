using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using ict.Models;
using Microsoft.AspNetCore.Session;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace ict.Controllers
{
    public class LoginController : Controller
    {

        SqlConnection sqlconn = new DBClass().SqlStrCon();
        public ActionResult Index()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string IsActive = HttpContext.Session.GetInt32("IsActive").ToString();

            //TempData["URL"] = "/Home/Index";
            return View();
        }
        public ActionResult ListUser()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string IsActive = HttpContext.Session.GetInt32("IsActive").ToString();

            ViewData["ListUserData"] = ListUserDisplay();
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        public IActionResult UserProfile()
        {
            ViewBag.Message = HttpContext.Session.GetString("Username");
            ViewBag.Fullname = HttpContext.Session.GetString("fullname");
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login obj)
        {
            if (ModelState.IsValid)
            {
                sqlconn.Open();
                string sql = @"select * from UserProfile WHERE UserName = @UserName and 
                             Password = CONVERT(VARCHAR(32), HashBytes('MD5', @Password),2) ;";

                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                sqlcomm.Parameters.AddWithValue("@UserName", obj.UserName.ToString());
                sqlcomm.Parameters.AddWithValue("@Password", obj.Password.ToString());
                SqlDataReader sdr = sqlcomm.ExecuteReader();

                if (sdr.Read())
                {
                    var request = new HttpRequestMessage();
                    HttpContext.Session.SetInt32("UserId", sdr.GetInt32(0));
                    HttpContext.Session.SetString("Username", sdr.GetString(1));
                    HttpContext.Session.SetInt32("IsActive", sdr.GetInt32(3));
                    HttpContext.Session.SetString("OfficeName", sdr.GetString(4));
                    HttpContext.Session.SetString("fullname", sdr.GetString(5));
                    HttpContext.Session.SetString("chiefoff", sdr.GetString(6));

                    //HttpContext.Session.SetString("Username", obj.UserName);
                    ViewData["Session"] = HttpContext.Session.GetString("Username");
                    ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
                    // ViewData["chiefoff"] = HttpContext.Session.GetString("chiefoff");

                    sqlconn.Close();
                    string page = HttpContext.Request.Query["page"].ToString();
                    string prevPage = String.Empty;
                    prevPage = TempData["URL"].ToString();
                    //return RedirectToAction("", "Home");
                    //var value = new Uri("http://example.com/");
                    //request.Headers.Referrer = value;
                    return Redirect(prevPage);
                    // return Redirect("https://checkin.dmh.go.th");
                }
                else
                {
                    ViewData["ErrorLogin"] = "ไม่สามารถเข้าสู่ระบบได้ กรุณาตรวจสอบ Username และ Password";
                }

            }
            return View();
        }
        public IActionResult Create()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string IsActive = HttpContext.Session.GetInt32("IsActive").ToString();
            if (IsActive == "2")
            {
                ViewData["Officedata"] = OfficeDisplay();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        [HttpPost]
        public IActionResult Create(Login ur)
        {
            try
            {
                sqlconn.Open();
                string sql = @"INSERT INTO UserProfile VALUES(@UserName,CONVERT(VARCHAR(32), HashBytes('MD5',@Password), 2),@IsActive,@OfficeName,@fullname,@chiefoff)";
                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                sqlcomm.Parameters.AddWithValue("@UserName", ur.UserName);
                sqlcomm.Parameters.AddWithValue("@Password", ur.Password);
                sqlcomm.Parameters.AddWithValue("@IsActive", ur.IsActive);
                sqlcomm.Parameters.AddWithValue("@OfficeName", ur.OfficeName);
                sqlcomm.Parameters.AddWithValue("@fullname", ur.fullname);
                sqlcomm.Parameters.AddWithValue("@chiefoff", "0");

                sqlcomm.ExecuteNonQuery();
                sqlconn.Close();

                return RedirectToAction("ListUser");
            }
            catch
            {
                return View();
            }

        }
        public List<OfficeModel> OfficeDisplay()
        {
            List<OfficeModel> off = new List<OfficeModel>();
            string Osql = "SELECT * FROM Office ORDER BY OrganizationID ";
            SqlCommand sqlcomm = new SqlCommand(Osql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                OfficeModel offs = new OfficeModel();

                offs.OfficeID = Convert.ToInt32(sdr["OfficeID"]);
                offs.OfficeCode = sdr["OfficeCode"].ToString();
                offs.OrganizationID = Convert.ToInt32(sdr["OrganizationID"]);
                offs.OfficeName = sdr["OfficeName"].ToString();

                off.Add(offs);
            }
            sqlconn.Close();
            return off;
        }
        public List<Login> ListUserDisplay()
        {
            List<Login> log = new List<Login>();
            string sql = "select * from UserProfile";
            SqlCommand sqlcomm = new SqlCommand(sql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                Login logs = new Login();

                logs.UserId = Convert.ToInt32(sdr["UserId"]);
                logs.UserName = sdr["UserName"].ToString();
                logs.Password = sdr["Password"].ToString();
                logs.IsActive = Convert.ToInt32(sdr["IsActive"]);
                logs.OfficeName = sdr["OfficeName"].ToString();

                log.Add(logs);
            }
            sqlconn.Close();
            return log;
        }

        public ActionResult Delete(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            if (HttpContext.Session.GetString("Username") != null)
            {
                Login log = new Login();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"select * from UserProfile where UserId =@UserId";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@UserId", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    log.UserId = Convert.ToInt32(dt.Rows[0][0]);
                    log.UserName = dt.Rows[0][1].ToString();
                    log.OfficeName = dt.Rows[0][4].ToString();

                    sqlconn.Close();
                    return View(log);
                }
                else
                    return RedirectToAction("Index");
                //return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            sqlconn.Open();
            string sql = @"delete from UserProfile WHERE  UserId =@UserId ";
            SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

            sqlcomm.Parameters.AddWithValue("@UserId", id);

            sqlcomm.ExecuteNonQuery();
            sqlconn.Close();
            return RedirectToAction("ListUser");
        }

        public ActionResult Password()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            //int Uid = (int)HttpContext.Session.GetInt32("UserId");
            //string up = HttpContext.Session.GetString("Username");

            if (ViewData["Username"] != null)
            {
                int Uid = (int)HttpContext.Session.GetInt32("UserId");
                string up = HttpContext.Session.GetString("Username");
                ViewData["GetUser"] = NameAdd(Uid);
                ViewData["Officedata"] = OfficeDisplay();

                return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public List<Login> NameAdd(int uid)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            List<Login> NameAdd = new List<Login>();
            string Ssql = "SELECT * FROM UserProfile WHERE UserId = '" + uid + "'";
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                Login Add = new Login();

                Add.UserId = Convert.ToInt32(sdr["UserId"]);
                Add.UserName = sdr["UserName"].ToString();
                Add.Password = sdr["Password"].ToString();
                Add.OfficeName = sdr["OfficeName"].ToString();
                Add.fullname = sdr["fullname"].ToString();

                NameAdd.Add(Add);
            }
            sqlconn.Close();
            return NameAdd;
        }

        [HttpPost]
        public IActionResult Password(Login Lg)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");

            try
            {
                sqlconn.Open();
                string sql = @"UPDATE UserProfile SET Password = CONVERT(VARCHAR(32), HashBytes('MD5', @Password),2), OfficeName = @OfficeName , fullname = @fullname WHERE UserId =@UserId";
                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);


                sqlcomm.Parameters.AddWithValue("@UserId", Lg.UserId);
                sqlcomm.Parameters.AddWithValue("@Password", Lg.Password);
                sqlcomm.Parameters.AddWithValue("@OfficeName", Lg.OfficeName);
                sqlcomm.Parameters.AddWithValue("@fullname", Lg.fullname);

                int rowsAffected = sqlcomm.ExecuteNonQuery();
                if (rowsAffected == 1)
                {
                    TempData["messge"] = "บันทึกข้อมูลเรียบร้อย";
                }

                sqlconn.Close();
                //return RedirectToAction("Password", "Login", new { id = Lg.UserId });
                return RedirectToAction("Logout");
            }
            catch
            {
                return View();
            }
        }

    }
}
