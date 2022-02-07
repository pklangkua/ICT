using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using System.Data;
using ict.Models;
using Microsoft.AspNetCore.Http;

namespace ict.Controllers
{
    public class EmplyeeController : Controller
    {

        SqlConnection sqlconn = new DBClass().SqlStrCon();
        Datetime date = new Datetime();
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                ViewData["Results"] = TableDisplay();
                return View();
            }
            else
                return RedirectToAction("Index","Login");
            
        }
        public IActionResult Create()
        {
            return View();
        }
        public ActionResult Edit(int? id)
        {
            Emplyee emp = new Emplyee();
            DataTable dt = new DataTable();
            sqlconn.Open();
            string sql = @"select * from Employees where EmployeeId=@emid";
            SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
            sda.SelectCommand.Parameters.AddWithValue("@emid", id);
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                emp.EmployeeId = Convert.ToInt32(dt.Rows[0][0]);
                emp.Name = dt.Rows[0][1].ToString();
                emp.Gender = dt.Rows[0][2].ToString();
                emp.Age = Convert.ToInt32(dt.Rows[0][3]);
                emp.Position = dt.Rows[0][4].ToString();
                emp.Office = dt.Rows[0][5].ToString();
                emp.HireDate = Convert.ToDateTime(dt.Rows[0][6]);
                emp.Salary = Convert.ToInt32(dt.Rows[0][7]);

                return View(emp);
            }
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(Emplyee ur)
        {
            sqlconn.Open();
            string sql = @"UPDATE Employees SET Name = @Name,Gender = @Gender,Age = @Age,Position = @Position,
                            Office = @Office,HireDate = @HireDate,Salary = @Salary WHERE EmployeeId =@EmployeeId ";
            SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

            sqlcomm.Parameters.AddWithValue("@EmployeeId", ur.EmployeeId);
            sqlcomm.Parameters.AddWithValue("@Name", ur.Name);
            sqlcomm.Parameters.AddWithValue("@Gender", ur.Gender);
            sqlcomm.Parameters.AddWithValue("@Age", ur.Age);
            sqlcomm.Parameters.AddWithValue("@Position", ur.Position);
            sqlcomm.Parameters.AddWithValue("@Office", ur.Office);
            sqlcomm.Parameters.AddWithValue("@HireDate", date.changeDatetime(ur.HireDate));
            sqlcomm.Parameters.AddWithValue("@Salary", ur.Salary);

            sqlcomm.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Create(Emplyee ur)
        {
            //string sql = "INSERT INTO Emplyee(Name,Gender,Age,Position,Office,HireDate,Salary)" +
            //             " VALUES ('" + ur.Name + "','" + ur.Gender + "','" + ur.Age + "','" +
            //             ur.Position + "','" + ur.Office + "','','" + ur.Salary + "')";
            
            string dateemp = date.changeDatetime(ur.HireDate);
            try
            {
                sqlconn.Open();
                string sql = @"INSERT INTO Employees VALUES(@Name,@Gender,@Age,@Position,@Office,@HireDate,@Salary)";
                SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

                sqlcomm.Parameters.AddWithValue("@Name", ur.Name);
                sqlcomm.Parameters.AddWithValue("@Gender", ur.Gender);
                sqlcomm.Parameters.AddWithValue("@Age", ur.Age);
                sqlcomm.Parameters.AddWithValue("@Position", ur.Position);
                sqlcomm.Parameters.AddWithValue("@Office", ur.Office);
                sqlcomm.Parameters.AddWithValue("@HireDate", dateemp);
                sqlcomm.Parameters.AddWithValue("@Salary", ur.Salary);

                sqlcomm.ExecuteNonQuery();
                // ViewData["Message"] = "Insert success";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }


        }
        public List<Emplyee> TableDisplay()
        {
            List<Emplyee> em = new List<Emplyee>();
            string Ssql = "select * from Employees";
            SqlCommand sqlcomm = new SqlCommand(Ssql);
            SqlDataAdapter sda = new SqlDataAdapter();
            sqlcomm.Connection = sqlconn;
            sqlconn.Open();
            sda.SelectCommand = sqlcomm;

            SqlDataReader sdr = sqlcomm.ExecuteReader();
            while (sdr.Read())
            {
                Emplyee emp = new Emplyee();
                emp.EmployeeId = Convert.ToInt16(sdr["EmployeeId"]);
                emp.Name = sdr["Name"].ToString();
                emp.Gender = sdr["Gender"].ToString();
                emp.Age = Convert.ToInt16(sdr["Age"]);
                emp.Position = sdr["Position"].ToString();
                emp.Office = sdr["Office"].ToString();
                emp.HireDate = Convert.ToDateTime(sdr["HireDate"]);
                emp.Salary = Convert.ToInt32(sdr["Salary"]);
                em.Add(emp);
            }
            return em;
        }

        public ActionResult Delete(int? id)
        {
            Emplyee emp = new Emplyee();
            DataTable dt = new DataTable();
            sqlconn.Open();
            string sql = @"select * from Employees where EmployeeId=@emid";
            SqlDataAdapter sda = new SqlDataAdapter(sql, sqlconn);
            sda.SelectCommand.Parameters.AddWithValue("@emid", id);
            sda.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                emp.EmployeeId = Convert.ToInt32(dt.Rows[0][0]);
                emp.Name = dt.Rows[0][1].ToString();
                emp.Gender = dt.Rows[0][2].ToString();
                emp.Age = Convert.ToInt32(dt.Rows[0][3]);
                emp.Position = dt.Rows[0][4].ToString();
                emp.Office = dt.Rows[0][5].ToString();
                emp.HireDate = Convert.ToDateTime(dt.Rows[0][6]);
                emp.Salary = Convert.ToInt32(dt.Rows[0][7]);

                return View(emp);
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            sqlconn.Open();
            string sql = @"delete from Employees WHERE EmployeeId =@EmployeeId ";
            SqlCommand sqlcomm = new SqlCommand(sql, sqlconn);

            sqlcomm.Parameters.AddWithValue("@EmployeeId", id);

            sqlcomm.ExecuteNonQuery();
            return RedirectToAction("Index");
        }
    }
}