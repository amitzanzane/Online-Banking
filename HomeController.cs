using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BankSystem.Controllers
{
    public class HomeController : Controller
    {       
        public ActionResult Index()
        {
            Session["loginSession"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fc)
        {
            login l = new login();
            l.username = fc["username"];
            l.password = fc["password"];
            
            if (Session["loginSession"] == null)
            {
                Session["loginSession"] = l.username;
            }

            SqlConnection con = null;
            SqlDataReader dr = null;
            try
            {
                string connectionString = GetConnectionString();

                con = new SqlConnection(connectionString);
                
                con.Open();
                string str = $"select * from Customer where Username = " + $" '{l.username}' and Password = '{l.password}'";
                SqlCommand cmd = new SqlCommand(str, con);

                dr = cmd.ExecuteReader();

                if (ModelState.IsValid)
                {
                    if (dr.HasRows)
                    {
                        TempData["message"] = "Login Successful";
                        TempData.Keep("message");
                        return RedirectToAction("BankPage", l);
                    }
                    else
                    {
                        ViewBag.attempt = "Login Failed. Please try again or if you are new then please Register";
                    }
                }

            }
            finally
            {
                // close reader
                if (dr != null)
                {
                    dr.Close();
                }
                // close connection
                if (con != null)
                {
                    con.Close();
                }
            }

            return View();
        }
        protected string GetConnectionString()
        {
            var datasource = @"DESKTOP-TSSC28A\SQLEXPRESS";//your server
            var database = "BankSystem"; //your database name
            var username = "amit"; //username of server to connect
            var password = "amit12345"; //password
                                        //your connection string 
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            return connString;
        }
        public ActionResult Register()
        {
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            SqlDataAdapter _da = new SqlDataAdapter("Select * From Branch", con);
            DataTable _dt = new DataTable();
            _da.Fill(_dt);
            ViewBag.BranchList = ToSelectList(_dt, "Id", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult Register(login l)
        {
            return RedirectToAction("BankPage", l);
        }

        private dynamic ToSelectList(DataTable dt, string v1, string v2)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    list.Add(new SelectListItem()
                    {
                        Text = row[v2].ToString(),
                        Value = row[v1].ToString()
                    });
                }
                return new SelectList(list, "Value", "Text");
            }
            catch 
            {
                throw new NotImplementedException();
            }
            
        }

        public ActionResult BankPage(login l)
        {
            try
            {
                if (Session["loginSession"] == null)
                {
                    Session["loginSession"] = l.username;
                }
                
                string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(constring);
                con.Open();


                if (l.Name != null)
                {
                    SqlCommand Cmd = new SqlCommand("INSERT_INTO_BANK_TABLES", con);
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@name", l.Name);
                    Cmd.Parameters.AddWithValue("@dob", l.DOB);
                    Cmd.Parameters.AddWithValue("@phone", l.Phone);
                    Cmd.Parameters.AddWithValue("@email", l.Email);
                    Cmd.Parameters.AddWithValue("@address", l.Address);
                    Cmd.Parameters.AddWithValue("@username", l.username);
                    Cmd.Parameters.AddWithValue("@password", l.password);
                    Cmd.Parameters.AddWithValue("@accType", l.AccountType);
                    Cmd.Parameters.AddWithValue("@branchId", l.Branch);

                    Cmd.ExecuteNonQuery();

                    TempData["message"] = "Registration Successful";
                    TempData.Keep("message");
                }

                //Contact
                SqlDataAdapter _da = new SqlDataAdapter("Select B.Name, B.Description, B.IFSC, B.Phone From Branch B Inner Join Account A on B.Id = A.BranchId Inner Join Customer C on A.CustId = C.Id  Where C.Username = " + $" '{Session["loginSession"]}'", con);
                DataTable dt = new DataTable();
                _da.Fill(dt);

                List<Branches> br = new List<Branches>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Branches brs = new Branches();
                    brs.Name = dt.Rows[i]["Name"].ToString();
                    brs.Description = dt.Rows[i]["Description"].ToString();
                    brs.IFSC = dt.Rows[i]["IFSC"].ToString();
                    brs.Contact = dt.Rows[i]["Phone"].ToString();
                    br.Add(brs);
                }
                l.Branchs = br;

                con.Close();

                return View(l);
            }
            catch (Exception)
            {

                throw;
            }
            
        }


        public ActionResult Statement()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Statement(Statement st)
        {
            if (st.StartDate >= Convert.ToDateTime("1/1/0002 00:00:00 AM") && st.EndDate <= DateTime.Now)
            {
                st.StatementList = tranrecord(st.StartDate, st.EndDate);
            }
            else
            {
                ViewData["wrongdates"] = "Please Enter valid Dates !";
                return View(); 
            }

            return View(st);
        }

        public List<Statemomentum> tranrecord(DateTime startDate, DateTime endDate)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();

            SqlDataAdapter _da = new SqlDataAdapter("Select T.TranDate, T.TranType, T.Amount From Transactions T Inner Join Account A On T.AccId = A.Id Inner Join Customer C On A.CustId = C.Id Where C.Username = " + $" '{Session["loginSession"]}' and T.TranDate Between '{startDate}' And '{endDate}'", con);
            DataTable dt = new DataTable();
            _da.Fill(dt);

            List<Statemomentum> st = new List<Statemomentum>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Statemomentum s = new Statemomentum();
                s.TranDate = Convert.ToDateTime(dt.Rows[i]["TranDate"]);
                s.TranType = dt.Rows[i]["TranType"].ToString();
                s.Amount = Convert.ToInt32(dt.Rows[i]["Amount"]);
                st.Add(s);
            }
            con.Close();

            return st;
        }

        public ActionResult Balance()
        {
            login l = new login();
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);            
            SqlDataAdapter _da = new SqlDataAdapter("Select A.Balance From Account A Inner Join Customer C On A.CustId = C.Id Where C.Username = " + $" '{Session["loginSession"].ToString()}'", con);
            DataTable dt = new DataTable();
            _da.Fill(dt);

            string bal = dt.Rows[0]["Balance"].ToString();

            TempData["balance"] = bal;
            TempData.Keep("balance");

            return RedirectToAction("BankPage");
        }

        public ActionResult Transfer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Transfer(string AccountNumber, string IFSC, string AccountHolder, float Amount)
        {
            string constring = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            con.Open();

            //Check Valid Account Details
            SqlDataAdapter _da = new SqlDataAdapter("Select * From Customer C Inner Join Account A on C.Id = A.CustId Inner Join Branch B on A.BranchId = B.Id  Where A.AccNumber = " + $" '{AccountNumber}' and B.IFSC = '{IFSC}' and C.Name = '{AccountHolder}'", con);
            DataSet ds = new DataSet();
            _da.Fill(ds);

            if (ds != null && ds.Tables[0].Rows.Count != 0)
            {
                //Balance check
                SqlCommand oCmd = new SqlCommand("Select Balance from Account A Inner Join Customer C on C.Id = A.CustId Where C.Username = @username", con);
                oCmd.Parameters.AddWithValue("@username", Session["loginSession"]);
                SqlDataReader dr = oCmd.ExecuteReader();
                if (dr.Read())
                {
                    int bal = Convert.ToInt32(dr["Balance"]);
                    
                    if (bal >= Amount)
                    {
                        dr.Close();

                        SqlCommand Cmd = new SqlCommand("UPDATE_TRANSACTION", con);
                        Cmd.CommandType = CommandType.StoredProcedure;
                        Cmd.Parameters.AddWithValue("@Username", Session["loginSession"]);
                        Cmd.Parameters.AddWithValue("@AccNumber", AccountNumber);
                        Cmd.Parameters.AddWithValue("@IFSC", IFSC);
                        Cmd.Parameters.AddWithValue("@AccHolder", AccountHolder);
                        Cmd.Parameters.AddWithValue("@Amount", Amount);
                        Cmd.Parameters.Add("@text", SqlDbType.Char, 500);
                        Cmd.Parameters["@text"].Direction = ParameterDirection.Output;

                        Cmd.ExecuteNonQuery();
                        //ViewBag.text = (string)Cmd.Parameters["@text"].Value; //first check whether it is null or not then use this line

                        if (ViewBag.text == null)
                        {
                            SqlCommand cmds = new SqlCommand("ADD_TRANSACTION_RECORD", con);
                            cmds.CommandType = CommandType.StoredProcedure;
                            cmds.Parameters.AddWithValue("@Username", Session["loginSession"]);
                            cmds.Parameters.AddWithValue("@AccNumber", AccountNumber);
                            cmds.Parameters.AddWithValue("@Amount", Amount);

                            cmds.ExecuteNonQuery();

                            ViewBag.text = "Transaction Successful.";
                        }
                    }
                    else
                    {
                        ViewBag.text = "You don't have enough Balance !";
                    }
                }                               
                
            }
            else
            {
                ViewBag.text = "Please Check User Credentials !!!";
            }
            
            con.Close();

            return View();
        }
    }
}