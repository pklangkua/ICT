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
    public class ConferenceController : Controller
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
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            if (Users != null)
            {
                ViewData["Results"] = ConferenceDisplay2();
                return View();
            }
            else
            {
                ViewData["Results"] = ConferenceDisplay();
                return View();
            }
        }
        public IActionResult Create()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewData["Officedata"] = OfficeDisplay();
                ViewData["State"] = State();
                return View();
            }
            else
            {
                TempData["URL"] = "Conference/Create";
                //return RedirectToAction("Index", "Login", new { page = "/Conference/Create" });
                return RedirectToAction("Index", "Login");
            }

        }
        public List<ConferenceModel> ConferenceDisplay()
        {
            List<ConferenceModel> con = new List<ConferenceModel>();
            string Ssql = "select c.*,s.S_Name from conference c INNER JOIN State s on s.S_Id = c.DtState order by ConID desc";
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            //SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
           // sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                ConferenceModel conf = new ConferenceModel();

                conf.ConID = Convert.ToInt32(sdr["ConID"]);
                conf.ConName = sdr["ConName"].ToString();
                conf.ConDept = sdr["ConDept"].ToString();
                conf.ConTel = sdr["ConTel"].ToString();
                conf.Participant = sdr["Participant"].ToString();
                conf.DtDate = Convert.ToDateTime(sdr["DtDate"]);
                conf.DtState = sdr["S_Name"].ToString();
                conf.Status = Convert.ToInt32(sdr["Status"]);
                conf.DtTitle = sdr["DtTitle"].ToString();
                conf.Users = sdr["Users"].ToString();

                con.Add(conf);
            }
            sqlconn.Close();
            return con;
        }
        public List<ConferenceModel> ConferenceDisplay2()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string Users = HttpContext.Session.GetString("Username");
            string IsActive = HttpContext.Session.GetInt32("IsActive").ToString();
            string Ssql;
            List<ConferenceModel> con = new List<ConferenceModel>();
            if (IsActive == "2")
            {
                Ssql = "select c.*,s.S_Name from conference c INNER JOIN State s on s.S_Id = c.DtState order by ConID desc";
            }
            else
            {
                Ssql = "select c.*,s.S_Name from conference c INNER JOIN State s on s.S_Id = c.DtState where Users =@Users order by ConID desc";
            }
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            sqlcomm.Parameters.AddWithValue("@Users", Users);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                ConferenceModel conf = new ConferenceModel();

                conf.ConID = Convert.ToInt32(sdr["ConID"]);
                conf.ConName = sdr["ConName"].ToString();
                conf.ConDept = sdr["ConDept"].ToString();
                conf.ConTel = sdr["ConTel"].ToString();
                conf.Participant = sdr["Participant"].ToString();
                conf.DtDate = Convert.ToDateTime(sdr["DtDate"]);
                conf.DtState = sdr["S_Name"].ToString();
                conf.Status = Convert.ToInt32(sdr["Status"]);
                conf.DtTitle = sdr["DtTitle"].ToString();

                con.Add(conf);
            }
            return con;
        }

        [HttpPost]
        public IActionResult Create(ConferenceModel ur)
        {
            string DtDate = date.changeDatetime(ur.DtDate);
            string DtDate2 = date.changeDatetime(ur.DtDate2);
            string Users = HttpContext.Session.GetString("Username");
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");

            ViewData["State"] = State();

            try
            {
                sqlconn.Open();
                string sql = @"INSERT INTO conference VALUES(@ConName,@ConDept,@ConTel,@Participant,@DtDate,@DtState,
                                @DtCuser,@DtTitle,@DtPres,@DtName,@DtPosition,@DtTel,@DtEmail,@DtControl,@DtPos2,@DtTel2,
                                @DtDate2,@Status,@Users,@Reason,@URL,@Color,@Borrow)";
                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                sqlcomm.Parameters.AddWithValue("@ConName", ur.ConName);
                sqlcomm.Parameters.AddWithValue("@ConDept", ur.ConDept);
                sqlcomm.Parameters.AddWithValue("@ConTel", ur.ConTel);
                sqlcomm.Parameters.AddWithValue("@Participant", ur.Participant);
                sqlcomm.Parameters.AddWithValue("@DtDate", ur.DtDate);
                sqlcomm.Parameters.AddWithValue("@DtState", ur.DtState);
                sqlcomm.Parameters.AddWithValue("@DtCuser", ur.DtCuser);
                sqlcomm.Parameters.AddWithValue("@DtTitle", ur.DtTitle);
                sqlcomm.Parameters.AddWithValue("@DtPres", ur.DtPres);
                sqlcomm.Parameters.AddWithValue("@DtName", ur.DtName);
                sqlcomm.Parameters.AddWithValue("@DtPosition", ur.DtPosition);
                sqlcomm.Parameters.AddWithValue("@DtTel", ur.DtTel);
                sqlcomm.Parameters.AddWithValue("@DtEmail", ur.DtEmail);
                sqlcomm.Parameters.AddWithValue("@DtControl", ur.DtControl);
                sqlcomm.Parameters.AddWithValue("@DtPos2", ur.DtPos2);
                sqlcomm.Parameters.AddWithValue("@DtTel2", ur.DtTel2);
                sqlcomm.Parameters.AddWithValue("@DtDate2", ur.DtDate2);
                sqlcomm.Parameters.AddWithValue("@Status", 0);
                sqlcomm.Parameters.AddWithValue("@Users", Users);
                sqlcomm.Parameters.AddWithValue("@Reason", " ");
                sqlcomm.Parameters.AddWithValue("@URL", "Conference/Detail/");
                sqlcomm.Parameters.AddWithValue("@Color", "#1e90ff");
                sqlcomm.Parameters.AddWithValue("@Borrow", ur.Borrow);

                int intd2 = sqlcomm.ExecuteNonQuery();
                if (intd2 == 1)
                {
                    string msg2 = @"" + ur.DtTitle + "\n" + "หน่วยงาน : " + ur.ConDept + "\n" + "วันที่ประชุม : " + ur.DtDate + "\n" + "วันที่สิ้นสุดการประชุม : " + ur.DtDate2 + "\n" + "ผู้ควบคุม : " + ur.DtControl;
                    sent.SentLine("AdgZGCxjLPlcI7vU2y0dMgjdWiJdFOcUL1cmsfqqON5", msg2);
                    // AdgZGCxjLPlcI7vU2y0dMgjdWiJdFOcUL1cmsfqqON5
                }
                sqlconn.Close();
                if (ur.Borrow != true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    BorrowsAddModel br = new BorrowsAddModel();
                    br.NameAdd = ur.ConName.ToString();
                    br.Tel = Convert.ToInt32(ur.ConTel);
                    br.Sdate = ur.DtDate;
                    br.Edate = ur.DtDate2;
                    return RedirectToAction("Create", "Borrow", br);
                }

            }
            catch
            {
                return View();
            }

        }
        public ActionResult Delete(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            if (HttpContext.Session.GetString("Username") != null)
            {
                ConferenceModel conf = new ConferenceModel();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"select ConID,DtTitle,DtDate from conference where ConID=@ConID";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@ConID", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    conf.ConID = Convert.ToInt32(dt.Rows[0][0]);
                    conf.DtTitle = dt.Rows[0][1].ToString();
                    conf.DtDate = Convert.ToDateTime(dt.Rows[0][2]);

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
        public ActionResult Delete(int id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            sqlconn.Open();
            string sql = @"delete from conference WHERE ConID =@ConID ";
            SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

            sqlcomm.Parameters.AddWithValue("@ConID", id);

            sqlcomm.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ConferenceModel conf = new ConferenceModel();
            DataTable dt = new DataTable();
            sqlconn.Open();
            string sql = @"select c.*,s.S_Name from conference c INNER JOIN State s on s.S_Id = c.DtState where ConID =@ConID";
            SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
            sda.SelectCommand.Parameters.AddWithValue("@ConID", id);
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                conf.ConID = Convert.ToInt32(dt.Rows[0][0]);
                conf.ConName = dt.Rows[0][1].ToString();
                conf.ConDept = dt.Rows[0][2].ToString();
                conf.ConTel = dt.Rows[0][3].ToString();
                conf.Participant = dt.Rows[0][4].ToString();
                conf.DtDate = Convert.ToDateTime(dt.Rows[0][5]);
                conf.DtState = dt.Rows[0][24].ToString();
                conf.DtCuser = dt.Rows[0][7].ToString();
                conf.DtTitle = dt.Rows[0][8].ToString();
                conf.DtPres = dt.Rows[0][9].ToString();
                conf.DtName = dt.Rows[0][10].ToString();
                conf.DtPosition = dt.Rows[0][11].ToString();
                conf.DtTel = dt.Rows[0][12].ToString();
                conf.DtEmail = dt.Rows[0][13].ToString();
                conf.DtControl = dt.Rows[0][14].ToString();
                conf.DtPos2 = dt.Rows[0][15].ToString();
                conf.DtTel2 = dt.Rows[0][16].ToString();
                conf.DtDate2 = Convert.ToDateTime(dt.Rows[0][17]);
                conf.Status = Convert.ToInt32(dt.Rows[0][18]);
                conf.Users = dt.Rows[0][19].ToString();
                conf.Reason = dt.Rows[0][20].ToString();

                return View(conf);
            }
            else
                return RedirectToAction("Index");
        }
        public ActionResult Update(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            string active = HttpContext.Session.GetInt32("IsActive").ToString();
            if (active == "2")
            {
                ConferenceModel conf = new ConferenceModel();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"select * from conference where ConID =@ConID order by ConID desc";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@ConID", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    conf.ConID = Convert.ToInt32(dt.Rows[0][0]);
                    conf.ConDept = dt.Rows[0][2].ToString();
                    conf.DtDate = Convert.ToDateTime(dt.Rows[0][5]);
                    conf.DtTitle = dt.Rows[0][8].ToString();
                    conf.DtControl = dt.Rows[0][14].ToString();
                    conf.DtPos2 = dt.Rows[0][15].ToString();
                    conf.DtTel2 = dt.Rows[0][16].ToString();
                    conf.DtDate2 = Convert.ToDateTime(dt.Rows[0][17]);
                    conf.Status = Convert.ToInt32(dt.Rows[0][18]);
                    conf.Reason = dt.Rows[0][20].ToString();


                    return View(conf);
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
            else
                return RedirectToAction("Index", "Login");

        }

        [HttpPost]
        public ActionResult Update(ConferenceModel con)
        {
            string color = "";
            if (con.Status == 2)
            {
                color = "#02d667";
            }
            else if (con.Status == 1)
            {
                color = "#FF0000";
            }

            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            sqlconn.Open();
            string sql = @"UPDATE conference 
                           SET DtControl = @DtControl, 
                               DtPos2 = @DtPos2 ,
                               DtTel2 = @DtTel2 ,
                               Status = @Status ,
                               Reason = @Reason ,
                               Color  = @Color
                           WHERE ConID =@ConID ";
            SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

            sqlcomm.Parameters.AddWithValue("@ConID", con.ConID);
            sqlcomm.Parameters.AddWithValue("@DtControl", con.DtControl);
            sqlcomm.Parameters.AddWithValue("@DtPos2", con.DtPos2);
            sqlcomm.Parameters.AddWithValue("@DtTel2", con.DtTel2);
            sqlcomm.Parameters.AddWithValue("@Status", con.Status);
            sqlcomm.Parameters.AddWithValue("@Reason", con.Reason);
            sqlcomm.Parameters.AddWithValue("@Color", color);

            int intd2 = sqlcomm.ExecuteNonQuery();

            if (intd2 == 1)
            {
                string msg2 = @"" + con.DtTitle + "\n" + "หน่วยงาน : " + con.ConDept + "\n" + "วันที่ประชุม : " + con.DtDate + "\n" + "วันที่สิ้นสุดการประชุม : " + con.DtDate2 + "\n" + "ผู้ควบคุม : " + con.DtControl + "\n" + "เหตุผลเพิ่มเติม : " + con.Reason;
                sent.SentLine("AdgZGCxjLPlcI7vU2y0dMgjdWiJdFOcUL1cmsfqqON5", msg2);
                // AdgZGCxjLPlcI7vU2y0dMgjdWiJdFOcUL1cmsfqqON5
            }
            return RedirectToAction("Index");
        }

        public List<OfficeModel> OfficeDisplay()
        {
            List<OfficeModel> off = new List<OfficeModel>();
            string Osql = "select * from Office where OrganizationID = 1";
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
            //ViewBag.OffList = off;
            return off;

        }

        public List<StateModel> State()
        {
            List<StateModel> State = new List<StateModel>();
            string sql = "select * from State ";
            SqlCommand sqlcomm = new SqlCommand(sql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                StateModel States = new StateModel();

                States.S_Id = Convert.ToInt32(sdr["S_Id"]);
                States.S_Name = sdr["S_Name"].ToString();

                State.Add(States);
            }
            //ViewBag.OffList = off;
            sqlconn.Close();
            return State;

        }

        public ActionResult Waiting()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["Results"] = WaitingDisplay(0);
            return View();
        }
        public ActionResult Approve()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["Results"] = WaitingDisplay(2);
            return View();
        }
        public ActionResult DisApprove()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["Results"] = WaitingDisplay(1);
            return View();
        }

        public List<ConferenceModel> WaitingDisplay(int st)
        {
            string Uername = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            string IsActive = HttpContext.Session.GetInt32("IsActive").ToString();
            string Ssql;
            List<ConferenceModel> con = new List<ConferenceModel>();
            if (IsActive == "2")
            {
                Ssql = "select c.*,s.S_Name from conference c INNER JOIN State s on s.S_Id = c.DtState where c.Status ='" + st + "' order by c.ConID desc";
            }
            else
            {
                Ssql = "select c.*,s.S_Name from conference c INNER JOIN State s on s.S_Id = c.DtState where c.Users='" + Uername + "' AND c.Status ='" + st + "' order by c.ConID desc";
            }
            //string Ssql = "select * from conference where Users='" + Uername + "' AND Status ='" + st + "'";
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                ConferenceModel conf = new ConferenceModel();

                conf.ConID = Convert.ToInt32(sdr["ConID"]);
                conf.ConName = sdr["ConName"].ToString();
                conf.ConDept = sdr["ConDept"].ToString();
                conf.ConTel = sdr["ConTel"].ToString();
                conf.Participant = sdr["Participant"].ToString();
                conf.DtDate = Convert.ToDateTime(sdr["DtDate"]);
                conf.DtState = sdr["S_Name"].ToString();
                conf.Status = Convert.ToInt32(sdr["Status"]);
                conf.DtTitle = sdr["DtTitle"].ToString();

                con.Add(conf);
            }
            return con;
        }

        public ActionResult Edit(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewData["Officedata"] = OfficeDisplay();
                ViewData["State"] = State();

                ViewData["Username"] = HttpContext.Session.GetString("Username");
                ViewData["fullname"] = HttpContext.Session.GetString("fullname");
                ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
                ConferenceModel conf = new ConferenceModel();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"select c.*,s.S_Name from conference c INNER JOIN State s on s.S_Id = c.DtState where ConID =@ConID";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@ConID", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    conf.ConID = Convert.ToInt32(dt.Rows[0][0]);
                    conf.ConName = dt.Rows[0][1].ToString();
                    conf.ConDept = dt.Rows[0][2].ToString();
                    conf.ConTel = dt.Rows[0][3].ToString();
                    conf.Participant = dt.Rows[0][4].ToString();
                    conf.DtDate = Convert.ToDateTime(dt.Rows[0][5]);
                    conf.DtState = dt.Rows[0][6].ToString();
                    conf.DtCuser = dt.Rows[0][7].ToString();
                    conf.DtTitle = dt.Rows[0][8].ToString();
                    conf.DtPres = dt.Rows[0][9].ToString();
                    conf.DtName = dt.Rows[0][10].ToString();
                    conf.DtPosition = dt.Rows[0][11].ToString();
                    conf.DtTel = dt.Rows[0][12].ToString();
                    conf.DtEmail = dt.Rows[0][13].ToString();
                    conf.DtControl = dt.Rows[0][14].ToString();
                    conf.DtPos2 = dt.Rows[0][15].ToString();
                    conf.DtTel2 = dt.Rows[0][16].ToString();
                    conf.DtDate2 = Convert.ToDateTime(dt.Rows[0][17]);
                    conf.Status = Convert.ToInt32(dt.Rows[0][18]);
                    conf.Users = dt.Rows[0][19].ToString();
                    conf.Reason = dt.Rows[0][20].ToString();

                    sqlconn.Close();
                    return View(conf);
                }
                else
                    return RedirectToAction("Index");

            }
            else
                return RedirectToAction("Index", "Login");
        }


    }
}