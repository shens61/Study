using Dapper;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace DapperSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string sql = "INSERT INTO Customers (CustomerName) Values (@CustomerName);";

            //using (var connection = new SqlConnection(FiddleHelper.GetConnectionStringSqlServerW3Schools()))
            //{
            //    var affectedRows = connection.Execute(sql, new { CustomerName = "Mark" });

            //    Console.WriteLine(affectedRows);

            //    var customer = connection.Query<Customer>("Select * FROM CUSTOMERS WHERE CustomerName = 'Mark'").ToList();

            //    FiddleHelper.WriteTable(customer);
            //}

            Console.ReadKey(true);
        }
    }

    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
