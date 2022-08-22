using System;
using System.Data.SqlClient;
using VSCodeSample.Models;
using System.Collections.Generic;

namespace VSCodeSample.DAL
{
    public class WinAdUserDAL
    {
        private const string DBCONNECTIONSTRING = "Data Source=AWASE1WUXMSQL81;Initial Catalog=HRDB;User Id=shicheng_conn;Password=Jabil@123456";
        public WinAdUserInfo Get(string userId)
        {
            WinAdUserInfo user = null;
            string sql = "SELECT * FROM WinAdUser WHERE userid=@userid";
            SqlConnection connection = new SqlConnection(DBCONNECTIONSTRING);
            SqlCommand command = new SqlCommand(sql,connection);
            connection.Open();
            SqlParameter para  = new SqlParameter("@userid",userId);
            command.Parameters.Add(para);
            using(SqlDataReader dr  = command.ExecuteReader())
            {
                if(dr.Read())
                {
                    user  = new WinAdUserInfo
                    {
                        UserID = dr["userid"].ToString(),
                        DisplayName = dr["displayname"].ToString(),
                        Email = dr["mail"].ToString(),
                        Status = 1
                    };
                }
            }
            connection.Close();

            return user;
        }

        public List<WinAdUserInfo> List(string name)
        {
            List<WinAdUserInfo> users = new List<WinAdUserInfo>
            {
                new WinAdUserInfo
                {
                    UserID = "shens61",
                    DisplayName = "Shicheng shen",
                    Email = "Shicheng_Shen@jabil.com",
                    Status = 1
                },
                 new WinAdUserInfo
                {
                    UserID = "zhouz13",
                    DisplayName = "zhixiang zhou",
                    Email = "zhixiang_zhou@jabil.com",
                    Status = 1
                }
            };

            return users;
        }
    }
}
