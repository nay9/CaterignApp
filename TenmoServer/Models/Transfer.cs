using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public int TransferType { get; set; }
        public int TransferStatus { get; set; }
        public decimal Amount { get; set; }




        public Transfer(int TransferId,  int AccountFrom, int AccountTo, int TransferType, int TransferStatus, decimal Amount)
        {
            this.TransferId = TransferId;

            this.AccountFrom = AccountFrom;
            this.AccountTo = AccountTo;
            this.TransferType = TransferType;
            this.TransferStatus = TransferStatus;
            this.Amount = Amount;

        }

        public Transfer()
        {

        }
    }
}
