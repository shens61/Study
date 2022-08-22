using CsvHelper;
using CsvHelper.Configuration;
using DapperSample.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Z.BulkOperations;
using Z.Dapper.Plus;

namespace DapperSample.Web.Controllers
{
    public class ImportDataController : Controller
    {
        private const string DBCONNECTIONSTRING = "Data Source=AWASE1WUXMSQL81;Initial Catalog=mydb;User Id=mydb_conn;Password=Jabil@123456";

        public IActionResult ImportCustomers()
        {
            CustomerData data = new CustomerData()
            {
                Text = "Sally, Whittaker,sally.whittaker@example.com" + Environment.NewLine +
                  "Belinda, Jameson, belinda.jameson@example.com" + Environment.NewLine +
                  "Jeff, Smith, jeff.smith@example.com" + Environment.NewLine +
                  "Sandy, Allen, sandy.allen@example.com" + Environment.NewLine
            };

            return View(data);
        }

        [HttpPost]
        public IActionResult ImportCustomers(CustomerData data)
        {
            DapperPlusManager.Entity<CustomerInfo>().Table("Customers");

            try
            {
                List<AuditEntry> auditEntries = new List<AuditEntry>();
                List<CustomerInfo> customers = new List<CustomerInfo>();
                using (IDbConnection db = new SqlConnection(DBCONNECTIONSTRING))
                {
                    CsvConfiguration csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture);
                    csvConfig.HasHeaderRecord = false;
                    using (var csv = new CsvReader(new StringReader(data.Text), csvConfig))
                    {
                        while (csv.Read())
                        {
                            string[] result = new string[3];
                            string value;

                            for (int i = 0; csv.TryGetField<string>(i, out value); i++)
                            {
                                result[i] = value;
                            }

                            CustomerInfo c = new CustomerInfo()
                            {
                                FirstName = result[0],
                                LastName = result[1],
                                Email = result[2]
                            };

                            customers.Add(c);
                        }
                    }

                    db.BulkInsert(customers);
                }

                return RedirectToAction("Index", "Customer");
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
