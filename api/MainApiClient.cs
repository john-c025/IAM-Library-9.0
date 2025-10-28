using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IAM_Library.models.auth;
using Newtonsoft.Json;
using IAM_Library._custom;

namespace IAM_Library.api
{
    public class MainApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        public static string _apiUrl;

        //private readonly string _token; //implement token if needed nalang, for now wag muna

        public MainApiClient(string apiBaseUrl, string endpoint) // constructor
        {
            _httpClient = new HttpClient();
            _apiBaseUrl = apiBaseUrl;
            //_token = token; //implement account token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader));

            _apiUrl = _apiBaseUrl + endpoint;


        }



        


    }
}
