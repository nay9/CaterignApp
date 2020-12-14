using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserDAO UserSqlDAO;

        public UsersController(IUserDAO UserSqlDAO)
        {
            this.UserSqlDAO = UserSqlDAO;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            return Ok(this.UserSqlDAO.GetUsers());
        }


    }
}