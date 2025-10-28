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
using IAM_Library.models.reports;
using IAM_Library.models.general;

namespace IAM_Library.reports
{
    public class ReportsApiClient //inherit from MainApiClient Class
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
            { "ReportsCoverageEndpoint", Encryption.decodeString(_constants.reportsCoverageEndpoint) },
            {"ReportsCHistoryEndpointBase",Encryption.decodeString(_constants.reportsCHistoryEndpointBase) },
            { "ReportsCSummaryEndpointBase", Encryption.decodeString(_constants.reportsCSummaryEndpointBase) },
            { "ReportsCReferralEndpointBase", Encryption.decodeString(_constants.reportsCReferralEndpointbase) },
            { "ReportsCOptionParam", Encryption.decodeString(_constants.reportsCSummaryOptionParam) },
            { "ReportsCAccKey", Encryption.decodeString(_constants.reportsCSummaryAccKey) },
            { "SystemAccessBase", Encryption.decodeString(_constants.systemAccessBase) },
            { "SystemAppIdParam", Encryption.decodeString(_constants.systemAppIdParam) },
            { "ReportsCEndpointDfromParam", Encryption.decodeString(_constants.reportsCSummaryEndpointDfromParam) },
            { "ReportsCEndpointDtoParam", Encryption.decodeString(_constants.reportsCSummaryEndpointDtoParam)},
            { "ReportsUnilevelEndpointBase", Encryption.decodeString(_constants.reportsUnilevelSalesCommissionBase)},
            { "ReportsPairHistoryEndpointBase", Encryption.decodeString(_constants.reportsPairsHistory)},
            { "ReportsLatestParam", Encryption.decodeString(_constants.latestParam)},
            { "ReportsCheckMatch", Encryption.decodeString(_constants.reportsCheckMatchBase)},
            { "ReportsRSM", Encryption.decodeString(_constants.reportsRSBase)},
            { "ReportsBinarySummaryList", Encryption.decodeString(_constants.reportsBinarySummary)},
            { "GetCommissionCtr", Encryption.decodeString(_constants.GETCommCtr)},
            { "UnilevelLeadershipBase", Encryption.decodeString(_constants.unilevelLeadership)},
            { "RqvHistoryBase", Encryption.decodeString(_constants.rqvSummary)},
            { "MoverHistoryBase", Encryption.decodeString(_constants.moverHistoryBase)},
            { "InfinityHistoryBase", Encryption.decodeString(_constants.infinityHistoryBase)},
            { "GetTotalUnicom", Encryption.decodeString(_constants.getTotalUnicom)},
            { "GetRank", Encryption.decodeString(_constants.getAccountRank)},
            {"IdNoParam",Encryption.decodeString(_constants.idNumberParam) },
            {"ReportsPurchaseHistoryListBase",Encryption.decodeString(_constants.purchasesListBase) },
            
            // Add more endpoints as needed
        };

        //Reports API Client
        public ReportsApiClient(string apiBaseUrl, AuthApiResponseData accessCredentials, HttpClient httpClient) // constructor
        {
            //define api endoints, access_token, api_base_url here
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

        //GET Coverage of Dates here
        public async Task<List<ReportsDataCommissionCoverage>> GetCoverageDatesList()
        {

            var responseData = "";
            Console.WriteLine($"from Report client to building url base url {_apiConfig.BaseUrl}");
            var api_coverage_endpoint_url = custom.BuildUrl(_apiConfig.BaseUrl, "ReportsCoverageEndpoint", _apiConfig);
            Console.WriteLine($"Built Url is {api_coverage_endpoint_url}");

            //UriBuilder uriBuilder = new UriBuilder(_apiConfig.BaseUrl);
            //uriBui    lder.Path = _apiConfig.Endpoints["ReportsCoverageEndpoint"];
            //var api_coverage_endpoint_url = uriBuilder.ToString();

            try
            {

                var response = await _httpClient.GetAsync(api_coverage_endpoint_url);

                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();

                    var apiResponseDataList = JsonConvert.DeserializeObject<List<ReportsDataCommissionCoverage>>(responseData);
                    Console.WriteLine("Test Coverage from Client: " + responseData);
                    Console.WriteLine("Test Coverage Type is  : " + responseData.GetType());

                    return apiResponseDataList;
                }
                else
                {
                    return new List<ReportsDataCommissionCoverage>(); // Return an empty list in case of failure or throw an custom exception for non successful request
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during retrieval of data:  {ex.Message}");
                return new List<ReportsDataCommissionCoverage>(); // Return an empty list in case of exception
            }
        }


        public async Task<List<Dictionary<string, object>>> GetCoverageDatesList_AsDict()
        {

            var responseData = "";
            Console.WriteLine($"from Report client to building url base url {_apiConfig.BaseUrl}");
            var api_coverage_endpoint_url = custom.BuildUrl(_apiConfig.BaseUrl, "ReportsCoverageEndpoint", _apiConfig);
            Console.WriteLine($"Built Url is {api_coverage_endpoint_url}");


            try
            {

                var response = await _httpClient.GetAsync(api_coverage_endpoint_url);

                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();

                    var apiResponseDataList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseData);
                    Console.WriteLine("Test Coverage from Client: " + responseData);
                    Console.WriteLine("Test Coverage Type is  : " + responseData.GetType());

                    return apiResponseDataList;
                }
                else
                {
                    return new List<Dictionary<string, object>>(); // Return an empty list in case of failure or throw an custom exception for non successful request
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during retrieval of data:  {ex.Message}");
                return new List<Dictionary<string, object>>(); // Return an empty list in case of exception
            }
        }
        //



        //GET Commission Summary
        public async Task<List<ReportsDataCommissionSummary>> GetCommissionSummary(string optionParam, string dFrom, string dTo)
        {


            string api_summary_endpoint_url = custom.BuildUrl(_apiBaseUrl, "ReportsCSummaryEndpointBase", _apiConfig, (_apiConfig.Endpoints["ReportsCOptionParam"], optionParam), (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey), (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom), (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo));

            Console.WriteLine($"Test URI Built for Report Commission Summary is : {api_summary_endpoint_url}");

            var responseData = "";
            try
            {
                var response = await _httpClient.GetAsync(api_summary_endpoint_url);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();


                    var reportsCommissionSummary = JsonConvert.DeserializeObject<List<ReportsDataCommissionSummary>>(responseData);
                    Console.WriteLine("Summary : " + responseData);
                    return reportsCommissionSummary;
                }
                else
                {
                    throw new HttpRequestException(); //Create Custom exceptions with less boilerplate
                }

            }
            catch (HttpRequestException ex)
            {
                return new List<ReportsDataCommissionSummary>();
            }

        }

        public async Task<List<Dictionary<string, object>>> GetCommissionSummary_AsDict(string optionParam, string dFrom, string dTo)
        {


            string api_summary_endpoint_url = custom.BuildUrl(_apiBaseUrl, "ReportsCSummaryEndpointBase", _apiConfig, (_apiConfig.Endpoints["ReportsCOptionParam"], optionParam), (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey), (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom), (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo));

            Console.WriteLine($"Test URI Built for Report Commission Summary is : {api_summary_endpoint_url}");

            var responseData = "";
            try
            {
                var response = await _httpClient.GetAsync(api_summary_endpoint_url);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();


                    var reportsCommissionSummary = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseData);
                    Console.WriteLine("Summary : " + responseData);
                    return reportsCommissionSummary;
                }
                else
                {
                    throw new HttpRequestException(); //Create Custom exceptions with less boilerplate
                }

            }
            catch (HttpRequestException ex)
            {
                return new List<Dictionary<string, object>>();
            }

        }
        public async Task<ApiResponseModel<CommissionCtrModel>> GetCommCtr()
        {
            Console.WriteLine($"[DEBUG] from API Ctr Account Check client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "GetCommissionCtr", _apiConfig, (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Ctr : {api_verification_activation}");
                var GET_wallet_balance_response = await _httpClient.GetAsync(api_verification_activation);
                if (GET_wallet_balance_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_wallet_balance_response.Content.ReadAsStringAsync();
                    var verification_response_asJson = JsonConvert.DeserializeObject<CommissionCtrModel>(responseData);
                    Console.WriteLine("[DEBUG] QR Response Content : " + responseData);

                    return new ApiResponseModel<CommissionCtrModel>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Ctr Fetched!!",
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
                return new ApiResponseModel<CommissionCtrModel>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot fetch Ctr details : {e.Message}",
                    Data = null
                };
            }
        }




        public async Task<ApiResponseModel<SystemAccessModel>> GetSystemAccess()
        {
            Console.WriteLine($"[DEBUG] from API Ctr Account Check client to building url base url {_apiConfig.BaseUrl}");
            
            string getSystemMobileAccess = custom.BuildUrl(_apiBaseUrl, "SystemAccessBase", _apiConfig, (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey), (_apiConfig.Endpoints["SystemAppIdParam"], "1"));



            try
            {
                Console.WriteLine($"[DEBUG] API Built for systemAccess : {getSystemMobileAccess}");
                var GET_wallet_balance_response = await _httpClient.GetAsync(getSystemMobileAccess);
                if (GET_wallet_balance_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_wallet_balance_response.Content.ReadAsStringAsync();
                    var verification_response_asJson = JsonConvert.DeserializeObject<SystemAccessModel>(responseData);
                    Console.WriteLine("[DEBUG] QR Response Content : " + responseData);

                    return new ApiResponseModel<SystemAccessModel>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "System Access Status Fetched!",
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
                return new ApiResponseModel<SystemAccessModel>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot fetch Ctr details : {e.Message}",
                    Data = null
                };
            }
        }



        public async Task<ApiResponseModel<accountRank>> GetRank()
        {
            Console.WriteLine($"[DEBUG] from API Ctr Account Check client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "GetRank", _apiConfig, (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for rank : {api_verification_activation}");
                var GET_wallet_balance_response = await _httpClient.GetAsync(api_verification_activation);
                if (GET_wallet_balance_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_wallet_balance_response.Content.ReadAsStringAsync();
                    var verification_response_asJson = JsonConvert.DeserializeObject<accountRank>(responseData);
                    Console.WriteLine("[DEBUG] QR Response Content : " + responseData);

                    return new ApiResponseModel<accountRank>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Rank Fetched!!",
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
                return new ApiResponseModel<accountRank>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot fetch rank details : {e.Message}",
                    Data = null
                };
            }
        }





        //GET Commission History //TODO
        public async Task<List<ReportsDataCommissionHistory>> GetReportsCommissionHistory(string optionParam, string dFrom, string dTo)
        {
            string api_history_endpoint_url = custom.BuildUrl(_apiBaseUrl, "ReportsCHistoryEndpointBase", _apiConfig, (_apiConfig.Endpoints["ReportsCOptionParam"], optionParam), (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey), (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom), (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo));


            Console.WriteLine($"Test URI Built is : {api_history_endpoint_url}");

            var responseData = "";
            try
            {
                var response = await _httpClient.GetAsync(api_history_endpoint_url);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseData);
                    var apiResponseCoverageHistory = JsonConvert.DeserializeObject<List<ReportsDataCommissionHistory>>(responseData);
                    Console.WriteLine("Summary : " + responseData);
                    return apiResponseCoverageHistory;
                }
                else
                {
                    throw new HttpRequestException(); //Create Custom exceptions with less boilerplate
                }

            }
            catch (HttpRequestException ex)
            {
                return null;
            }

        }

        public async Task<List<Dictionary<string, object>>> GetReportsCommissionHistory_AsDict(string optionParam, string dFrom, string dTo)
        {
            string api_history_endpoint_url = custom.BuildUrl(_apiBaseUrl, "ReportsCHistoryEndpointBase", _apiConfig, (_apiConfig.Endpoints["ReportsCOptionParam"], optionParam), (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey), (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom), (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo));


            Console.WriteLine($"Test URI Built is : {api_history_endpoint_url}");

            var responseData = "";
            try
            {
                var response = await _httpClient.GetAsync(api_history_endpoint_url);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();

                    var apiResponseCoverageHistory = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseData);
                    Console.WriteLine("Summary : " + responseData);
                    return apiResponseCoverageHistory;
                }
                else
                {
                    throw new HttpRequestException(); //Create Custom exceptions with less boilerplate
                }

            }
            catch (HttpRequestException ex)
            {
                return null;
            }

        }


        //TODO Work on switch case for 
        // 1 - referral
        // 2 - DSC
        // 3 - ISC
        // 4 - J4U
        // 5 - GM5

        public async Task<List<Dictionary<string, object>>> GetReferralCommissionAsDict(string optionParam, string dFrom, string dTo)
        {
            string api_dsc_endpoint_url = custom.BuildUrl(_apiBaseUrl, "ReportsCReferralEndpointBase", _apiConfig, (_apiConfig.Endpoints["ReportsCOptionParam"], optionParam), (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey), (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom), (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo));


            Console.WriteLine($"Test URI Built is : {api_dsc_endpoint_url}");

            var responseData = "";
            var apiResponseReferralCommission = new List<Dictionary<string, object>>();
            try
            {
                var referralResponse = await _httpClient.GetAsync(api_dsc_endpoint_url);

                if (referralResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await referralResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Summary : " + responseData);

                        try
                        {
                            apiResponseReferralCommission = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(responseData);
                        }
                        catch (Exception ex)
                        {
                            if (responseData.ToLower() == "No records found!".ToLower())
                            {
                                Dictionary<string, object> error = null;
                                error.Add("Error", $"Error Description {responseData}");
                                apiResponseReferralCommission.Add(error);
                            }

                        }



                        return apiResponseReferralCommission;

                    }
                    catch (Exception ex)
                    {
                        throw new HttpRequestException();
                    }

                }
                else
                {
                    throw new HttpRequestException(); //Create Custom exceptions with less boilerplate
                }


            }
            catch (HttpRequestException ex)
            {
                return null;
            }

        }

        public async Task<List<ReportsDataDSCCommission>> GetReferralCommission(string optionParam, string dFrom, string dTo)
        {
            string api_dsc_endpoint_url = custom.BuildUrl(_apiBaseUrl, "ReportsCReferralEndpointBase", _apiConfig, (_apiConfig.Endpoints["ReportsCOptionParam"], optionParam), (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey), (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom), (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo));


            Console.WriteLine($"Test URI Built is : {api_dsc_endpoint_url}");

            var responseData = "";
            var apiResponseReferralCommission = new List<ReportsDataDSCCommission>();
            try
            {
                var referralResponse = await _httpClient.GetAsync(api_dsc_endpoint_url);

                if (referralResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await referralResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from Referral Comission Function : " + responseData);

                        try
                        {
                            apiResponseReferralCommission = JsonConvert.DeserializeObject<List<ReportsDataDSCCommission>>(responseData);
                        }
                        catch (Exception ex)
                        {
                            if (responseData.ToLower() == "No records found!".ToLower())
                            {

                                apiResponseReferralCommission.Add(null);
                            }

                        }



                        return apiResponseReferralCommission;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex}");


                        throw new HttpRequestException();
                    }

                }
                else
                {
                    throw new HttpRequestException(); //Create Custom exceptions with less boilerplate
                }


            }
            catch (HttpRequestException ex)
            {
                return null;
            }

        }

        // UNILEVEL

        public async Task<ApiResponseModel<List<ReportsDataUnilevelCommission>>> GetUnilevelSales(string dFrom)
        {
            string api_dsc_endpoint_url = custom.BuildUrl(_apiBaseUrl, "ReportsUnilevelEndpointBase", _apiConfig, (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey), (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom));

            Console.WriteLine($"Test UNIL Built URL: {api_dsc_endpoint_url}");

            var apiResponseReferralCommission = new List<ReportsDataUnilevelCommission>();
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromMinutes(2)); // Set a timeout of 30 seconds
                var referralResponse = await _httpClient.GetAsync(api_dsc_endpoint_url, cts.Token);

                if (referralResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        using (var stream = await referralResponse.Content.ReadAsStreamAsync())
                        using (var streamReader = new StreamReader(stream))
                        using (var jsonReader = new JsonTextReader(streamReader))
                        {
                            var serializer = new JsonSerializer();
                            apiResponseReferralCommission = serializer.Deserialize<List<ReportsDataUnilevelCommission>>(jsonReader);
                        }

                        Console.WriteLine("Data Fetch from Unilevel Commission Function: " + apiResponseReferralCommission.Count + " records fetched.");

                        return new ApiResponseModel<List<ReportsDataUnilevelCommission>> { IsSuccess = true, Description = "Fetched USC!", Data = apiResponseReferralCommission };
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error during data processing: {ex.Message}");
                        throw new HttpRequestException(ex.Message);
                    }
                }
                else
                {
                    string errorContent = await referralResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {referralResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException(message: $"Non-success status code received: {referralResponse.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP request error: {ex.Message}");
                return new ApiResponseModel<List<ReportsDataUnilevelCommission>> { IsSuccess = false, Description = $"{ex.Message}", Data = null };
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timed out: {ex.Message}");
                return new ApiResponseModel<List<ReportsDataUnilevelCommission>> { IsSuccess = false, Description = "Request timed out", Data = null };
            }
        }



        public async Task<ApiResponseModel<List<ReportsDataUnilevelLeadershipCommission>>> GetUnilevelSalesLeadership(string dFrom)
        {
            string api_dsc_endpoint_url = custom.BuildUrl(_apiBaseUrl, "UnilevelLeadershipBase", _apiConfig, (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey), (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom));


            Console.WriteLine($"Test URI Leadership Built is : {api_dsc_endpoint_url}");

            var responseData = "";
            var apiResponseReferralCommission = new List<ReportsDataUnilevelLeadershipCommission>();
            try
            {
                var referralResponse = await _httpClient.GetAsync(api_dsc_endpoint_url);

                if (referralResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await referralResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from Unilevel Leadership Comission Function : " + responseData);

                        try
                        {
                            apiResponseReferralCommission = JsonConvert.DeserializeObject<List<ReportsDataUnilevelLeadershipCommission>>(responseData);
                        }
                        catch (Exception ex)
                        {
                            if (responseData.ToLower() == "No records found!".ToLower())
                            {

                                apiResponseReferralCommission.Add(null);
                            }

                        }



                        return new ApiResponseModel<List<ReportsDataUnilevelLeadershipCommission>> { IsSuccess = true, Description = "Fetched ULC!", Data = apiResponseReferralCommission };

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex}");


                        throw new HttpRequestException(ex.Message);
                    }

                }
                else
                {
                    string errorContent = await referralResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {referralResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException(message: $"Non-success status code received: {referralResponse.StatusCode}"); //Create Custom exceptions with less boilerplate
                }


            }
            catch (HttpRequestException ex)
            {

                return new ApiResponseModel<List<ReportsDataUnilevelLeadershipCommission>> { IsSuccess = false, Description = $"{ex.Message}", Data = null };

            }

        }

        public async Task<ApiResponseModel<List<ResidualSalesMatch>>> GetResidualSalesMatch(string dFrom, string dTo)
        {
            var responseData = "";
            var listOfResidualSalesMatch = new List<ResidualSalesMatch>();
            var apiResponseResidualSalesMatch = new ApiResponseModel<List<ResidualSalesMatch>>();

            string api_residualsalesmatch_endpoint_url = custom.BuildUrl(
                _apiBaseUrl,
                "ReportsRSM",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom),
                (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo),
                (_apiConfig.Endpoints["ReportsLatestParam"], "true")
            );

            Console.WriteLine($"Test URI Built is : {api_residualsalesmatch_endpoint_url}");

            try
            {
                var residualSalesMatchResponse = await _httpClient.GetAsync(api_residualsalesmatch_endpoint_url);

                if (residualSalesMatchResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await residualSalesMatchResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from Residual Sales Match Function : " + responseData);
                        Console.WriteLine("Data Fetch from Residual Sales Match Function COUNT : " + responseData.Count());

                        try
                        {
                            listOfResidualSalesMatch = JsonConvert.DeserializeObject<List<ResidualSalesMatch>>(responseData);
                            apiResponseResidualSalesMatch = new ApiResponseModel<List<ResidualSalesMatch>> { IsSuccess = true, StatusCode = 200, Description = "Success in fetching residual sales match", Data = listOfResidualSalesMatch };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            if (responseData.ToLower() == "no records found!")
                            {
                                listOfResidualSalesMatch.Add(null);
                                apiResponseResidualSalesMatch = new ApiResponseModel<List<ResidualSalesMatch>> { StatusCode = 500, Description = "Failed in fetching residual sales match", Data = listOfResidualSalesMatch };
                            }
                            else
                            {
                                throw new HttpRequestException(message: "Could not parse error");
                            }
                        }

                        return apiResponseResidualSalesMatch;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException(message: "Error encountered while processing the response data.", inner: ex);
                    }
                }
                else
                {
                    string errorContent = await residualSalesMatchResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {residualSalesMatchResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException(message: $"Non-success status code received: {residualSalesMatchResponse.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return null;
            }
        }

        public async Task<ApiResponseModel<List<moverSummary>>> GetMoverSummary()
        {
            var responseData = "";
            var listOfMoverSummary = new List<moverSummary>();
            var apiResponseMoverSummary = new ApiResponseModel<List<moverSummary>>();

            string api_moversummary_endpoint_url = custom.BuildUrl(
                _apiBaseUrl,
                "MoverHistoryBase",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey)
                
            );

            Console.WriteLine($"Test URI Built is : {api_moversummary_endpoint_url}");

            try
            {
                var moverSummaryResponse = await _httpClient.GetAsync(api_moversummary_endpoint_url);

                if (moverSummaryResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await moverSummaryResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from Mover Summary Function : " + responseData);
                        Console.WriteLine("Data Fetch from Mover Summary Function COUNT : " + responseData.Count());

                        try
                        {
                            listOfMoverSummary = JsonConvert.DeserializeObject<List<moverSummary>>(responseData);
                            apiResponseMoverSummary = new ApiResponseModel<List<moverSummary>>
                            {
                                IsSuccess = true,
                                StatusCode = 200,
                                Description = "Success in fetching mover summary",
                                Data = listOfMoverSummary
                            };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            if (responseData.ToLower() == "no records found!")
                            {
                                listOfMoverSummary.Add(null);
                                apiResponseMoverSummary = new ApiResponseModel<List<moverSummary>>
                                {
                                    IsSuccess = false,
                                    StatusCode = 500,
                                    Description = "Failed in fetching mover summary",
                                    Data = listOfMoverSummary
                                };
                            }
                            else
                            {
                                throw new HttpRequestException(message: "Could not parse error");
                            }
                        }

                        return apiResponseMoverSummary;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException(message: "Error encountered while processing the response data.", inner: ex);
                    }
                }
                else
                {
                    string errorContent = await moverSummaryResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {moverSummaryResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException(message: $"Non-success status code received: {moverSummaryResponse.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return null;
            }
        }

        public async Task<ApiResponseModel<List<infinityHistory>>> GetInfinityHistory(string dFrom, string dTo)
        {
            var responseData = "";
            var listOfPairHistory = new List<infinityHistory>();
            var apiResponseReferralCommission = new ApiResponseModel<List<infinityHistory>>();

            
                var api_infinityHistory_endpoint_url = custom.BuildUrl(
                    _apiBaseUrl,
                    "InfinityHistoryBase",
                    _apiConfig,
                    (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                    (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom),
                    (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo)
                );

                Console.WriteLine($"Test URI Built is : {api_infinityHistory_endpoint_url}");

                try
                {
                    var referralResponse = await _httpClient.GetAsync(api_infinityHistory_endpoint_url);

                    if (referralResponse.IsSuccessStatusCode)
                    {
                        try
                        {
                            responseData = await referralResponse.Content.ReadAsStringAsync();
                            Console.WriteLine("Data Fetch from Pairs History Comission Function : " + responseData);
                            Console.WriteLine("Data Fetch from Pairs History Comission Function COUNT : " + responseData.Count());

                            try
                            {

                                listOfPairHistory = JsonConvert.DeserializeObject<List<infinityHistory>>(responseData);
                                apiResponseReferralCommission = new ApiResponseModel<List<infinityHistory>> { IsSuccess = true, StatusCode = 200, Description = "Success in fetching pair history", Data = listOfPairHistory };

                            }
                            catch (JsonException ex)
                            {
                                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                                if (responseData.ToLower() == "no records found!")
                                {
                                    //listOfPairHistory.Add(null);
                                    apiResponseReferralCommission = new ApiResponseModel<List<infinityHistory>> { IsSuccess = false, StatusCode = 500, Description = ex.Message, Data = listOfPairHistory };

                                }
                                else
                                {
                                    throw new HttpRequestException(message: responseData);
                                }
                            }

                            return apiResponseReferralCommission;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                            throw new HttpRequestException(message: "Error encountered while processing the response data.", inner: ex);
                        }
                    }
                    else
                    {
                        string errorContent = await referralResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: Received non-success status code {referralResponse.StatusCode}");
                        Console.WriteLine($"Error Content: {errorContent}");
                        throw new HttpRequestException(message: $"Non-success status code received: {referralResponse.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HttpRequestException: {ex.Message}");
                    return null;
                }

            
        }
         

        public async Task<ApiResponseModel<List<rqvSummary>>> GetRQVSummary(string dFrom, string dTo)
        {
            var responseData = "";
            var listOfRqvSummary = new List<rqvSummary>();
            var apiResponseRqvSummary = new ApiResponseModel<List<rqvSummary>>();

            string api_rqvsummary_endpoint_url = custom.BuildUrl(
                _apiBaseUrl,
                "RqvHistoryBase",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom),
                (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo)
            );

            Console.WriteLine($"Test URI Built is : {api_rqvsummary_endpoint_url}");

            try
            {
                var rqvSummaryResponse = await _httpClient.GetAsync(api_rqvsummary_endpoint_url);

                if (rqvSummaryResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await rqvSummaryResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from RQV Summary Function : " + responseData);
                        Console.WriteLine("Data Fetch from RQV Summary Function COUNT : " + responseData.Count());

                        try
                        {
                            listOfRqvSummary = JsonConvert.DeserializeObject<List<rqvSummary>>(responseData);
                            apiResponseRqvSummary = new ApiResponseModel<List<rqvSummary>>
                            {
                                IsSuccess = true,
                                StatusCode = 200,
                                Description = "Success in fetching RQV summary",
                                Data = listOfRqvSummary
                            };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            if (responseData.ToLower() == "no records found!")
                            {
                                listOfRqvSummary.Add(null);
                                apiResponseRqvSummary = new ApiResponseModel<List<rqvSummary>>
                                {
                                    IsSuccess = false,
                                    StatusCode = 500,
                                    Description = "Failed in fetching RQV summary",
                                    Data = listOfRqvSummary
                                };
                            }
                            else
                            {
                                throw new HttpRequestException(message: "Could not parse error");
                            }
                        }

                        return apiResponseRqvSummary;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException(message: "Error encountered while processing the response data.", inner: ex);
                    }
                }
                else
                {
                    string errorContent = await rqvSummaryResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {rqvSummaryResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException(message: $"Non-success status code received: {rqvSummaryResponse.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return null;
            }
        }



        public async Task<ApiResponseModel<List<MainProductPurchaseResponse>>> GetPurchaseHistoryList(string idno, string option, string dfrom, string dto)
        {
            var responseData = "";
            var listOfPurchaseObject = new List<MainProductPurchaseResponse>();
            var apiResponsePurchasesHistory = new ApiResponseModel<List<MainProductPurchaseResponse>>();

            string apiEndpointUrl = custom.BuildUrl(
                _apiBaseUrl,
                "ReportsPurchaseHistoryListBase",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCOptionParam"], option),
                //(_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                (_apiConfig.Endpoints["IdNoParam"], idno),
                (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dfrom),
                (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dto)
            );

            Console.WriteLine($"Test URI Built is : {apiEndpointUrl}");

            try
            {
                var rqvSummaryResponse = await _httpClient.GetAsync(apiEndpointUrl);

                if (rqvSummaryResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await rqvSummaryResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from RQV Summary Function : " + responseData);
                        Console.WriteLine("Data Fetch from RQV Summary Function COUNT : " + responseData.Count());

                        try
                        {
                            listOfPurchaseObject = JsonConvert.DeserializeObject<List<MainProductPurchaseResponse>>(responseData);
                            apiResponsePurchasesHistory = new ApiResponseModel<List<MainProductPurchaseResponse>>
                            {
                                IsSuccess = true,
                                StatusCode = 200,
                                Description = "Success in fetching RQV summary",
                                Data = listOfPurchaseObject
                            };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            if (responseData.ToLower() == "no records found!")
                            {
                                listOfPurchaseObject.Add(null);
                                apiResponsePurchasesHistory = new ApiResponseModel<List<MainProductPurchaseResponse>>
                                {
                                    IsSuccess = false,
                                    StatusCode = 500,
                                    Description = "Failed in fetching RQV summary",
                                    Data = listOfPurchaseObject
                                };
                            }
                            else
                            {
                                throw new HttpRequestException(message: "Could not parse error");
                            }
                        }

                        return apiResponsePurchasesHistory;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException(message: "Error encountered while processing the response data.", inner: ex);
                    }
                }
                else
                {
                    string errorContent = await rqvSummaryResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {rqvSummaryResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException(message: $"Non-success status code received: {rqvSummaryResponse.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return null;
            }
        }




        public async Task<ApiResponseModel<MainProductPurchaseResponse>> GetPurchaseHistoryListV1(string idno, string option, string dfrom, string dto)
        {
            var responseData = "";
            var PurchaseHistory = new MainProductPurchaseResponse();
            var apiResponse = new ApiResponseModel<MainProductPurchaseResponse>();

            string apiEndpointUrl = custom.BuildUrl(
                _apiBaseUrl,
                "ReportsPurchaseHistoryListBase",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCOptionParam"], option),
                //(_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                (_apiConfig.Endpoints["IdNoParam"], idno),
                (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dfrom),
                (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dto)
            );

            Console.WriteLine($"API Endpoint URL for PurchaseReports: {apiEndpointUrl}");

            try
            {
                var response = await _httpClient.GetAsync(apiEndpointUrl);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetched from Summary PurchaseReports: " + responseData);

                        try
                        {
                            PurchaseHistory = JsonConvert.DeserializeObject<MainProductPurchaseResponse>(responseData);
                            apiResponse = new ApiResponseModel<MainProductPurchaseResponse> { IsSuccess = true, StatusCode = 200, Description = "Success in fetching summary binary reports", Data = PurchaseHistory };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            throw new HttpRequestException("Error parsing JSON response");
                        }

                        return apiResponse;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException("Error encountered while processing the response data for Purchase History.", ex);
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code Purchase {response.StatusCode}");
                    Console.WriteLine($"Error Content Purchase: {errorContent}");
                    throw new HttpRequestException($"Non-success status code received: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException from Purchase: {ex.Message}");
                return null;
            }
        }




        public async Task<ApiResponseModel<List<ReportsCommHistory>>> GetSummaryBinaryReports(string option, string dfrom, string dto)
        {
            var responseData = "";
            var listOfReportsCommHistory = new List<ReportsCommHistory>();
            var apiResponse = new ApiResponseModel<List<ReportsCommHistory>>();

            string apiEndpointUrl = custom.BuildUrl(
                _apiBaseUrl,
                "ReportsBinarySummaryList",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCOptionParam"], option),
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dfrom),
                (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dto)
            );

            Console.WriteLine($"API Endpoint URL: {apiEndpointUrl}");

            try
            {
                var response = await _httpClient.GetAsync(apiEndpointUrl);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetched from Summary Binary Reports Function: " + responseData);

                        try
                        {
                            listOfReportsCommHistory = JsonConvert.DeserializeObject<List<ReportsCommHistory>>(responseData);
                            apiResponse = new ApiResponseModel<List<ReportsCommHistory>> { IsSuccess = true, StatusCode = 200, Description = "Success in fetching summary binary reports", Data = listOfReportsCommHistory };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            throw new HttpRequestException("Error parsing JSON response");
                        }

                        return apiResponse;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException("Error encountered while processing the response data.", ex);
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {response.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException($"Non-success status code received: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return null;
            }
        }



        public async Task<ApiResponseModel<List<TotalUnicomModel>>> GetTotalUnicom()
        {
            var responseData = "";
          
            var apiResponse = new ApiResponseModel<List<TotalUnicomModel>>();

            string apiEndpointUrl = custom.BuildUrl(
                _apiBaseUrl,
                "GetTotalUnicom",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey)
                
            );

            Console.WriteLine($"API Endpoint URL: {apiEndpointUrl}");

            try
            {
                var response = await _httpClient.GetAsync(apiEndpointUrl);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetched from Summary Total Unicom Function: " + responseData);

                        try
                        {
                            var deserializedResponse = JsonConvert.DeserializeObject<List<TotalUnicomModel>>(responseData);
                            
                            apiResponse = new ApiResponseModel<List<TotalUnicomModel>> { IsSuccess = true, StatusCode = 200, Description = "Success in fetching summary unicom reports", Data = deserializedResponse };
                            return apiResponse;
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            if (responseData.ToLower().Contains("no record found"))
                            {
                                apiResponse = new ApiResponseModel<List<TotalUnicomModel>> { IsSuccess = true, StatusCode = 200, Description = $"{responseData}", Data = new List<TotalUnicomModel>() };
                            }
                            else
                            {
                                apiResponse = new ApiResponseModel<List<TotalUnicomModel>> { IsSuccess = false, StatusCode = 500, Description = $"{responseData}", Data = new List<TotalUnicomModel>() };
                            }
                           
                            return apiResponse;
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                        throw new HttpRequestException(ex.Message);
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {response.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException($"Non-success status code received: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
        }




        public async Task<ApiResponseModel<List<ReportsCheckMatched>>> CheckMatch(string dFrom, string dTo)
        {
            var responseData = "";
            var listOfCheckMatch = new List<ReportsCheckMatched>();
            var apiResponseCheckMatch = new ApiResponseModel<List<ReportsCheckMatched>>();

            string api_checkmatch_endpoint_url = custom.BuildUrl(
                _apiBaseUrl,
                "ReportsCheckMatch",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom),
                (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo),
                (_apiConfig.Endpoints["ReportsLatestParam"], "true")
            );

            Console.WriteLine($"Test URI Built is : {api_checkmatch_endpoint_url}");

            try
            {
                var checkmatchResponse = await _httpClient.GetAsync(api_checkmatch_endpoint_url);

                if (checkmatchResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await checkmatchResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from Check Match Function : " + responseData);
                        Console.WriteLine("Data Fetch from Check Match Function COUNT : " + responseData.Count());

                        try
                        {
                            listOfCheckMatch = JsonConvert.DeserializeObject<List<ReportsCheckMatched>>(responseData);
                            apiResponseCheckMatch = new ApiResponseModel<List<ReportsCheckMatched>> { IsSuccess = true, StatusCode = 200, Description = "Success in fetching check match", Data = listOfCheckMatch };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            if (responseData.ToLower() == "no records found!")
                            {
                                listOfCheckMatch.Add(null);
                                apiResponseCheckMatch = new ApiResponseModel<List<ReportsCheckMatched>> { StatusCode = 500, Description = "Failed in fetching check match", Data = listOfCheckMatch };
                            }
                            else
                            {
                                throw new HttpRequestException(message: "Could not parse error");
                            }
                        }

                        return apiResponseCheckMatch;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException(message: "Error encountered while processing the response data.", inner: ex);
                    }
                }
                else
                {
                    string errorContent = await checkmatchResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {checkmatchResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException(message: $"Non-success status code received: {checkmatchResponse.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return null;
            }
        }



        //

        public async Task<ApiResponseModel<List<ReportsDataPairsHistory>>>GetPairsHistory(string dFrom, string dTo)
        {
            var responseData = "";
            var listOfPairHistory = new List<ReportsDataPairsHistory>();
            var apiResponseReferralCommission = new ApiResponseModel<List<ReportsDataPairsHistory>>();

            string api_dsc_endpoint_url = custom.BuildUrl(
                _apiBaseUrl,
                "ReportsPairHistoryEndpointBase",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom),
                (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo),
                (_apiConfig.Endpoints["ReportsLatestParam"], "true")
            );

            Console.WriteLine($"Test URI Built is : {api_dsc_endpoint_url}");

            try
            {
                var referralResponse = await _httpClient.GetAsync(api_dsc_endpoint_url);

                if (referralResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await referralResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from Pairs History Comission Function : " + responseData);
                        Console.WriteLine("Data Fetch from Pairs History Comission Function COUNT : " + responseData.Count());

                        try
                        {

                            listOfPairHistory = JsonConvert.DeserializeObject<List<ReportsDataPairsHistory>>(responseData);
                            apiResponseReferralCommission = new ApiResponseModel<List<ReportsDataPairsHistory>> { IsSuccess=true, StatusCode = 200, Description = "Success in fetching pair history", Data = listOfPairHistory };
                            
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            if (responseData.ToLower() == "no records found!")
                            {
                                listOfPairHistory.Add(null);
                                apiResponseReferralCommission = new ApiResponseModel<List<ReportsDataPairsHistory>> { StatusCode = 500, Description = "Failed in fetching pair history", Data = listOfPairHistory };

                            }
                            else
                            {
                                throw new HttpRequestException(message: "Could not parse error");
                            }
                        }

                        return apiResponseReferralCommission;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException(message: "Error encountered while processing the response data.", inner: ex);
                    }
                }
                else
                {
                    string errorContent = await referralResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {referralResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException(message: $"Non-success status code received: {referralResponse.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return null;
            }
           
        }


        public async Task<ApiResponseModel<ReportsDataPairsHistory>> GetLatestPairHistory(string dFrom, string dTo)
        {
            var responseData = "";
            var apiResponseReferralCommission = new ApiResponseModel<ReportsDataPairsHistory>();

            string api_dsc_endpoint_url = custom.BuildUrl(
                _apiBaseUrl,
                "ReportsPairHistoryEndpointBase",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom),
                (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo),
                (_apiConfig.Endpoints["ReportsLatestParam"], "true") // Add parameter to fetch the latest record
            );

            Console.WriteLine($"Test URI Built is : {api_dsc_endpoint_url}");

            try
            {
                var referralResponse = await _httpClient.GetAsync(api_dsc_endpoint_url);

                if (referralResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await referralResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from Pairs History Commission Function : " + responseData);

                        var latestRecord = new ReportsDataPairsHistory();
                        try
                        {
                            latestRecord = JsonConvert.DeserializeObject<ReportsDataPairsHistory>(responseData);

                            apiResponseReferralCommission = new ApiResponseModel<ReportsDataPairsHistory>
                            {
                                StatusCode = 200,
                                Description = "Success in fetching pair history",
                                Data = latestRecord
                            };
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            throw new HttpRequestException("Could not parse response data.", ex);
                        }

                        return apiResponseReferralCommission;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException("Error encountered while processing the response data.", ex);
                    }
                }
                else
                {
                    string errorContent = await referralResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {referralResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException($"Non-success status code received: {referralResponse.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return null;
            }
        }




        //

        //experimental :

        // using binary search to try optimize fetching without api modificationnnnn :>
        public async Task<ApiResponseModel<ReportsDataPairsHistory>> GetLatestPairList(string dFrom, string dTo)
        {
            DateTime startDate = DateTime.Parse(dFrom);
            DateTime endDate = DateTime.Parse(dTo);
            var apiResponse = new ApiResponseModel<List<ReportsDataPairsHistory>>();

            while (startDate <= endDate)
            {
                DateTime midDate = startDate.AddDays((endDate - startDate).TotalDays / 2);
                var response = await FetchPairHistoryData(startDate.ToString("MM/dd/yyyy"), midDate.ToString("MM/dd/yyyy"));

                if (response.Data != null && response.Data.Count > 0)
                {
                    // There are records in this range, so narrow down to the later half
                    startDate = midDate.AddDays(1);
                    apiResponse = response; // Save this as the latest response with data
                }
                else
                {
                    // No records in this range, so narrow down to the earlier half
                    endDate = midDate.AddDays(-1);
                }
            }

            // The latest record found in the smallest valid date range
            if (apiResponse.Data != null && apiResponse.Data.Count > 0)
            {
                var latestRecord = apiResponse.Data.OrderByDescending(r => r.tranDate).FirstOrDefault();
                return new ApiResponseModel<ReportsDataPairsHistory>
                {
                    StatusCode = 200,
                    Description = "Success in fetching pair history",
                    Data = latestRecord
                };
            }
            else
            {
                return new ApiResponseModel<ReportsDataPairsHistory>
                {
                    StatusCode = 404,
                    Description = "No records found"
                };
            }
        }

        private async Task<ApiResponseModel<List<ReportsDataPairsHistory>>> FetchPairHistoryData(string dFrom, string dTo)
        {
            string api_dsc_endpoint_url = custom.BuildUrl(
                _apiBaseUrl,
                "ReportsPairHistoryEndpointBase",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey),
                (_apiConfig.Endpoints["ReportsCEndpointDfromParam"], dFrom),
                (_apiConfig.Endpoints["ReportsCEndpointDtoParam"], dTo)
            );

            Console.WriteLine($"Fetching data from {dFrom} to {dTo}");

            var responseData = "";
            var apiResponse = new ApiResponseModel<List<ReportsDataPairsHistory>>();

            try
            {
                var referralResponse = await _httpClient.GetAsync(api_dsc_endpoint_url);

                if (referralResponse.IsSuccessStatusCode)
                {
                    responseData = await referralResponse.Content.ReadAsStringAsync();
                    var listOfPairHistory = JsonConvert.DeserializeObject<List<ReportsDataPairsHistory>>(responseData);
                    apiResponse = new ApiResponseModel<List<ReportsDataPairsHistory>>
                    {
                        StatusCode = 200,
                        Description = "Success in fetching pair history",
                        Data = listOfPairHistory
                    };
                }
                else
                {
                    apiResponse = new ApiResponseModel<List<ReportsDataPairsHistory>>
                    {
                        StatusCode = (int)referralResponse.StatusCode,
                        Description = "Failed to fetch pair history",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                apiResponse = new ApiResponseModel<List<ReportsDataPairsHistory>>
                {
                    StatusCode = 500,
                    Description = "Exception occurred while fetching pair history",
                    Data = null
                };
            }

            return apiResponse;
        }



    }
}

//TODO: 1. Restructure Cleints & Bridges: Add custom exceptions, refactor and reoptimize some functions,
