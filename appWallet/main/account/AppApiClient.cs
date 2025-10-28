using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.appWallet.api;
using IAM_Library.appWallet.models;
using IAM_Library.appWallet.models.dashboard;
using Newtonsoft.Json;

namespace IAM_Library.appWallet.main.account
{
    public class AppApiClient
    {

        private readonly string _apiBaseUrl;
        private readonly string _apiEndpoint;
        private readonly string _accessToken;
        public HttpClient _httpClient;

        public AppApiClient(string apiBaseUrl,HttpClient httpClient) // constructor
        {
            //_httpClient = httpClient;
            _apiBaseUrl = apiBaseUrl;
            _apiEndpoint = Encryption.decodeString(_wallet_endpoints.walletGetAccountDetailsFull);
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader));
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

        }

        public async Task<GetAppInfoModel> GetAppInfo(int appId)
        {
            var responseData = "";
            var startTime = DateTime.Now;

            Console.WriteLine($"[DEBUG] ===== GetAppInfo STARTED at {startTime:yyyy-MM-dd HH:mm:ss.fff} =====");

            _httpClient.DefaultRequestHeaders.Clear();


            Console.WriteLine($"[DEBUG] Auth Header Type: Bearer");
            Console.WriteLine($"[DEBUG] Access Token (first 20 chars): {(_accessToken?.Length > 20 ? _accessToken.Substring(0, 20) + "..." : _accessToken ?? "NULL")}");

            try
            {
                var endpoint = $"/v1/App/GetAppInfo?appid={appId}";
                var apiUrl = _apiBaseUrl + endpoint;

                Console.WriteLine($"[DEBUG] Endpoint: {endpoint}");
                Console.WriteLine($"[DEBUG] API Base URL: {_apiBaseUrl}");
                Console.WriteLine($"[DEBUG] Full API URL: {apiUrl}");

                Console.WriteLine($"[DEBUG] Sending GET request to: {apiUrl}");
                var requestStartTime = DateTime.Now;

                var response = await _httpClient.GetAsync(apiUrl);

                var requestEndTime = DateTime.Now;
                var requestDuration = requestEndTime - requestStartTime;

                Console.WriteLine($"[DEBUG] Request completed in {requestDuration.TotalMilliseconds}ms");
                Console.WriteLine($"[DEBUG] Response Status Code: {response.StatusCode}");
                Console.WriteLine($"[DEBUG] Response Reason Phrase: {response.ReasonPhrase}");
                Console.WriteLine($"[DEBUG] Response Headers Count: {response.Headers.Count()}");
                Console.WriteLine($"[DEBUG] Response Content Type: {response.Content.Headers.ContentType}");

                responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] Response Content Length: {responseData.Length} characters");
                Console.WriteLine($"[DEBUG] Response Content: {responseData}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var apiResponseData = JsonConvert.DeserializeObject<GetAppInfoModel>(responseData);
                        Console.WriteLine($"[DEBUG] Successfully deserialized app info response");
                        Console.WriteLine($"[DEBUG] Deserialized Response: {JsonConvert.SerializeObject(apiResponseData, Formatting.Indented)}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetAppInfo SUCCESS - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return apiResponseData;
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"[ERROR] JSON Deserialization failed: {jsonEx.Message}");
                        Console.WriteLine($"[ERROR] JSON Stack Trace: {jsonEx.StackTrace}");
                        Console.WriteLine($"[ERROR] Raw Response Data: {responseData}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetAppInfo JSON ERROR - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return _defaults_wal.invalidAppInfo;
                    }
                }
                else
                {
                    Console.WriteLine($"[WARNING] Non-success status code received: {response.StatusCode}");
                    Console.WriteLine($"[WARNING] Attempting to deserialize error response");

                    try
                    {
                        var apiResponseData = JsonConvert.DeserializeObject<GetAppInfoModel>(responseData);
                        Console.WriteLine($"[DEBUG] Successfully deserialized error response");
                        Console.WriteLine($"[DEBUG] Error Response Data: {JsonConvert.SerializeObject(apiResponseData, Formatting.Indented)}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetAppInfo ERROR RESPONSE - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return apiResponseData;
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"[ERROR] Failed to deserialize error response: {jsonEx.Message}");
                        Console.WriteLine($"[ERROR] Raw Error Response: {responseData}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetAppInfo DESERIALIZATION ERROR - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return _defaults_wal.invalidAppInfo;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ERROR] HttpRequestException in GetAppInfo");
                Console.WriteLine($"[ERROR] HttpRequestException Message: {ex.Message}");
                Console.WriteLine($"[ERROR] HttpRequestException Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== GetAppInfo HTTP EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return _defaults_wal.invalidAppInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] General Exception in GetAppInfo");
                Console.WriteLine($"[ERROR] Exception Type: {ex.GetType().Name}");
                Console.WriteLine($"[ERROR] Exception Message: {ex.Message}");
                Console.WriteLine($"[ERROR] Exception Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== GetAppInfo GENERAL EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return _defaults_wal.invalidAppInfo;
            }





        }


    }
}
