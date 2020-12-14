using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;
        const decimal startingBalance = 1000;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Account GetAccount(int userId)
        {
            Account returnAccount = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM accounts WHERE user_id = @user_id", conn);
                cmd.Parameters.AddWithValue("@user_id", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows && reader.Read())
                {
                    returnAccount = GetAccountFromReader(reader);
                }
            }

            return returnAccount;
        }

        public Account UpdateAccount(int id, decimal amount)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("Update accounts SET balance = @balance WHERE user_id = @user_id", conn);
                cmd.Parameters.AddWithValue("@user_id", id);
                cmd.Parameters.AddWithValue("@balance", amount);
                cmd.ExecuteNonQuery();
            }

            return GetAccount(id);
        }

        private Account GetAccountFromReader(SqlDataReader reader)
        {
            return new Account()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                AccountId = Convert.ToInt32(reader["account_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
               
            };
        }

    }
}
