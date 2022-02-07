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
using CrystalDecisions.CrystalReports.Engine;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Web;

namespace ict.Controllers
{
    public class WebController : Controller
    {
        SqlConnection sqlconn = new DBClass().SqlStrCon();
        LineNotification sent = new LineNotification();
        public IActionResult Index()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            string Users = HttpContext.Session.GetString("Username");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string IsActive = HttpContext.Session.GetInt32("IsActive").ToString();
            if (Users != null)
            {
                ViewData["Results"] = ListUserDisplay();
                return View();
            }
            else
            {
                TempData["URL"] = "Web/";
                return RedirectToAction("Index", "Login");
            }


        }

        public List<WebModel> ListUserDisplay()
        {
            string Users = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            string IsActive = HttpContext.Session.GetInt32("IsActive").ToString();
            string sql;
            List<WebModel> web = new List<WebModel>();
            if (IsActive == "2")
            {
                sql = "select * from Web ";
            }
            else
            {
                sql = "select * from Web where Users =@Users";
            }
            //string sql = "select * from Web";
            SqlCommand sqlcomm = new SqlCommand(sql);
            sqlcomm.Parameters.AddWithValue("@Users", Users);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                WebModel webs = new WebModel();

                webs.WebID = Convert.ToInt32(sdr["WebID"]);
                webs.WebSub = sdr["WebSub"].ToString();
                webs.WebDetail = sdr["WebDetail"].ToString();
                webs.WebFile = sdr["WebFile"].ToString();
                webs.WebDate = Convert.ToDateTime(sdr["WebDate"]);
                webs.Users = sdr["Users"].ToString();

                web.Add(webs);
            }
            return web;
        }

        public IActionResult Create()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            string Users = HttpContext.Session.GetString("Username");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string IsActive = HttpContext.Session.GetInt32("IsActive").ToString();
            if (Users != null)
            {
                return View();
            }
            else
            {
                TempData["URL"] = "Web/Create";
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Create(List<IFormFile> WebFile, WebModel ur)
        {

            string Users = HttpContext.Session.GetString("Username");
            var size = WebFile.Sum(f => f.Length);
            var filePaths = new List<string>();

            foreach (var formFile in WebFile)
            {
                if (formFile.Length > 0)
                {
                    string FileName = Path.GetRandomFileName() + Path.GetFileNameWithoutExtension(formFile.FileName) + Path.GetExtension(formFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/File", FileName);
                    var type = Path.GetExtension(formFile.FileName);
                    //string temp = Path.GetTempFileName(filePath);
                    if (type != ".pdf")
                    {
                        ViewBag.ErrorType = "File type not specified ";
                        return View();
                    }
                    else
                    {
                        filePaths.Add(filePath);
                        using (var strem = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(strem);

                            sqlconn.Open();
                            try
                            {
                                string sql = @"INSERT INTO Web VALUES(@WebSub,@WebDetail,@WebFile,GETDATE(),'" + Users + "')";
                                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                                sqlcomm.Parameters.AddWithValue("@WebSub", ur.WebSub);
                                sqlcomm.Parameters.AddWithValue("@WebDetail", ur.WebDetail);
                                sqlcomm.Parameters.AddWithValue("@WebFile", FileName);

                                sqlcomm.ExecuteNonQuery();

                                var filePath2 = "https://ict.dmh.go.th/conference/File/" + FileName;
                                string msg2 = @"การขอลงเว็บไซต์" + "\n" + "หัวข้อ :" + ur.WebSub + "\n" + "รายละเอียด: " + ur.WebDetail + "\n" + "วันที่ : " + DateTime.Now + "\n" + "File :" + filePath2;
                                sent.SentLine("AdgZGCxjLPlcI7vU2y0dMgjdWiJdFOcUL1cmsfqqON5", msg2);
                            }
                            catch (Exception)
                            {

                            }

                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            if (HttpContext.Session.GetString("Username") != null)
            {
                WebModel conf = new WebModel();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"select * from Web where WebID=@WebID";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@WebID", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    conf.WebSub = dt.Rows[0][1].ToString();
                    conf.WebDetail = dt.Rows[0][2].ToString();
                    conf.WebFile = dt.Rows[0][3].ToString();

                    ViewData["User"] = HttpContext.Session.GetString("Username");
                    return View(conf);
                }
                else
                    return RedirectToAction("Index");
                //return View();
            }
            else
                return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public ActionResult Delete(int id, string FileName, string WebSub)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            sqlconn.Open();
            string sql = @"delete from Web where WebID=@WebID ";
            SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

            sqlcomm.Parameters.AddWithValue("@WebID", id);
            var filePath = Path.Combine("wwwroot/File", FileName);
            System.IO.File.Delete(filePath);

            sqlcomm.ExecuteNonQuery();
            var filePath2 = "https://ict.dmh.go.th/conference/File/" + FileName;
            string msg2 = @"ลบข้อมูลการขอลงเว็บไซต์" + "\n" + "หัวข้อ :" + WebSub + "\n" + "File :" + filePath2;

            sent.SentLine("AdgZGCxjLPlcI7vU2y0dMgjdWiJdFOcUL1cmsfqqON5", msg2);
            return RedirectToAction("Index");
        }

        //public ActionResult Download_PDF()
        //{
        //    ConferenceModel context = new ConferenceModel();

        //    ReportDocument rd = new ReportDocument();
        //    rd.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Report"), "Emp_Data.rpt");
        //   // rd.SetDataSource(context.conference.Select(c => new
        //    //{
        //    //    id = c.id,
        //    //    name = c.name
        //    //}).ToList());

        //    //Response.Buffer = false;
        //    //Response.ClearContent();
        //    //Response.ClearHeaders();


        //    rd.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
        //    rd.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
        //    rd.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;

        //    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //    stream.Seek(0, SeekOrigin.Begin);

        //    return File(stream, "application/pdf", "CustomerList.pdf");
        //}

        public ActionResult pdf()
        {
            ReportDocument cryRpt = new ReportDocument();
            //cryRpt.PrintOptions.PaperSize = PaperSize.PaperA4;
            // string patch = ConfigurationSettings.AppSettings["serverpath"];
            //string Report = patch + "Count_risk.rpt";
            cryRpt.Load(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Report"), "CrystalReport1.rpt");
            // GlobalVar.ReportLoginPathServer(Report);
            // cryRpt.Load(Report);
            // GlobalVar.ReportLogin(cryRpt);

            cryRpt.SetParameterValue("@ConID", "'" + 10 + "'");

            cryRpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            cryRpt.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(5, 5, 5, 5));
            cryRpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA5;

            Stream stream = cryRpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            return File(stream, "application/pdf", "CustomerList.pdf");
        }

    }
}