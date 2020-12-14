using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
    public class BalanceTransferService
    {
        private readonly string API_BASE_URL = "https://localhost:44315/";

        private readonly IRestClient client = new RestClient();
        


        public BalanceTransferService()
        {
            this.API_BASE_URL = API_BASE_URL + "BalanceTransfer";

            this.client = new RestClient();


        }
        public void Login(IAuthenticator token)
        {
            this.client.Authenticator = token;
        }

        //updates the balance amount when transfers are activated
        public BalanceTransfer UpdateAccountBalance(BalanceTransfer newbalanceTransfer)
        {
            RestRequest request = new RestRequest(API_BASE_URL);
            //BalanceTransfer transferInfo = new BalanceTransfer(newbalanceTransfer);

            request.AddJsonBody(newbalanceTransfer);
            var response = client.Post<BalanceTransfer>(request);

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

        //creates a pending transfer to the selected user
        public BalanceTransfer RequestTransfer(BalanceTransfer newbalanceTransfer)
        {
            RestRequest request = new RestRequest(API_BASE_URL);
            //BalanceTransfer transferInfo = new BalanceTransfer(newbalanceTransfer);

            request.AddJsonBody(newbalanceTransfer);
            var response = client.Put<BalanceTransfer>(request);

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

        //updates transfer status based on the users decision
        public BalanceTransfer AcceptOrDeclineTransfer(BalanceTransfer newbalanceTransfer)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "/AcceptOrDeclineTransfer");

            request.AddJsonBody(newbalanceTransfer);
            var response = client.Put<BalanceTransfer>(request);

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
