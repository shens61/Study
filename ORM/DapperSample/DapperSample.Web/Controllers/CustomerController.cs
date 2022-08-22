using Dapper;
using Dapper.Contrib.Extensions;
using DapperSample.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DapperSample.Web.Controllers
{
    public class CustomerController : Controller
    {
        private const string DBCONNECTIONSTRING = "Data Source=AWASE1WUXMSQL81;Initial Catalog=mydb;User Id=mydb_conn;Password=Jabil@123456";
        // GET: CustomerController
        public ActionResult Index()
        {
            List<CustomerInfo> customers = new List<CustomerInfo>();
            using (IDbConnection db = new SqlConnection(DBCONNECTIONSTRING))
            {
                //customers = db.Query<CustomerInfo>("SELECT * FROM Customers").ToList();
                customers = db.GetAll<CustomerInfo>().ToList();
            }

            return View(customers);
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int id)
        {
            CustomerInfo customer = new CustomerInfo();
            using (IDbConnection db = new SqlConnection(DBCONNECTIONSTRING))
            {
                customer = db.Query<CustomerInfo>($"SELECT * FROM Customers WHERE CustomerID=@id", new { id }).SingleOrDefault();
            }

            return View(customer);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerInfo customer)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(DBCONNECTIONSTRING))
                {
                    string sql = "Insert Into Customers (FirstName, LastName, Email) Values(@FirstName, @LastName, @Email)";

                    db.Execute(sql, customer);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            CustomerInfo customer = new CustomerInfo();
            using (IDbConnection db = new SqlConnection(DBCONNECTIONSTRING))
            {
                customer = db.Query<CustomerInfo>($"Select * From Customers WHERE CustomerID = {id}").SingleOrDefault();
            }

            return View(customer);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustomerInfo customer)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(DBCONNECTIONSTRING))
                {
                    string sql = "UPDATE Customers set FirstName=@FirstName,LastName=@LastName,Email=@Email " +
                        " WHERE CustomerID=@CustomerID";

                    db.Execute(sql, customer);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            CustomerInfo customer = new CustomerInfo();
            using (IDbConnection db = new SqlConnection(DBCONNECTIONSTRING))
            {
                customer = db.Query<CustomerInfo>("Select * From Customers WHERE CustomerID =" + id, new { id }).SingleOrDefault();
            }

            return View(customer);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(DBCONNECTIONSTRING))
                {
                    string sql = "Delete From Customers WHERE CustomerID = " + id;

                    int rowsAffected = db.Execute(sql);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
