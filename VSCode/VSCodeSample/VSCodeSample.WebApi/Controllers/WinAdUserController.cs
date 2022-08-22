using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VSCodeSample.Models;
using VSCodeSample.BLL;

namespace VSCodeSample.WebApi.Controllers
{
    [ApiController]
    public class WinAdUserController : ControllerBase
    {
        private readonly ILogger<WinAdUserController> _logger;
        private readonly WinAdUserService _userService = new WinAdUserService();

        public WinAdUserController(ILogger<WinAdUserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("api/users/{name}")]
        public IEnumerable<WinAdUserInfo> List(string name)
        {
           return _userService.List(name);
        }

        [HttpGet]
        [Route("api/user/{userId}")]
        public Task<WinAdUserInfo> Get(string userId)
        {
           return _userService.Get(userId);
        }
    }
}
