using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TenmoServer.DAO;
using TenmoServer.Models;



namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BalanceTransferController : ControllerBase
    {
        private readonly IUserDAO UserSqlDAO;
        private readonly IAccountDAO AccountSqlDAO;
        private readonly ITransferDAO TransferSqlDAO;

        public BalanceTransferController(IAccountDAO AccountSqlDAO, ITransferDAO TransferSqlDAO)
        {
            this.AccountSqlDAO = AccountSqlDAO;
            this.TransferSqlDAO = TransferSqlDAO;
        }

        //updates new balances for transfer accounts after transfer is made
        [HttpPost]
        public void UpdateAccountFromBalance(BalanceTransfer newbalanceTransfer)
        {
            int fromId = newbalanceTransfer.AccountFrom;
            int ToId = newbalanceTransfer.AccountTo;
            decimal amount = newbalanceTransfer.Amount;


            Account fromAccount = this.AccountSqlDAO.GetAccount(fromId);
            decimal newBalanceFrom = fromAccount.Balance - amount;
            AccountSqlDAO.UpdateAccount(fromId, newBalanceFrom);

            Account ToAccount = this.AccountSqlDAO.GetAccount(ToId);
            decimal newBalanceTo = ToAccount.Balance + amount;
            AccountSqlDAO.UpdateAccount(ToId, newBalanceTo);

            Transfer newTransfer = new Transfer(0, fromId, ToId, 2, 2, amount);
            TransferSqlDAO.AddTransfer(newTransfer);

        }

        //send pending transfer to another user
        [HttpPut]
        public void RequestTransfer(BalanceTransfer newbalanceTransfer)
        {
            int fromId = newbalanceTransfer.AccountFrom;
            int ToId = newbalanceTransfer.AccountTo;
            decimal amount = newbalanceTransfer.Amount;

            Transfer newTransfer = new Transfer(0, fromId, ToId, 1, 1, amount);
            TransferSqlDAO.AddTransfer(newTransfer);

        }

        //change transfer status based on user choice
        [HttpPut("AcceptOrDeclineTransfer")]
        public void AcceptOrDeclineTransfer(BalanceTransfer newbalanceTransfer)
        {
            int fromId = newbalanceTransfer.AccountFrom;
            int ToId = newbalanceTransfer.AccountTo;
            decimal amount = newbalanceTransfer.Amount;
            
            if (newbalanceTransfer.TransferStatus == 2)
            {
                Transfer newTransfer = new Transfer(newbalanceTransfer.TransferId, fromId, ToId, 1, 2, amount);
                TransferSqlDAO.AcceptOrDeclineTransfer(newTransfer);

                Account fromAccount = this.AccountSqlDAO.GetAccount(fromId);
                decimal newBalanceFrom = fromAccount.Balance - amount;
                AccountSqlDAO.UpdateAccount(fromId, newBalanceFrom);

                Account ToAccount = this.AccountSqlDAO.GetAccount(ToId);
                decimal newBalanceTo = ToAccount.Balance + amount;
                AccountSqlDAO.UpdateAccount(ToId, newBalanceTo);
            }
            else {

            Transfer newTransfer = new Transfer(newbalanceTransfer.TransferId, fromId, ToId, 1, 3, amount);
            TransferSqlDAO.AcceptOrDeclineTransfer(newTransfer);
            }


        }



    }
}
