using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class BalanceTransfer
    {
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }
        public int TransferType { get; set; }
        public int TransferStatus { get; set; }

        public int TransferId { get; set; }


        public BalanceTransfer(int AccountFrom, int AccountTo, decimal Amount)
        {
            this.AccountFrom = AccountFrom;
            this.AccountTo = AccountTo;
            this.Amount = Amount;
        }


        public BalanceTransfer()
        {

        }
        public BalanceTransfer(int AccountFrom, int AccountTo, decimal Amount, int TransferType, int TransferStatus, int TransferId)
        {
            this.AccountFrom = AccountFrom;
            this.AccountTo = AccountTo;
            this.Amount = Amount;
            this.TransferType = TransferType;
            this.TransferStatus = TransferStatus;
            this.TransferId = TransferId;
        }
    }
}
