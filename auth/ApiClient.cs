using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using IAM_Library.models.auth;
using IAM_Library.appWallet.models.wallet;
using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.appWallet.models;
using System.Net;
using IAM_Library.appWallet.api;
using IAM_Library.appWallet.models.dashboard;

namespace IAM_Library.auth
{
    public class ApiClient//Auth Api // inherit from MainApiClient Class
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        //private readonly string _token; //implement token if needed nalang, for now wag muna

        public ApiClient(string apiBaseUrl, HttpClient httpClient) // constructor
        {
            _httpClient = httpClient;
            _apiBaseUrl = apiBaseUrl;
            //_token = token; //implement account token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader));

            //TEST Client Check
            //Console.WriteLine($"Response from API Client Instance 1 with header of {Encryption.decodeString(_Auth.authHeader)}");     
        }

		public async Task<AuthWalletApiResponseData> AuthenticateAsyncWallet(string username, string oCodedUsername, string oCodedPassword, DeviceDetails deviceDetails)
		{
			_httpClient.DefaultRequestHeaders.Clear();
			_httpClient.DefaultRequestHeaders.Add("ApiKey", Encryption.decodeString(_wallet_endpoints.api_key));
			try
			{
				var authRequest = new
				{
					usrname = username,
					cCodedusrname = oCodedUsername,
					cCodedpword = oCodedPassword,
					deviceId = deviceDetails.deviceID,
					deviceName = deviceDetails.deviceName,
					ipAddress = deviceDetails.ipAddress
				};

				var authRequestJson = JsonConvert.SerializeObject(authRequest);
				var content = new StringContent(authRequestJson, Encoding.UTF8, "application/json");

				var response = await _httpClient.PostAsync(_apiBaseUrl, content);
				Console.WriteLine($"Auth POST : {authRequestJson}");
				Console.WriteLine($"Response from API Client after Auth : Code is {response.StatusCode} . . .  & API Signature is {response}");

				if (response.IsSuccessStatusCode)
				{
					var responseData = await response.Content.ReadAsStringAsync();
					try
					{
						var apiResponse = JsonConvert.DeserializeObject<AuthWalletApiResponseData>(responseData);
						Console.WriteLine($"ApiClient WALLET - Signature is: {apiResponse.signature}");
						Console.WriteLine($"ApiClient WALLET - Session ID is: {apiResponse.sesssionID}");
						return apiResponse;
					}
					catch (JsonException)
					{
						Console.WriteLine("Response was not in JSON format.");
						return new AuthWalletApiResponseData
						{
							accountKey = "Unknown",
							signature = "invalid_response_format",
							sesssionID = "unknown"
						};
					}
				}
				else
				{
					var errorResponse = await response.Content.ReadAsStringAsync();
					Console.WriteLine($"Error response from server: {errorResponse}");
					return new AuthWalletApiResponseData
					{
						accountKey = "Not Found",
						signature = errorResponse, // Return the plain error message as the signature
						sesssionID = "error"
					};
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error during authentication: {ex.Message}");
				return new AuthWalletApiResponseData
				{
					accountKey = "Not Found",
					signature = $"Exception: {ex.Message}",
					sesssionID = "error"
				};
			}
		}



		/*
        public async Task<AuthWalletApiResponseData> AuthenticateAsyncWallet(string username, string oCodedUsername, string oCodedPassword, DeviceDetails deviceDetails)
        {
            //Console.WriteLine($"Response from API Client before Auth creation: Running . . . ");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", Encryption.decodeString(_wallet_endpoints.api_key));
            try
            {
                var authRequest = new { usrname = username, cCodedusrname = oCodedUsername, cCodedpword = oCodedPassword, deviceId = deviceDetails.deviceID, deviceName = deviceDetails.deviceName, ipAddress = deviceDetails.ipAddress };

                var authRequestJson = JsonConvert.SerializeObject(authRequest);
                var content = new StringContent(authRequestJson, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_apiBaseUrl, content);
                Console.WriteLine($"Auth POST : {authRequestJson}");

                Console.WriteLine($"Response from API Client after Auth : Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    var apiResponse = JsonConvert.DeserializeObject<AuthWalletApiResponseData>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());

                    Console.WriteLine($"ApiClient WALLET - Signature is: {apiResponse.signature} ");
                    Console.WriteLine($"ApiClient WALLET - Session ID is: {apiResponse.sesssionID}");
                    return apiResponse;

                }
                else
                {
                    //Console.WriteLine(new ApiResponseData { accountKey = "Not Found", signature = "invalid user" });
                    return new AuthWalletApiResponseData { accountKey = "Not Found", signature = "invalid_user", sesssionID="invalid_user" };


                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error during authentication: {ex.Message}");
                return new AuthWalletApiResponseData { accountKey = "Not Found", signature = $"invalid_user", sesssionID="invalid_user" };
            }
        }
        */
		// Authenticate
		public async Task<AuthApiResponseData> AuthenticateAsync(string username, string oCodedUsername, string oCodedPassword)
        {
            //Console.WriteLine($"Response from API Client before Auth creation: Running . . . ");
            try
            {
                var authRequest = new { usrname = username, cCodedusrname = oCodedUsername, cCodedpword = oCodedPassword};

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
