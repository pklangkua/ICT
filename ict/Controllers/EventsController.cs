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
    public class EventsController : Controller
    {
        SqlConnection sqlconn = new DBClass().SqlStrCon();
        LineNotification sent = new LineNotification();
        Datetime date = new Datetime();

        public IEnumerable<object> WebFile { get; private set; }

        public ActionResult Index()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            TempData["URL"] = "Events";
            return View();
        }
        public ActionResult Create()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewData["State"] = State();
                return View();
            }
            else
            {
                TempData["URL"] = "Events/Create";
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult Create(EventsModel ev)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            string e_sdate = date.changeDatetime(ev.e_sdate);
            string e_edate = date.changeDatetime(ev.e_edate);
            string users = HttpContext.Session.GetString("Username");
            ViewData["State"] = State();

            try
            {
                sqlconn.Open();
                string sql = @"INSERT INTO Events VALUES(@e_name,@e_sdate,@e_edate,@e_state,
                                                         @e_create,@e_dept,@e_other,@e_president,GETDATE(),@Users,@URL,@Color)";
                //string sql = @"INSERT INTO Events VALUES("+ev.e_name+","+ ev.e_sdate +","+ev.e_edate +","+
                //                                         ev.e_state+","+ev.e_create+","+ ev.e_dept+","+ev.e_other+
                //                                         ","+ ev.e_president+","+ ev.e_dateCreate+","+ users+","+ev.URL+","+ev.Color+")";

                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);
                sqlcomm.Parameters.AddWithValue("@e_name", ev.e_name);
                sqlcomm.Parameters.AddWithValue("@e_sdate", ev.e_sdate);
                sqlcomm.Parameters.AddWithValue("@e_edate", ev.e_edate);
                sqlcomm.Parameters.AddWithValue("@e_state", ev.e_state);
                sqlcomm.Parameters.AddWithValue("@e_create", ev.e_create);
                sqlcomm.Parameters.AddWithValue("@e_dept", ev.e_dept);
                sqlcomm.Parameters.AddWithValue("@e_other", ev.e_other);
                sqlcomm.Parameters.AddWithValue("@e_president", ev.e_president);
                //sqlcomm.Parameters.AddWithValue("@e_dateCreate", ev.e_edate);
                sqlcomm.Parameters.AddWithValue("@Users", users);
                sqlcomm.Parameters.AddWithValue("@URL", "Events/Detail/");
                sqlcomm.Parameters.AddWithValue("@Color", "#1e90ff");
                sqlcomm.ExecuteNonQuery();
                sqlconn.Close();

                return RedirectToAction("ShowEvents");
            }
            catch
            {
                return View();
            }
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

        public ActionResult Detail(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            ViewData["DetailShows"] = DetailShows((int)id);

            EventsModel ev = new EventsModel();
            DataTable dt = new DataTable();
            sqlconn.Open();
            string sql = @"SELECT
                            dbo.Events.e_id,
                            dbo.Events.e_name,
                            dbo.Events.e_sdate,
                            dbo.Events.e_edate,
                            dbo.Events.e_other,
                            dbo.Events.e_president,
                            dbo.State.S_Name

                            FROM
                            dbo.Events
                            INNER JOIN dbo.State ON dbo.State.S_Id = dbo.Events.e_state 
                            WHERE e_id=@e_id";
            SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
            sda.SelectCommand.Parameters.AddWithValue("@e_id", id);
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                ev.e_id = Convert.ToInt32(dt.Rows[0][0]);
                ev.e_name = dt.Rows[0][1].ToString();
                ev.e_sdate = Convert.ToDateTime(dt.Rows[0][2]);
                ev.e_edate = Convert.ToDateTime(dt.Rows[0][3]);
                ev.e_other = dt.Rows[0][4].ToString();
                ev.e_president = dt.Rows[0][5].ToString();
                ev.e_state_name = dt.Rows[0][6].ToString();

                return View(ev);
            }
            else
                return RedirectToAction("Index");

        }

        public ActionResult ShowEvents()
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewData["Results"] = DataShows();
                return View();
            }
            else
            {
                TempData["URL"] = "Events/ShowEvents";
                return RedirectToAction("Index", "Login");
            }
        }

        public List<EventsModel> DataShows()
        {
            List<EventsModel> ev = new List<EventsModel>();
            string Ssql = @"SELECT
                            dbo.Events.e_id,
                            dbo.Events.e_name,
                            dbo.Events.e_sdate,
                            dbo.Events.e_edate,
                            dbo.Events.e_other,
                            dbo.Events.e_president,
                            dbo.State.S_Name

                            FROM
                            dbo.Events
                            INNER JOIN dbo.State ON dbo.State.S_Id = dbo.Events.e_state
                            ORDER BY e_id DESC";
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                EventsModel evs = new EventsModel();

                evs.e_id = Convert.ToInt32(sdr["e_id"]);
                evs.e_name = sdr["e_name"].ToString();
                evs.e_sdate = Convert.ToDateTime(sdr["e_sdate"]);
                evs.e_edate = Convert.ToDateTime(sdr["e_edate"]);
                evs.e_other = sdr["e_other"].ToString();
                evs.e_president = sdr["e_president"].ToString();
                evs.e_state_name = sdr["S_Name"].ToString();

                ev.Add(evs);
            }
            sqlconn.Close();
            return ev;
        }

        public ActionResult Createfile(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();
            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewData["termData"] = termData();
                ViewData["EData"] = Edata((int)id);
                ViewData["DetailShows"] = DetailShows((int)id);

                return View();
            }
            else
            {
                TempData["URL"] = "Events/Createfile";
                return RedirectToAction("Index", "Login");
            }
        }

        public List<termModel> termData()
        {
            List<termModel> tr = new List<termModel>();
            string Ssql = @"SELECT * FROM term";
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                termModel trs = new termModel();

                trs.t_id = Convert.ToInt32(sdr["t_id"]);
                trs.t_name = sdr["t_name"].ToString();

                tr.Add(trs);
            }
            sqlconn.Close();
            return tr;
        }

        public string Edata(int id)
        {
            string e_name = "";
            DataTable dt = new DataTable();
            sqlconn.Open();
            string sql = @"SELECT
                            dbo.Events.e_id,
                            dbo.Events.e_name,
                            dbo.Events.e_sdate,
                            dbo.Events.e_edate,
                            dbo.Events.e_other,
                            dbo.Events.e_president,
                            dbo.State.S_Name

                            FROM
                            dbo.Events
                            INNER JOIN dbo.State ON dbo.State.S_Id = dbo.Events.e_state 
                            WHERE e_id=@e_id";
            SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
            sda.SelectCommand.Parameters.AddWithValue("@e_id", id);
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                e_name = dt.Rows[0][1].ToString();

            }
            sqlconn.Close();
            return e_name.ToString();
        }

        [HttpPost]
        public async Task<IActionResult> Createfile(List<IFormFile> ed_file, EventsDetailModel ed, int? id)
        {
            var size = ed_file.Sum(f => f.Length);
            var filePaths = new List<string>();

            ViewData["termData"] = termData();
            ViewData["EData"] = Edata(ed.e_id);
            ViewData["DetailShows"] = DetailShows((int)id);
            if (ed_file.Count == 0)
            {
                sqlconn.Open();
                //string sql = @"INSERT INTO Web VALUES(@WebSub,@WebDetail,@WebFile,GETDATE(),'" + Users + "')";

                string sql = @"INSERT INTO EventsDetail VALUES(@ed_name,@ed_detail,@ed_file,GETDATE(),@t_id,@ed_num,@e_id)";
                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                sqlcomm.Parameters.AddWithValue("@ed_name", ed.ed_name);
                sqlcomm.Parameters.AddWithValue("@ed_detail", ed.ed_detail);
                sqlcomm.Parameters.AddWithValue("@ed_file", "");
                sqlcomm.Parameters.AddWithValue("@t_id", ed.t_id);
                sqlcomm.Parameters.AddWithValue("@ed_num", ed.ed_num);
                sqlcomm.Parameters.AddWithValue("@e_id", id);

                sqlcomm.ExecuteNonQuery();
                sqlconn.Close();
            }
            else
            {
                foreach (var formFile in ed_file)
                {
                    if (formFile.Length > 0)
                    {
                        string FileName = Path.GetRandomFileName() + Path.GetFileNameWithoutExtension(formFile.FileName) + Path.GetExtension(formFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/File", FileName);
                        var type = Path.GetExtension(formFile.FileName);
                        //string temp = Path.GetTempFileName(filePath);

                        if (type != ".pdf" && type != ".doc" && type != ".docx" && type != ".xls" && type != ".xlsx" && type != ".ptt" && type != ".pttx")
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
                                //string sql = @"INSERT INTO Web VALUES(@WebSub,@WebDetail,@WebFile,GETDATE(),'" + Users + "')";
                                string sql = @"INSERT INTO EventsDetail VALUES(@ed_name,@ed_detail,@ed_file,GETDATE(),@t_id,@ed_num,@e_id)";
                                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                                sqlcomm.Parameters.AddWithValue("@ed_name", ed.ed_name);
                                sqlcomm.Parameters.AddWithValue("@ed_detail", ed.ed_detail);
                                sqlcomm.Parameters.AddWithValue("@ed_file", FileName);
                                sqlcomm.Parameters.AddWithValue("@t_id", ed.t_id);
                                sqlcomm.Parameters.AddWithValue("@ed_num", ed.ed_num);
                                sqlcomm.Parameters.AddWithValue("@e_id", id);

                                sqlcomm.ExecuteNonQuery();
                                sqlconn.Close();
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Createfile");
        }

        public List<EventsDetailModel> DetailShows(int e_id)
        {
            sqlconn.Open();
            List<EventsDetailModel> ed = new List<EventsDetailModel>();
            string sql = @"select  t.v " +
                           "from (SELECT DISTINCT ed_num  FROM EventsDetail ec where ec.e_id=" + e_id + " ) as t(v)" +
                           " cross apply ( " +
                           " select x = cast('<i>' + replace(v, '.', '</i><i>') + '</i>' as xml)" +
                           " ) x " +
                           " order by x.value('i[1]','int'),x.value('i[2]','int'), x.value('i[3]','int')";
            SqlCommand sqlcomm = new SqlCommand(sql);
            sqlcomm.Connection = sqlconn;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                string Ssql = @"SELECT  ev.e_name, ed.ed_name, ed.ed_file, ed.ed_num,t.t_id ,ed.ed_id FROM
                                                dbo.EventsDetail AS ed
                                                INNER JOIN dbo.Events AS ev ON ev.e_id = ed.e_id
                                                INNER JOIN term t on t.t_id=ed.t_id           
                                                WHERE ed.ed_num='" + sdr["v"] + "' AND ed.e_id=" + e_id;
                SqlCommand sqlcomm2 = new SqlCommand(Ssql);
                sqlcomm2.Connection = sqlconn;

                SqlDataReader sdr2 = sqlcomm2.ExecuteReader();
                while (sdr2.Read())
                {
                    EventsDetailModel eds = new EventsDetailModel();

                    eds.ed_name = sdr2["ed_name"].ToString();
                    eds.ed_file = sdr2["ed_file"].ToString();
                    eds.ed_num = sdr2["ed_num"].ToString();
                    eds.t_id = Convert.ToInt32(sdr2["t_id"]);
                    eds.ed_id = Convert.ToInt32(sdr2["ed_id"]);

                    ed.Add(eds);
                }
            }
            sqlconn.Close();
            return ed;
        }

        public ActionResult DeleteDetail(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            if (HttpContext.Session.GetString("Username") != null)
            {
                EventsDetailModel ed = new EventsDetailModel();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"SELECT
                                dbo.EventsDetail.ed_id,
                                dbo.EventsDetail.ed_name,
                                dbo.EventsDetail.ed_detail,
                                dbo.EventsDetail.ed_file,
                                dbo.EventsDetail.e_id

                                FROM EventsDetail
                                WHERE ed_id =@ed_id";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@ed_id", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    ed.ed_id = Convert.ToInt32(dt.Rows[0][0]);
                    ed.ed_name = dt.Rows[0][1].ToString();
                    ed.ed_detail = dt.Rows[0][2].ToString();
                    ed.ed_file = dt.Rows[0][3].ToString();
                    ed.e_id = Convert.ToInt32(dt.Rows[0][4]);

                }
                else
                    RedirectToAction("Createfile", new { id = ed.e_id });
                return View(ed);
            }
            else
            {
                TempData["URL"] = "Events/DeleteDetail";
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult DeleteDetail(int ed_id, string ed_file, int e_id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            sqlconn.Open();
            string sql = @"delete from EventsDetail where ed_id=@ed_id ";
            SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

            sqlcomm.Parameters.AddWithValue("@ed_id", ed_id);
            if (ed_file is null)
            {
                sqlcomm.ExecuteNonQuery();
            }
            else
            {
                var filePath = Path.Combine("wwwroot/File", ed_file);
                System.IO.File.Delete(filePath);
                sqlcomm.ExecuteNonQuery();
            }

            sqlconn.Close();
            return RedirectToAction("Createfile", new { id = e_id });
        }

        public ActionResult Delete(int? id)
        {
            ViewData["Username"] = HttpContext.Session.GetString("Username");
            ViewData["fullname"] = HttpContext.Session.GetString("fullname");
            ViewData["OfficeName"] = HttpContext.Session.GetString("OfficeName");
            ViewBag.Message = HttpContext.Session.GetInt32("IsActive").ToString();

            if (HttpContext.Session.GetString("Username") != null)
            {
                EventsModel ev = new EventsModel();
                DataTable dt = new DataTable();
                sqlconn.Open();
                string sql = @"SELECT e_id,e_name FROM Events WHERE e_id =@e_id";
                SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
                sda.SelectCommand.Parameters.AddWithValue("@e_id", id);
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    ev.e_id = Convert.ToInt32(dt.Rows[0][0]);
                    ev.e_name = dt.Rows[0][1].ToString();

                }
                else
                    RedirectToAction("ShowEvents");
                return View(ev);
            }
            else
            {
                TempData["URL"] = "Events/DeleteDetail";
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public ActionResult Delete(int e_id)
        {
            sqlconn.Open();
            string sql = @" BEGIN TRANSACTION;   
                                delete from Events where e_id=@e_id;
                                delete from EventsDetail where e_id=@e_id;
                            COMMIT TRANSACTION;   ";
            SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

            sqlcomm.Parameters.AddWithValue("@e_id", e_id);
            sqlcomm.ExecuteNonQuery();

            sqlconn.Close();
            return RedirectToAction("ShowEvents");
        }
    }
}
