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
using Microsoft.AspNetCore.Routing;

namespace ict.Controllers
{
    public class BorrowController : Controller
    {
        SqlConnection sqlconn = new DBClass().SqlStrCon();
        LineNotification sent = new LineNotification();

        Datetime date = new Datetime();
        public ActionResult Index()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            string Users = HttpContext.Session.GetString("Username");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            ViewData["Results"] = BorrowDisplay();
            return View();
        }

        public List<BorrowModel> BorrowDisplay()
        {
            List<BorrowModel> bor = new List<BorrowModel>();
            string Ssql = "SELECT * FROM BorrowsHardware WHERE Active = 1";
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                BorrowModel br = new BorrowModel();

                br.B_Id = Convert.ToInt32(sdr["B_Id"]);
                br.B_number = sdr["B_number"].ToString();
                br.B_Name = sdr["B_Name"].ToString();
                br.B_mor = Convert.ToInt32(sdr["B_mor"]);
                br.BH_Id = Convert.ToInt32(sdr["BH_Id"]);
                br.TypeID = Convert.ToInt32(sdr["TypeID"]);
                br.GroupID = Convert.ToInt32(sdr["GroupID"]);
                br.B_after = Convert.ToInt32(sdr["B_after"]);

                bor.Add(br);
            }
            return bor;
        }

        public IActionResult Create(BorrowsAddModel br)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            if (HttpContext.Session.GetString("Username") != null)
            {
                // ViewData["sss"] = BorrowsAddModel();
                return View();
            }
            else
            {
                TempData["URL"] = "Borrow/Create";
                return RedirectToAction("Index", "Login");
            }
                
        }

        [HttpPost]
        public IActionResult Create2(BorrowsAddModel ur)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            string Dept = (string)ViewData["OfficeName"];
            string Subject = "ขออนุมัติยืมครุภัณฑ์";

            string Users = HttpContext.Session.GetString("Username");
            int Notebook = 0;
            int Tablet = 0;
            int Camera = 0;
            int Tripod = 0;
            int Speaker = 0;
            int Projector = 0;
            if (ur.Notebook == true)
            {
                Notebook = 1;
            }
            if (ur.Tablet == true)
            {
                Tablet = 1;
            }
            if (ur.Camera == true)
            {
                Camera = 1;
            }
            if (ur.Tripod == true)
            {
                Tripod = 1;
            }
            if (ur.Speaker == true)
            {
                Speaker = 1;
            }
            if (ur.Projector == true)
            {
                Projector = 1;
            }

            try
            {
                sqlconn.Open();
                string sql = @"INSERT INTO BorrowsAdd VALUES(@Subject,@Dept,@Objectives,@Sdate,@Edate,@NameAdd,'0','',
                               '','0','','','0','','','0','','',@Dates,@Notebook,@Tablet,@Camera,@Tripod,@Speaker,@Projector,@Users,@Status,@Tel,
                                @n_Notebook,@n_Tablet,@n_Camera,@n_Tripod,@n_Speaker,@n_Projector)";
                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                sqlcomm.Parameters.AddWithValue("@Subject", Subject);
                sqlcomm.Parameters.AddWithValue("@Dept", Dept);
                sqlcomm.Parameters.AddWithValue("@Objectives", ur.Objectives);
                sqlcomm.Parameters.AddWithValue("@Sdate", ur.Sdate);
                sqlcomm.Parameters.AddWithValue("@Edate", ur.Edate);
                sqlcomm.Parameters.AddWithValue("@NameAdd", ur.NameAdd);
                sqlcomm.Parameters.AddWithValue("@Dates", "ทั้งวัน");
                sqlcomm.Parameters.AddWithValue("@Notebook", Notebook);
                sqlcomm.Parameters.AddWithValue("@Tablet", Tablet);
                sqlcomm.Parameters.AddWithValue("@Camera", Camera);
                sqlcomm.Parameters.AddWithValue("@Tripod", Tripod);
                sqlcomm.Parameters.AddWithValue("@Speaker", Speaker);
                sqlcomm.Parameters.AddWithValue("@Projector", Projector);
                sqlcomm.Parameters.AddWithValue("@Users", Users);
                sqlcomm.Parameters.AddWithValue("@Status", 0);
                sqlcomm.Parameters.AddWithValue("@Tel", ur.Tel);
                sqlcomm.Parameters.AddWithValue("@n_Notebook", ur.n_Notebook);
                sqlcomm.Parameters.AddWithValue("@n_Tablet", ur.n_Tablet);
                sqlcomm.Parameters.AddWithValue("@n_Camera", ur.n_Camera);
                sqlcomm.Parameters.AddWithValue("@n_Tripod", ur.n_Tripod);
                sqlcomm.Parameters.AddWithValue("@n_Speaker", ur.n_Speaker);
                sqlcomm.Parameters.AddWithValue("@n_Projector", ur.n_Projector);

                int intd = sqlcomm.ExecuteNonQuery();
                if (intd == 1)
                {
                    string msg = @"" + Subject + "\n" + "หน่วยงาน : " + Dept + "\n" + "วันที่รับ : " + ur.Sdate + "\n" + "วันที่คืน : " + ur.Edate;
                    sent.SentLine("6uiaGbqkZ2KlAW7cK39I4zGB8zeL1yGfYK1asj7Blgo", msg);
                }

                sqlconn.Close();
                return RedirectToAction("ShowAdd");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult ShowAdd()
        {

            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            if (ViewData["Username"] != null)
            {
                ViewData["chiefoff"] = HttpContext.Session.GetString("chiefoff");
            }
            else
            {
                ViewData["chiefoff"] = "0";
            }

            ViewData["Results"] = BorrowAddDisplay();
            return View();
        }

        public List<BorrowsAddModel> BorrowAddDisplay()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            string Users = HttpContext.Session.GetString("Username");
            string IsActive = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string psql;
            if (IsActive != "2")
            {
                psql = "SELECT b.* FROM BorrowsAdd b INNER JOIN UserProfile u ON u.UserName = b.Users where b.Users='" + Users + "' ORDER BY b.Id DESC";
            }
            else
            {
                psql = "SELECT * FROM BorrowsAdd ORDER BY Id DESC";
            }

            List<BorrowsAddModel> borAdd = new List<BorrowsAddModel>();
            string Ssql = psql;
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                BorrowsAddModel brAdd = new BorrowsAddModel();

                brAdd.Id = Convert.ToInt32(sdr["Id"]);
                brAdd.Subject = sdr["Subject"].ToString();
                brAdd.Dept = sdr["Dept"].ToString();
                brAdd.Objectives = sdr["Objectives"].ToString();
                brAdd.Sdate = Convert.ToDateTime(sdr["Sdate"]);
                brAdd.Edate = Convert.ToDateTime(sdr["Edate"]);
                brAdd.NameAdd = sdr["NameAdd"].ToString();
                brAdd.Dates = sdr["Dates"].ToString();
                brAdd.Status = Convert.ToInt32(sdr["Status"]);
                brAdd.Checks = sdr["Checks"].ToString();

                borAdd.Add(brAdd);
            }
            sqlconn.Close();
            return borAdd;
        }

        public ActionResult Detail(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            BorrowsAddModel brAdd = new BorrowsAddModel();
            DataTable dt = new DataTable();
            sqlconn.Open();
            string sql = @"SELECT * FROM BorrowsAdd WHERE Id =@ID";
            SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
            sda.SelectCommand.Parameters.AddWithValue("@ID", id);
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                brAdd.Id = Convert.ToInt32(dt.Rows[0][0]);
                brAdd.Subject = dt.Rows[0][1].ToString();
                brAdd.Dept = dt.Rows[0][2].ToString();
                brAdd.Objectives = dt.Rows[0][3].ToString();
                brAdd.Sdate = Convert.ToDateTime(dt.Rows[0][4]);
                brAdd.Edate = Convert.ToDateTime(dt.Rows[0][5]);
                brAdd.NameAdd = dt.Rows[0][6].ToString();
                brAdd.Checks = dt.Rows[0][7].ToString();
                brAdd.Cdetail = dt.Rows[0][8].ToString();
                brAdd.Cname = dt.Rows[0][9].ToString();
                brAdd.CheadCheck = dt.Rows[0][10].ToString();
                brAdd.CheadDetail = dt.Rows[0][11].ToString();
                brAdd.CheadName = dt.Rows[0][12].ToString();
                brAdd.CeckCon = dt.Rows[0][13].ToString();
                brAdd.CeckConDetail = dt.Rows[0][14].ToString();
                brAdd.CeckConName = dt.Rows[0][15].ToString();
                brAdd.CCHead = dt.Rows[0][16].ToString();
                brAdd.CCHeadDetail = dt.Rows[0][17].ToString();
                brAdd.CCHeadName = dt.Rows[0][18].ToString();
                brAdd.Dates = dt.Rows[0][19].ToString();
                brAdd.Notebook = Convert.ToBoolean(dt.Rows[0][20]);
                brAdd.Tablet = Convert.ToBoolean(dt.Rows[0][21]);
                brAdd.Camera = Convert.ToBoolean(dt.Rows[0][22]);
                brAdd.Tripod = Convert.ToBoolean(dt.Rows[0][23]);
                brAdd.Speaker = Convert.ToBoolean(dt.Rows[0][24]);
                brAdd.Projector = Convert.ToBoolean(dt.Rows[0][25]);
                brAdd.Users = dt.Rows[0][26].ToString();
                brAdd.Status = Convert.ToInt32(dt.Rows[0][27]);
                brAdd.Tel = Convert.ToInt32(dt.Rows[0][28]);
                brAdd.n_Notebook = Convert.ToInt32(dt.Rows[0][29]);
                brAdd.n_Tablet = Convert.ToInt32(dt.Rows[0][30]);
                brAdd.n_Camera = Convert.ToInt32(dt.Rows[0][31]);
                brAdd.n_Tripod = Convert.ToInt32(dt.Rows[0][32]);
                brAdd.n_Speaker = Convert.ToInt32(dt.Rows[0][33]);
                brAdd.n_Projector = Convert.ToInt32(dt.Rows[0][34]);

                return View(brAdd);
            }
            else
                return RedirectToAction("Index");
        }

        public List<BorrowModel> BorrowDisplay2(string Dates, DateTime Sdate, DateTime Edate)
        {
            string DtDate = date.changeDatetime(Sdate);
            string DtDate2 = date.changeDatetime(Edate);

            string date1 = DtDate + " 00:00:00";
            string date2 = DtDate + " 23:59:00";
            string Psql = "";
            if (Dates == "ทั้งวัน")
            {
                Psql = @"SELECT h.* FROM BorrowsHardware h
                        where h.B_Id NOT IN(
                        SELECT DISTINCT d.B_Id FROM dataBorrows d 
                        WHERE d.dateStart BETWEEN '" + date1 + "' AND '" + date2 + "' " +
                        "OR  d.dateEnd BETWEEN '" + date1 + "' AND '" + date2 + "')";
            }
            else if (Dates == "ครึ่งวันเช้า")
            {
                Psql = "SELECT * FROM BorrowsHardware WHERE B_mor= 0 ";
            }
            else if (Dates == "ครึ่งวันบ่าย")
            {
                Psql = "SELECT * FROM BorrowsHardware WHERE ( B_after = 0)";
            }
            List<BorrowModel> bor = new List<BorrowModel>();
            string Ssql = Psql;
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                BorrowModel br = new BorrowModel();

                br.B_Id = Convert.ToInt32(sdr["B_Id"]);
                br.B_number = sdr["B_number"].ToString();
                br.B_Name = sdr["B_Name"].ToString();
                br.B_mor = Convert.ToInt32(sdr["B_mor"]);
                br.BH_Id = Convert.ToInt32(sdr["BH_Id"]);
                br.TypeID = Convert.ToInt32(sdr["TypeID"]);
                br.GroupID = Convert.ToInt32(sdr["GroupID"]);
                br.B_after = Convert.ToInt32(sdr["B_after"]);

                bor.Add(br);
            }
            sqlconn.Close();
            return bor;
        }

        public ActionResult Edit_Checks(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            if (HttpContext.Session.GetString("Username") != null)
            {
                BorrowsAddModel brAdd = new BorrowsAddModel();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"SELECT * FROM BorrowsAdd WHERE Id =@BID";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@BID", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    brAdd.Id = Convert.ToInt32(dt.Rows[0][0]);
                    brAdd.Subject = dt.Rows[0][1].ToString();
                    brAdd.Dept = dt.Rows[0][2].ToString();
                    brAdd.Objectives = dt.Rows[0][3].ToString();
                    brAdd.Sdate = Convert.ToDateTime(dt.Rows[0][4]);
                    brAdd.Edate = Convert.ToDateTime(dt.Rows[0][5]);
                    brAdd.NameAdd = dt.Rows[0][6].ToString();
                    brAdd.Checks = dt.Rows[0][7].ToString();
                    brAdd.Cdetail = dt.Rows[0][8].ToString();
                    brAdd.Cname = dt.Rows[0][9].ToString();
                    brAdd.CheadCheck = dt.Rows[0][10].ToString();
                    brAdd.CheadDetail = dt.Rows[0][11].ToString();
                    brAdd.CheadName = dt.Rows[0][12].ToString();
                    brAdd.CeckCon = dt.Rows[0][13].ToString();
                    brAdd.CeckConDetail = dt.Rows[0][14].ToString();
                    brAdd.CeckConName = dt.Rows[0][15].ToString();
                    brAdd.CCHead = dt.Rows[0][16].ToString();
                    brAdd.CCHeadDetail = dt.Rows[0][17].ToString();
                    brAdd.CCHeadName = dt.Rows[0][18].ToString();
                    brAdd.Dates = dt.Rows[0][19].ToString();
                    brAdd.Notebook = Convert.ToBoolean(dt.Rows[0][20]);
                    brAdd.Tablet = Convert.ToBoolean(dt.Rows[0][21]);
                    brAdd.Camera = Convert.ToBoolean(dt.Rows[0][22]);
                    brAdd.Tripod = Convert.ToBoolean(dt.Rows[0][23]);
                    brAdd.Speaker = Convert.ToBoolean(dt.Rows[0][24]);
                    brAdd.Projector = Convert.ToBoolean(dt.Rows[0][25]);
                    brAdd.Users = dt.Rows[0][26].ToString();
                    brAdd.Status = Convert.ToInt32(dt.Rows[0][27]);
                    brAdd.Tel = Convert.ToInt32(dt.Rows[0][28]);
                    brAdd.n_Notebook = Convert.ToInt32(dt.Rows[0][29]);
                    brAdd.n_Tablet = Convert.ToInt32(dt.Rows[0][30]);
                    brAdd.n_Camera = Convert.ToInt32(dt.Rows[0][31]);
                    brAdd.n_Tripod = Convert.ToInt32(dt.Rows[0][32]);
                    brAdd.n_Speaker = Convert.ToInt32(dt.Rows[0][33]);
                    brAdd.n_Projector = Convert.ToInt32(dt.Rows[0][34]);

                    sqlconn.Close();

                    ViewData["Res"] = BorrowDisplay2(brAdd.Dates, brAdd.Sdate, brAdd.Edate);
                    ViewData["HardWare"] = Hardware(brAdd.Id);
                    ViewData["NameAdd"] = NameAdd();
                    return View(brAdd);
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        [HttpPost]
        public ActionResult Edit_Checks(int[] B_Id, int Bor_id, string tdate, BorrowsAddModel bow)
        {
            int B_mor;
            int B_after;
            string Cdetail;
            if (bow.Cdetail == null)
            {
                Cdetail = "-";
            }
            else
                Cdetail = bow.Cdetail;

            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            string fullname = HttpContext.Session.GetString("fullname").ToString();

            string StartBor = date.changeDatetime(DateTime.Now);
            if (HttpContext.Session.GetString("Username") != null)
            {
                sqlconn.Open();
                for (int i = 0; i < B_Id.Length; i++)
                {
                    string sql = @"INSERT INTO dataBorrows VALUES(@Bor_id,@B_Id,@Status,'" + StartBor + "' ,'',@Sdate,@Edate)";
                    SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                    sqlcomm.Parameters.AddWithValue("@Bor_id", Bor_id);
                    sqlcomm.Parameters.AddWithValue("@B_Id", B_Id[i]);
                    sqlcomm.Parameters.AddWithValue("@Status", 1);
                    sqlcomm.Parameters.AddWithValue("@Sdate", bow.Sdate);
                    sqlcomm.Parameters.AddWithValue("@Edate", bow.Edate);

                    int rowsAffected = sqlcomm.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        if (tdate == "ทั้งวัน")
                        {
                            B_mor = 1;
                            B_after = 1;
                            string StrSQL = @"UPDATE BorrowsHardware SET B_mor =" + B_mor + ", B_after=" + B_after + " WHERE B_Id = " + B_Id[i] + "  ";
                            SqlCommand sqlcom = new SqlCommand(StrSQL, sqlconn);
                            sqlcom.ExecuteNonQuery();
                        }
                        else if (tdate == "ครึ่งวันเช้า")
                        {
                            B_mor = 1;
                            string StrSQL = @"UPDATE BorrowsHardware SET B_mor =" + B_mor + " WHERE B_Id = " + B_Id[i] + "  ";
                            SqlCommand sqlcom = new SqlCommand(StrSQL, sqlconn);
                            sqlcom.ExecuteNonQuery();
                        }
                        else if (tdate == "ครึ่งวันบ่าย")
                        {
                            B_after = 1;
                            string StrSQL = @"UPDATE BorrowsHardware SET B_after=" + B_after + " WHERE B_Id = " + B_Id[i] + "  ";
                            SqlCommand sqlcom = new SqlCommand(StrSQL, sqlconn);
                            sqlcom.ExecuteNonQuery();
                        }

                    }

                }

                string sql2 = @"UPDATE BorrowsAdd SET Checks = @Checks,Cdetail = @Cdetail ,Cname = @Cname, Status = 1, CheadName = @CheadName WHERE Id =@Id ";
                SqlCommand sqlcomm2 = new SqlCommand(sql2, sqlconn);

                sqlcomm2.Parameters.AddWithValue("@Id", bow.Id);
                sqlcomm2.Parameters.AddWithValue("@Checks", bow.Checks);
                sqlcomm2.Parameters.AddWithValue("@Cdetail", Cdetail);
                sqlcomm2.Parameters.AddWithValue("@Cname", bow.Cname);
                sqlcomm2.Parameters.AddWithValue("@CheadName", fullname);
                int intd2 = sqlcomm2.ExecuteNonQuery();

                if (intd2 == 1)
                {
                    string msg2 = @"" + bow.Subject + "\n" + "หน่วยงาน : " + bow.Dept + "\n" + "วันที่รับ : " + bow.Sdate + "\n" + "วันที่คืน : " + bow.Edate + "\n" + "ผู้รับผิดชอบ : " + bow.Cname;
                    sent.SentLine("AdgZGCxjLPlcI7vU2y0dMgjdWiJdFOcUL1cmsfqqON5", msg2);
                    // AdgZGCxjLPlcI7vU2y0dMgjdWiJdFOcUL1cmsfqqON5
                }

                sqlconn.Close();
                return RedirectToAction("Edit_Checks", new { id = Bor_id });
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult Edit(BorrowsAddModel bow)
        {
            string CheadDetail;
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string fullname = HttpContext.Session.GetString("fullname").ToString();
            if (bow.CheadDetail == null)
            {
                CheadDetail = "-";
            }
            else
                CheadDetail = bow.CheadDetail;
            if (HttpContext.Session.GetString("Username") != null)
            {
                sqlconn.Open();
                string sql = @"UPDATE BorrowsAdd SET CheadCheck = @CheadCheck,CheadDetail = @CheadDetail ,CheadName = @CheadName WHERE Id =@Id ";
                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                sqlcomm.Parameters.AddWithValue("@Id", bow.Id);
                sqlcomm.Parameters.AddWithValue("@CheadCheck", bow.CheadCheck);
                sqlcomm.Parameters.AddWithValue("@CheadDetail", CheadDetail);
                sqlcomm.Parameters.AddWithValue("@CheadName", fullname);
                sqlcomm.ExecuteNonQuery();

                return RedirectToAction("ShowAdd");
                //return View();
            }
            else
                return RedirectToAction("Index", "Login");

        }

        [HttpPost]
        public ActionResult Edit2(BorrowsAddModel bow)
        {
            string CeckConDetail;
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string fullname = HttpContext.Session.GetString("fullname").ToString();
            if (bow.CeckConDetail == null)
            {
                CeckConDetail = "-";
            }
            else
                CeckConDetail = bow.CeckConDetail;
            if (HttpContext.Session.GetString("Username") != null)
            {
                sqlconn.Open();
                string sql = @"UPDATE BorrowsAdd SET CeckCon = @CeckCon,CeckConDetail = @CeckConDetail ,CeckConName = @CeckConName WHERE Id =@Id ";
                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                sqlcomm.Parameters.AddWithValue("@Id", bow.Id);
                sqlcomm.Parameters.AddWithValue("@CeckCon", bow.CeckCon);
                sqlcomm.Parameters.AddWithValue("@CeckConDetail", CeckConDetail);
                sqlcomm.Parameters.AddWithValue("@CeckConName", fullname);
                sqlcomm.ExecuteNonQuery();

                return RedirectToAction("ShowAdd");
                //return View();
            }
            else
                return RedirectToAction("Index", "Login");

        }

        [HttpPost]
        public ActionResult Edit3(BorrowsAddModel bow)
        {
            string CCHeadDetail;
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string fullname = HttpContext.Session.GetString("fullname").ToString();
            if (bow.CCHeadDetail == null)
            {
                CCHeadDetail = "-";
            }
            else
                CCHeadDetail = bow.CCHeadDetail;
            if (HttpContext.Session.GetString("Username") != null)
            {
                sqlconn.Open();
                string sql = @"UPDATE BorrowsAdd SET CCHead = @CCHead,CCHeadDetail = @CCHeadDetail ,CCHeadName = @CCHeadName WHERE Id =@Id ";
                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                sqlcomm.Parameters.AddWithValue("@Id", bow.Id);
                sqlcomm.Parameters.AddWithValue("@CCHead", bow.CCHead);
                sqlcomm.Parameters.AddWithValue("@CCHeadDetail", CCHeadDetail);
                sqlcomm.Parameters.AddWithValue("@CCHeadName", fullname);
                sqlcomm.ExecuteNonQuery();

                return RedirectToAction("ShowAdd");
                //return View();
            }
            else
                return RedirectToAction("Index", "Login");

        }
        public List<ListHardWare> Hardware(int id)
        {

            string Psql = @"SELECT ba.Id,bh.B_Name,bh.B_number,bh.B_Id,d.Status FROM BorrowsAdd ba
                            INNER JOIN dataBorrows d on d.Bor_id = ba.Id
                            INNER JOIN BorrowsHardware bh on bh.B_Id = d.B_Id
                            WHERE ba.Id = " + id;

            List<ListHardWare> list = new List<ListHardWare>();
            SqlCommand sqlcomm = new SqlCommand(Psql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                ListHardWare br = new ListHardWare();

                br.Id = Convert.ToInt32(sdr["Id"]);
                br.Names = sdr["B_Name"].ToString();
                br.Number = sdr["B_number"].ToString();
                br.B_Id = Convert.ToInt32(sdr["B_Id"]);
                br.Status = Convert.ToInt32(sdr["Status"]);

                list.Add(br);
            }
            sqlconn.Close();
            return list;
        }

        public ActionResult Back(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            if (HttpContext.Session.GetString("Username") != null)
            {
                BorrowsAddModel brAdd = new BorrowsAddModel();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"SELECT * FROM BorrowsAdd WHERE Id =@BID";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@BID", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    brAdd.Id = Convert.ToInt32(dt.Rows[0][0]);
                    brAdd.Subject = dt.Rows[0][1].ToString();
                    brAdd.Dept = dt.Rows[0][2].ToString();
                    brAdd.Objectives = dt.Rows[0][3].ToString();
                    brAdd.Sdate = Convert.ToDateTime(dt.Rows[0][4]);
                    brAdd.Edate = Convert.ToDateTime(dt.Rows[0][5]);
                    brAdd.NameAdd = dt.Rows[0][6].ToString();
                    brAdd.Checks = dt.Rows[0][7].ToString();
                    brAdd.Cdetail = dt.Rows[0][8].ToString();
                    brAdd.Cname = dt.Rows[0][9].ToString();
                    brAdd.CheadCheck = dt.Rows[0][10].ToString();
                    brAdd.CheadDetail = dt.Rows[0][11].ToString();
                    brAdd.CheadName = dt.Rows[0][12].ToString();
                    brAdd.CeckCon = dt.Rows[0][13].ToString();
                    brAdd.CeckConDetail = dt.Rows[0][14].ToString();
                    brAdd.CeckConName = dt.Rows[0][15].ToString();
                    brAdd.CCHead = dt.Rows[0][16].ToString();
                    brAdd.CCHeadDetail = dt.Rows[0][17].ToString();
                    brAdd.CCHeadName = dt.Rows[0][18].ToString();
                    brAdd.Dates = dt.Rows[0][19].ToString();
                    brAdd.Notebook = Convert.ToBoolean(dt.Rows[0][20]);
                    brAdd.Tablet = Convert.ToBoolean(dt.Rows[0][21]);
                    brAdd.Camera = Convert.ToBoolean(dt.Rows[0][22]);
                    brAdd.Tripod = Convert.ToBoolean(dt.Rows[0][23]);
                    brAdd.Speaker = Convert.ToBoolean(dt.Rows[0][24]);
                    brAdd.Projector = Convert.ToBoolean(dt.Rows[0][25]);
                    brAdd.Users = dt.Rows[0][26].ToString();
                    brAdd.Status = Convert.ToInt32(dt.Rows[0][27]);
                    brAdd.Tel = Convert.ToInt32(dt.Rows[0][28]);
                    brAdd.n_Notebook = Convert.ToInt32(dt.Rows[0][29]);
                    brAdd.n_Tablet = Convert.ToInt32(dt.Rows[0][30]);
                    brAdd.n_Camera = Convert.ToInt32(dt.Rows[0][31]);
                    brAdd.n_Tripod = Convert.ToInt32(dt.Rows[0][32]);
                    brAdd.n_Speaker = Convert.ToInt32(dt.Rows[0][33]);
                    brAdd.n_Projector = Convert.ToInt32(dt.Rows[0][34]);

                    sqlconn.Close();

                    ViewData["Res"] = BorrowDisplay2(brAdd.Dates, brAdd.Sdate, brAdd.Edate);
                    ViewData["HardWare"] = Hardware(brAdd.Id);
                    return View(brAdd);
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult Back(int[] B_Id, int BA_Id, string tdate, BorrowsAddModel bow)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            string fullname = HttpContext.Session.GetString("fullname").ToString();
            string EndBor = date.changeDatetime(DateTime.Now);
            int B_mor;
            int B_after;

            string CeckConDetail;
            if (bow.CeckConDetail == null)
            {
                CeckConDetail = "-";
            }
            else
                CeckConDetail = bow.CeckConDetail;

            if (HttpContext.Session.GetString("Username") != null)
            {
                sqlconn.Open();
                for (int i = 0; i < B_Id.Length; i++)
                {
                    string sql = @"UPDATE dataBorrows SET Status = 0 ,EndBor = '" + EndBor + "' WHERE Bor_id=@Id AND B_Id =@B_Id";
                    SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                    sqlcomm.Parameters.AddWithValue("@Id", BA_Id);
                    sqlcomm.Parameters.AddWithValue("@B_Id", B_Id[i]);

                    int rowsAffected = sqlcomm.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        if (tdate == "ทั้งวัน")
                        {
                            B_mor = 0;
                            B_after = 0;
                            string StrSQL = @"UPDATE BorrowsHardware SET B_mor =" + B_mor + ", B_after=" + B_after + " WHERE B_Id = " + B_Id[i] + "  ";
                            SqlCommand sqlcom = new SqlCommand(StrSQL, sqlconn);
                            sqlcom.ExecuteNonQuery();
                        }
                        else if (tdate == "ครึ่งวันเช้า")
                        {
                            B_mor = 0;
                            string StrSQL = @"UPDATE BorrowsHardware SET B_mor =" + B_mor + " WHERE B_Id = " + B_Id[i] + "  ";
                            SqlCommand sqlcom = new SqlCommand(StrSQL, sqlconn);
                            sqlcom.ExecuteNonQuery();
                        }
                        else if (tdate == "ครึ่งวันบ่าย")
                        {
                            B_after = 0;
                            string StrSQL = @"UPDATE BorrowsHardware SET B_after=" + B_after + " WHERE B_Id = " + B_Id[i] + "  ";
                            SqlCommand sqlcom = new SqlCommand(StrSQL, sqlconn);
                            sqlcom.ExecuteNonQuery();
                        }
                    }
                }
                string sql2 = @"UPDATE BorrowsAdd SET Status =2,CeckCon = @CeckCon,CeckConDetail = @CeckConDetail,CeckConName = @CeckConName  WHERE Id =@id";
                SqlCommand sqlcomm2 = new SqlCommand(sql2, sqlconn);

                sqlcomm2.Parameters.AddWithValue("@Id", BA_Id);
                sqlcomm2.Parameters.AddWithValue("@CeckCon", bow.CeckCon);
                sqlcomm2.Parameters.AddWithValue("@CeckConDetail", CeckConDetail);
                sqlcomm2.Parameters.AddWithValue("@CeckConName", fullname);

                sqlcomm2.ExecuteNonQuery();

                sqlconn.Close();
                return RedirectToAction("Back", new { id = BA_Id });

            }
            return RedirectToAction("Index", "Login");
        }


        public ActionResult CloseJob(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            if (HttpContext.Session.GetString("Username") != null)
            {
                BorrowsAddModel brAdd = new BorrowsAddModel();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"SELECT * FROM BorrowsAdd WHERE Id =@BID";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@BID", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    brAdd.Id = Convert.ToInt32(dt.Rows[0][0]);
                    brAdd.Subject = dt.Rows[0][1].ToString();
                    brAdd.Dept = dt.Rows[0][2].ToString();
                    brAdd.Objectives = dt.Rows[0][3].ToString();
                    brAdd.Sdate = Convert.ToDateTime(dt.Rows[0][4]);
                    brAdd.Edate = Convert.ToDateTime(dt.Rows[0][5]);
                    brAdd.NameAdd = dt.Rows[0][6].ToString();
                    brAdd.Checks = dt.Rows[0][7].ToString();
                    brAdd.Cdetail = dt.Rows[0][8].ToString();
                    brAdd.Cname = dt.Rows[0][9].ToString();
                    brAdd.CheadCheck = dt.Rows[0][10].ToString();
                    brAdd.CheadDetail = dt.Rows[0][11].ToString();
                    brAdd.CheadName = dt.Rows[0][12].ToString();
                    brAdd.CeckCon = dt.Rows[0][13].ToString();
                    brAdd.CeckConDetail = dt.Rows[0][14].ToString();
                    brAdd.CeckConName = dt.Rows[0][15].ToString();
                    brAdd.CCHead = dt.Rows[0][16].ToString();
                    brAdd.CCHeadDetail = dt.Rows[0][17].ToString();
                    brAdd.CCHeadName = dt.Rows[0][18].ToString();
                    brAdd.Dates = dt.Rows[0][19].ToString();
                    brAdd.Notebook = Convert.ToBoolean(dt.Rows[0][20]);
                    brAdd.Tablet = Convert.ToBoolean(dt.Rows[0][21]);
                    brAdd.Camera = Convert.ToBoolean(dt.Rows[0][22]);
                    brAdd.Tripod = Convert.ToBoolean(dt.Rows[0][23]);
                    brAdd.Speaker = Convert.ToBoolean(dt.Rows[0][24]);
                    brAdd.Projector = Convert.ToBoolean(dt.Rows[0][25]);
                    brAdd.Users = dt.Rows[0][26].ToString();
                    brAdd.Status = Convert.ToInt32(dt.Rows[0][27]);
                    brAdd.Tel = Convert.ToInt32(dt.Rows[0][28]);
                    brAdd.n_Notebook = Convert.ToInt32(dt.Rows[0][29]);
                    brAdd.n_Tablet = Convert.ToInt32(dt.Rows[0][30]);
                    brAdd.n_Camera = Convert.ToInt32(dt.Rows[0][31]);
                    brAdd.n_Tripod = Convert.ToInt32(dt.Rows[0][32]);
                    brAdd.n_Speaker = Convert.ToInt32(dt.Rows[0][33]);
                    brAdd.n_Projector = Convert.ToInt32(dt.Rows[0][34]);

                    sqlconn.Close();

                    ViewData["Res"] = BorrowDisplay2(brAdd.Dates, brAdd.Sdate, brAdd.Edate);
                    ViewData["HardWare"] = Hardware(brAdd.Id);
                    return View(brAdd);
                }
                else
                    return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult CloseJob(int id, BorrowsAddModel bor)
        {
            string CCHeadDetail;
            if (bor.CCHeadDetail == null)
            {
                CCHeadDetail = "-";
            }
            else
                CCHeadDetail = bor.CCHeadDetail;

            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            string fullname = HttpContext.Session.GetString("fullname").ToString();

            sqlconn.Open();

            string sql2 = @"UPDATE BorrowsAdd SET Status =3,CCHead = @CCHead,CCHeadDetail = @CCHeadDetail,CCHeadName = @CCHeadName  WHERE Id =@id";
            SqlCommand sqlcomm = new SqlCommand(sql2, sqlconn);

            sqlcomm.Parameters.AddWithValue("@Id", id);
            sqlcomm.Parameters.AddWithValue("@CCHead", bor.CCHead);
            sqlcomm.Parameters.AddWithValue("@CCHeadDetail", CCHeadDetail);
            sqlcomm.Parameters.AddWithValue("@CCHeadName", fullname);

            sqlcomm.ExecuteNonQuery();

            sqlconn.Close();
            return RedirectToAction("ShowAdd");
        }

        public List<Login> NameAdd()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            List<Login> NameAdd = new List<Login>();
            string Ssql = "SELECT * FROM UserProfile WHERE chiefoff = '2'";
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                Login Add = new Login();


                Add.fullname = sdr["fullname"].ToString();

                NameAdd.Add(Add);
            }
            sqlconn.Close();
            return NameAdd;
        }
    }
}
