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
    public class AccountsController : ControllerBase
    {
        private readonly IUserDAO UserSqlDAO;
        private readonly IAccountDAO AccountSqlDAO;

        public AccountsController(IAccountDAO AccountSqlDAO)
        {
            this.AccountSqlDAO = AccountSqlDAO;
        }

        [HttpGet("{userId}")]
        public ActionResult<Account> GetAccount(int userId)
        {
            Account account = this.AccountSqlDAO.GetAccount(userId);


            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);

        }







    }
}
