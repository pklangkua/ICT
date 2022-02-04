using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


    public class DBClass
    {
        public SqlConnection SqlStrCon()
        {
            string config = @"Data Source=PRADIT-THINKPAD\SQLEXPRESS;Initial Catalog=BooksDB;Persist Security Info=True;User ID=sa;Password=123456";
            return new SqlConnection(config);
        }
    }

