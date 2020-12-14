using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        Transfer AddTransfer(Transfer transfer);

        List<Transfer> GetAllUsersTransfers(int userId);

        Transfer GetSpecificTransfer(int transferId);

        Transfer AcceptOrDeclineTransfer(Transfer transfer);
    }
}
