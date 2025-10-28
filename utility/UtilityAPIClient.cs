using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System;
using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using IAM_Library.models.dashboard;
using System.Net.Http.Headers;
using IAM_Library.models.general;
using IAM_Library.models.wallet;
using Newtonsoft.Json;
using IAM_Library.models.reports;

namespace IAM_Library.utility
{
    public class UtilityAPIClient
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

            {"CheckAccount",Encryption.decodeString(_constants.GETCheckAccountID) },
            { "IdNumberParam", Encryption.decodeString(_constants.idNumberParamQR) },
            { "GetAccountKey", Encryption.decodeString(_constants.GETAccountKey) },
            { "valueParam", Encryption.decodeString(_constants.valueParameter) },




        };

        public UtilityAPIClient(string apiBaseUrl, AuthApiResponseData accessCredentials, AccountDetailData loggedAccData, HttpClient httpClient) // constructor
        {
            _accessToken = accessCredentials.signature;
            _accountKey = accessCredentials.accountKey;
            _apiBaseUrl = apiBaseUrl;
            _httpClient = httpClient;
            //auth header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
            _apiEndpointParameterSeparator = Encryption.decodeString(_constants.endpointParameterAndSeparator);
            //load reusable API configuration
            _apiConfig = new ApiConfiguration(_apiBaseUrl, endpoints);

        }

        public async Task<ApiResponseModel<AccountDetailData>> CheckAccountData(string idNumber)
        {
            Console.WriteLine($"[DEBUG] from API Registration Account Check client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "CheckAccount", _apiConfig, (_apiConfig.Endpoints["IdNumberParam"], idNumber));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for QR : {api_verification_activation}");
                var GET_wallet_balance_response = await _httpClient.GetAsync(api_verification_activation);
                if (GET_wallet_balance_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_wallet_balance_response.Content.ReadAsStringAsync();
                    var verification_response_asJson = JsonConvert.DeserializeObject<AccountDetailData>(responseData);
                    Console.WriteLine("[DEBUG] QR Response Content : " + responseData);

                    return new ApiResponseModel<AccountDetailData>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Account Fetched! User is Active!",
                        Data = verification_response_asJson
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_wallet_balance_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<AccountDetailData>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot fetch account details : {e.Message}",
                    Data = null
                };
            }
        }



        public async Task<ApiResponseModel<string>> GetAccountKey(string idNumber)
        {

            //idNumber = Encryption.MD5Hash(idNumber);
            //hash id number heree
            Console.WriteLine($"[DEBUG] from API Registration Account Check client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "GetAccountKey", _apiConfig, (_apiConfig.Endpoints["valueParam"], idNumber));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for QR : {api_verification_activation}");
                var GET_wallet_balance_response = await _httpClient.GetAsync(api_verification_activation);
                if (GET_wallet_balance_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_wallet_balance_response.Content.ReadAsStringAsync();
                    var verification_response_asJson = JsonConvert.DeserializeObject<AccountDetailData>(responseData);
                    Console.WriteLine("[DEBUG] QR Response Content : " + responseData);

                    return new ApiResponseModel<string>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Account Fetched! User is Active!",
                        Data = responseData
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_wallet_balance_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<string>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot fetch account details : {e.Message}",
                    Data = null
                };
            }
        }


    }
    // test for git link

   
    
    /*
    
    
    public class AccountInfo
    {
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"ID NUMBER: {IdNumber}\nNAME: {Name}\nSTATUS: {Status}";
        }
    }

    public class HtmlParser
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<AccountInfo> GetAccountInfoFromUrlAsync(string url)
        {
            try
            {
                var response = await client.GetStringAsync(url);
                return ParseAccountInfo(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching URL: {ex.Message}");
                return null;
            }
        }

        private AccountInfo ParseAccountInfo(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var idNumber = doc.DocumentNode.SelectSingleNode("//span[@id='ContentPlaceHolder1_lblPrimaryID']/b").InnerText.Trim();
            var name = doc.DocumentNode.SelectSingleNode("//span[@id='ContentPlaceHolder1_lblrecip']/b").InnerText.Trim();
            var status = doc.DocumentNode.SelectSingleNode("//span[@id='ContentPlaceHolder1_lblStatus']/b/font").InnerText.Trim();

            return new AccountInfo
            {
                IdNumber = idNumber,
                Name = name,
                Status = status
            };
        }
    */
    }

    //









