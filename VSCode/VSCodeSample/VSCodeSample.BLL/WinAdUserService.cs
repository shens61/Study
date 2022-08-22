using System;
using VSCodeSample.Models;
using VSCodeSample.DAL;
using System.Collections.Generic;

namespace VSCodeSample.BLL
{    public class WinAdUserService
    {
        private readonly WinAdUserDAL _dal  = new WinAdUserDAL();
        public WinAdUserInfo Get(string userId)
        {            
            return _dal.Get(userId);
        }

        public List<WinAdUserInfo> List(string name)
        {            
            return _dal.List(name);
        }
    }
}
