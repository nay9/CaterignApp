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
    public class TransfersController : ControllerBase
    {
        private readonly ITransferDAO TransferSqlDAO;

        public TransfersController(ITransferDAO TransferSqlDAO)
        {
            this.TransferSqlDAO = TransferSqlDAO;
        }


        [HttpGet ("{userId}")]
        [AllowAnonymous]
        public ActionResult<List<Transfer>> GetAllTransfers(int userId)
        {
            return Ok(this.TransferSqlDAO.GetAllUsersTransfers(userId));
        }

        [HttpGet("detail/{transferId}")]
        [AllowAnonymous]
        public ActionResult<Transfer> GetSpecificTransfer(int transferId)
        {

            return Ok(this.TransferSqlDAO.GetSpecificTransfer(transferId));
        }

    }
}
