using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient.APIClients
{

    //Profile in this class is our users outside (ie. the patrons using our service)
    public class ProfileService
    {

        private readonly string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();

        public ProfileService()
        {
            this.API_BASE_URL = API_BASE_URL + "Users";

            this.client = new RestClient();
        }

        public void Login(IAuthenticator token)
        {
            this.client.Authenticator = token;
        }

        //pulls up list of users to decide who to transfer to and from
        public List<API_User> GetAllUsers()
        {
            RestRequest request = new RestRequest(API_BASE_URL);

            var response = client.Get<List<API_User>>(request);

            if (response.IsSuccessful && response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                Console.WriteLine("An error occurred fetching users");

                return new List<API_User>();
            }
        }
    }
}
