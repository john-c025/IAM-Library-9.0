using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IAM_Library.appWallet.api;

namespace IAM_Library.appWallet.auth
{
    public class MainAuthWalletClient
    {
        private readonly string _apiBaseUrl;
        private readonly string _apiEndpointParameterSeparator;
        private readonly string _accessToken;
        private readonly string _accountKey;
        private HttpClient _httpClient;
        private readonly ApiConfiguration _apiConfig;
        private CustomScript custom = new CustomScript();

        private Dictionary<string, string> endpoints = new Dictionary<string, string> //define custom endpoints for ReportAPI Client
        {
            { "GetEncashmentBase", Encryption.decodeString(_constants.getEncashmentDetailsBase) },
            { "GetWalletBalanceBase", Encryption.decodeString(_constants.getWalletBalanceBase) },
        };
        public MainAuthWalletClient(AuthApiResponseData accessCredentials, HttpClient httpClient) // constructor
        {
            _accessToken = accessCredentials.signature;
            _accountKey = accessCredentials.accountKey;
            _apiBaseUrl = _wallet_endpoints.baseUrlWallet;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
            _apiEndpointParameterSeparator = Encryption.decodeString(_constants.endpointParameterAndSeparator);
            _apiConfig = new ApiConfiguration(_apiBaseUrl, endpoints);
        }

        public async Task<AuthApiResponseData> AuthenticateAsync(string username, string oCodedUsername, string oCodedPassword)
        {
            //Console.WriteLine($"Response from API Client before Auth creation: Running . . . ");
            try
            {
                var authRequest = new { usrname = username, cCodedusrname = oCodedUsername, cCodedpword = oCodedPassword };
                var authRequestJson = JsonConvert.SerializeObject(authRequest);
                var content = new StringContent(authRequestJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiBaseUrl, content);

                //Console.WriteLine($"Response from API Client after Auth : Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<AuthApiResponseData>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - Signature is: {apiResponse.signature}");
                    return apiResponse;
                }
                else
                {
                    //Console.WriteLine(new ApiResponseData { accountKey = "Not Found", signature = "invalid user" });
                    return new AuthApiResponseData { accountKey = "Not Found", signature = "invalid_user" };
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error during authentication: {ex.Message}");
                return new AuthApiResponseData { accountKey = "Not Found", signature = $"invalid_user" };
            }
        }
    }
}
