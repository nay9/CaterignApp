using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class AccountService
    {

        private readonly string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public AccountService()
        {
            this.API_BASE_URL = API_BASE_URL + "Accounts";

            this.client = new RestClient();
        }

        public void Login(IAuthenticator token)
        {
            this.client.Authenticator = token;
        }

        //pulls account info such as balance, after user id has been established from login
        public API_Account GetAccount(int id)
        {
            RestRequest request = new RestRequest(API_BASE_URL + $"/{id}");

            var response = client.Get<API_Account>(request);

            if(response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred fetching the Account");

                return null;
            }
        }

        public API_Account UpdateAccountBalance(API_Account account)
        {
            RestRequest request = new RestRequest(API_BASE_URL + $"/{account.AccountId}");
            request.AddJsonBody(account);
            var response = client.Put<API_Account>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred updating the Account");

                return null;
            }
        }

    }
}
