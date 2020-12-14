using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{
     class TransferService
    {
        private readonly string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public TransferService()
        {
            this.API_BASE_URL = API_BASE_URL + "Transfers";

            this.client = new RestClient();
        }
      
        public void Login(IAuthenticator token)
        {
            this.client.Authenticator = token;
        }

        //get list of all transfers sent to or from the logged in user
        public List<API_Transfer> GetAllTransfers(int userId)
        {
            RestRequest request = new RestRequest(API_BASE_URL + $"/{userId}");

            var response = client.Get<List<API_Transfer>>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred fetching transfers");

                return new List<API_Transfer>();
            }
        }

        //pull up transfer by specific id number
        public API_Transfer GetSpecificTransfer(int transferId)
        {
            RestRequest request = new RestRequest(API_BASE_URL + $"/detail/{transferId}");

            var response = client.Get<API_Transfer>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred fetching transfers");

                return new API_Transfer();
            }
        }


    }
}
