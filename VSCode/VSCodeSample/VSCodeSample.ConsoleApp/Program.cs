using System;
using VSCodeSample.BLL;
using VSCodeSample.Models;

namespace VSCodeSample.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please input user ID");
            string userId = Console.ReadLine();
            WinAdUserService userService = new WinAdUserService();
            WinAdUserInfo user = userService.Get(userId);
            Console.WriteLine(string.Format("{0},{1},{2}",user.UserID,user.DisplayName,user.Email));
            Console.ReadKey(true);
        }
    }
}
