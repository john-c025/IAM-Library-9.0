using IAM_Library.models.auth;
using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.dashboard;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IAM_Library.dashboard
{
    public class AccountsApiClient //inherit from MainApiClient Class
    {

        private readonly string _apiBaseUrl;
        private readonly string _apiEndpoint;
        private readonly string _accessToken;
        public HttpClient _httpClient;

        public AccountsApiClient(string apiBaseUrl, string accessToken, HttpClient httpClient) // constructor
        {
            //_httpClient = httpClient;
            _accessToken = accessToken;
            _apiBaseUrl = apiBaseUrl;
            _apiEndpoint = Encryption.decodeString(_constants.authSuccessDashEndpoint);
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

        }
        //
        public async Task<AccountDetailData> GetAccountDataAsync(string accountKey)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
            var responseData = "";

            try
            {

                var apiUrl = _apiBaseUrl + _apiEndpoint + accountKey;
                // here's the problem
                var response = await _httpClient.GetAsync(apiUrl);

                Console.WriteLine($" API Endpoint is {apiUrl}");
                Console.WriteLine($"Response from API Client after Account Detail Retrieval : Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();

                    var apiResponseData = JsonConvert.DeserializeObject<AccountDetailData>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - Signature is: {apiResponseData.primaryInfo.primaryID}");
                    return apiResponseData;

                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    var apiResponseData = JsonConvert.DeserializeObject<AccountDetailData>(responseData);
                    return apiResponseData;

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during retrieval of data:  {ex.Message}");
                return _defaults.invalidAccountDetailDataResponse;
                // return new ApiResponseData { accountKey = "Not Found", signature = $"invalid_user {ex.Message}" };
            }
        }
    }
}
