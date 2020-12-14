using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO
    {
        private readonly string connectionString;

        public TransferSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //List all the transfers of the specific user
        public List<Transfer> GetAllUsersTransfers(int userId)
        {
            List<Transfer> result = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM transfers WHERE account_from = @account_from", conn);
                cmd.Parameters.AddWithValue("@account_from", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(GetTransferFromReader(reader));
                    }
                }

            }

            return result;
        }

        //pull transfer by its respective id
        public Transfer GetSpecificTransfer(int transferId)
        {
            Transfer result = new Transfer();

            Console.WriteLine("we are in TransferSqlDAO");

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM transfers WHERE transfer_id = @transfer_id", conn);
                cmd.Parameters.AddWithValue("@transfer_id", transferId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    return GetTransferFromReader(reader);
                }

            }

            return result;
        }

        //insert new transfer into the database
        public Transfer AddTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO transfers (account_from, account_to, transfer_type_id, transfer_status_id, amount) VALUES (@account_from, @account_to, @transfer_type_id, @transfer_status_id, @amount)", conn);
                cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                cmd.Parameters.AddWithValue("@transfer_type_id", transfer.TransferType);
                cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatus);
                cmd.Parameters.AddWithValue("@amount", transfer.Amount);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT @@IDENTITY", conn);
                transfer.TransferId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return transfer;
        }

        public Transfer AcceptOrDeclineTransfer(Transfer transfer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE transfers SET transfer_status_id = @transfer_status_id WHERE transfer_id = @transfer_id", conn);
                cmd.Parameters.AddWithValue("@transfer_id", transfer.TransferId);
                cmd.Parameters.AddWithValue("@transfer_status_id", transfer.TransferStatus);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("SELECT @@IDENTITY", conn);
            }

            return transfer;
        }


        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            return new Transfer()
            {
                TransferId = Convert.ToInt32(reader["transfer_id"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                AccountTo = Convert.ToInt32(reader["account_to"]),
                TransferType = Convert.ToInt32(reader["transfer_type_id"]),
                TransferStatus = Convert.ToInt32(reader["transfer_status_id"]),
                Amount = Convert.ToDecimal(reader["amount"]),

            };
        }


    }
}
