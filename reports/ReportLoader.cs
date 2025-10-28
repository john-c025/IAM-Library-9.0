using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.dashboard;
using IAM_Library.models.auth;
using IAM_Library.models.dashboard;
using IAM_Library.models.general;
using IAM_Library.models.reports;
using IAM_Library.models.wallet;
using IAM_Library.utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.reports
{
    public class ReportLoader(AuthApiResponseData credentials, HttpClient _httpClient)
    {
       

            private static string apiBaseUrl = Encryption.decodeString(_constants.authBaseUrl);

            private ReportsApiClient reportsClient = new ReportsApiClient(apiBaseUrl,credentials,_httpClient);



            public async Task<List<ReportsDataCommissionCoverage>> LoadCoverageList()
            {
                

                var getCoverage = new List<ReportsDataCommissionCoverage>();
                AuthApiResponseData myCreds = credentials;


                try
                {
                    var coverageList = await reportsClient.GetCoverageDatesList();

                    getCoverage.AddRange(coverageList);
                }
                catch (Exception e)
                {
                    throw e;
                }

                return getCoverage;

            }

            public async Task<List<Dictionary<string, object>>> LoadCoverageListAsDict()
            {


                var getCoverage = new List<Dictionary<string, object>>();
                AuthApiResponseData myCreds = credentials;


                try
                {
                    var coverageList = await reportsClient.GetCoverageDatesList_AsDict();

                    getCoverage.AddRange(coverageList);
                }
                catch (Exception e)
                {
                    throw e;
                }

                return getCoverage;

            }

        public async Task<ApiResponseModel<List<MainProductPurchaseResponse>>> LoadPurchaseHistory(string idno,string option, string date_from, string date_to)
        {

            ApiResponseModel<List<MainProductPurchaseResponse>> apiResponse = new ApiResponseModel<List<MainProductPurchaseResponse>>();
          


            AuthApiResponseData myCreds = credentials;

            try
            {
                var response = await reportsClient.GetPurchaseHistoryList(idno,option, date_from, date_to);
                Console.WriteLine("TEST FROM LOADER RESPONSE PURCHASE HISTORY");
                Console.WriteLine(response);
                apiResponse = response;
                //data = apiResponse.Data;
            }
            catch (Exception e)
            {
                throw e;
            }
            Console.WriteLine("TEST FROM LOADER - Purchase History");
            Console.WriteLine(apiResponse);


            return apiResponse;

        }

        public async Task<List<ReportsDataCommissionSummary>> LoadCommissionSummary(string option, string date_from, string date_to)
            {

                List<ReportsDataCommissionSummary> apiResponse = new List<ReportsDataCommissionSummary>();
                var getSummary = new ReportsDataCommissionSummary();

                var data = new ReportsDataCommissionSummary();

                AuthApiResponseData myCreds = credentials;

                try
                {
                    var response = await reportsClient.GetCommissionSummary(option, date_from, date_to);
                    Console.WriteLine("TEST FROM LOADER RESPONSE");
                    Console.WriteLine(response);
                    apiResponse = response;
                    //data = apiResponse.Data;
                }
                catch (Exception e)
                {
                    throw e;
                }
                Console.WriteLine("TEST FROM LOADER - Summary");
                Console.WriteLine(apiResponse);


                return apiResponse;

            }


        public async Task<List<Dictionary<string, object>>> LoadCommissionSummaryAsDict(string option, string date_from, string date_to)
        {

            List<Dictionary<string, object>> apiResponse = new List<Dictionary<string, object>>();
            var getSummary = new ReportsDataCommissionSummary();

            var data = new ReportsDataCommissionSummary();

            AuthApiResponseData myCreds = credentials;

            try
            {
                var response = await reportsClient.GetCommissionSummary_AsDict(option, date_from, date_to);
                Console.WriteLine("TEST FROM LOADER RESPONSE");
                Console.WriteLine(response);
                apiResponse = response;
                //data = apiResponse.Data;
            }
            catch (Exception e)
            {
                throw e;
            }
            Console.WriteLine("TEST FROM LOADER - Summary");
            Console.WriteLine(apiResponse);


            return apiResponse;

        }




        public async Task<List<ReportsDataCommissionHistory>> LoadCommissionHistory(string option, string date_from, string date_to)
            {

                List<ReportsDataCommissionHistory> apiResponse = new List<ReportsDataCommissionHistory>();
                var getSummary = new ReportsDataCommissionHistory();

                var data = new ReportsDataCommissionHistory();

                AuthApiResponseData myCreds = credentials;

                try
                {
                    var response = await reportsClient.GetReportsCommissionHistory(option, date_from, date_to);
                    Console.WriteLine("TEST FROM LOADER RESPONSE - History");
                    Console.WriteLine(response);
                    apiResponse = response;
                    //data = apiResponse.Data;
                }
                catch (Exception e)
                {
                    throw e;
                }
                Console.WriteLine("TEST FROM LOADER");
                Console.WriteLine(apiResponse);
            //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


            return apiResponse;

            }


        // V2 - Dictionary Implementation
        public async Task<List<Dictionary<string,object>>> LoadCommissionHistoryAsDict(string option, string date_from, string date_to)
        {

            List<Dictionary<string, object>> apiResponse = new List<Dictionary<string, object>>();
            var getSummary = new ReportsDataCommissionHistory();

            var data = new ReportsDataCommissionHistory();

            AuthApiResponseData myCreds = credentials;

            try
            {
                var response = await reportsClient.GetReportsCommissionHistory_AsDict(option, date_from, date_to);
                Console.WriteLine("TEST FROM LOADER RESPONSE - History");
                Console.WriteLine(response);
                apiResponse = response;
                //data = apiResponse.Data;
            }
            catch (Exception e)
            {
                throw e;
            }
            Console.WriteLine("TEST FROM LOADER");
            Console.WriteLine(apiResponse);
            //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


            return apiResponse;

        }

        public async Task<List<Dictionary<string, object>>> LoadReferralCommissionAsDict(string option, string date_from, string date_to)
        {

            List<Dictionary<string, object>> apiResponse = new List<Dictionary<string, object>>();
            
            AuthApiResponseData myCreds = credentials;

            try
            {
                var response = await reportsClient.GetReferralCommissionAsDict(option, date_from, date_to);
                Console.WriteLine("TEST FROM LOADER RESPONSE - History");
                Console.WriteLine(response);
                apiResponse = response;
                //data = apiResponse.Data;
            }
            catch (Exception e)
            {
                throw e;
            }
            Console.WriteLine("TEST FROM LOADER");
            Console.WriteLine(apiResponse);
            //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


            return apiResponse;

        }

        public async Task<List<ReportsDataDSCCommission>> LoadReferralCommission(string option, string date_from, string date_to)
        {

            List<ReportsDataDSCCommission> apiResponse = new List<ReportsDataDSCCommission>();

            AuthApiResponseData myCreds = credentials;

            try
            {
                var response = await reportsClient.GetReferralCommission(option, date_from, date_to);
                Console.WriteLine("TEST FROM LOADER RESPONSE - History");
                Console.WriteLine(response);
                apiResponse = response;
                //data = apiResponse.Data;
            }
            catch (Exception e)
            {
                throw e;
            }
            Console.WriteLine("TEST FROM LOADER");
            Console.WriteLine(apiResponse);
            //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


            return apiResponse;

        }
        public async Task<ApiResponseModel<List<ReportsDataUnilevelCommission>>> LoadUnilevelDataCommission(string date_from)
        {
            try
            {
                var leadershipResponse = await reportsClient.GetUnilevelSales(date_from);
                return leadershipResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<ReportsDataUnilevelCommission>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        
        public async Task<ApiResponseModel<List<ReportsCommHistory>>> LoadRangedCommissionHistory(string option,string date_from, string date_to)
        {
            try
            {
                var historyResponse = await reportsClient.GetSummaryBinaryReports("0",date_from, date_to);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<ReportsCommHistory>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<ReportsCheckMatched>>> LoadCheckMatched(string date_from, string date_to)
        {
            try
            {
                var historyResponse = await reportsClient.CheckMatch(date_from,date_to);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<ReportsCheckMatched>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }



        public async Task<ApiResponseModel<List<rqvSummary>>> LoadRQVSummary(string date_from, string date_to)
        {
            try
            {
                var rqvResponse = await reportsClient.GetRQVSummary(date_from,date_to);
                return rqvResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<rqvSummary>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<infinityHistory>>> LoadInfinityHistory(string date_from, string date_to)
        {
            try
            {
                var infinityHistory = await reportsClient.GetInfinityHistory(date_from, date_to);
                return infinityHistory;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<infinityHistory>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<moverSummary>>> LoadMoverSummary()
        {
            try
            {
                var moverResponse = await reportsClient.GetMoverSummary();
                return moverResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<moverSummary>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<TotalUnicomModel>>> LoadTotalUnilevelCom()
        {

            var apiResponse = new ApiResponseModel<List<TotalUnicomModel>>();

            AuthApiResponseData myCreds = credentials;

            try
            {
                var response = await reportsClient.GetTotalUnicom();

                if (response.IsSuccess)
                {
                    Console.WriteLine($"TEST FROM LOADER RESPONSE - Total Unicom {response.Description}");
                    //Console.WriteLine(response);
                    apiResponse = response;
                }
                else
                {
                    Console.WriteLine($"TEST FROM LOADER RESPONSE - {response.Description}");
                    throw new Exception(response.Description);
                }
                
                //data = apiResponse.Data;
            }
            catch (Exception e)
            {
                //return new ApiResponseModel<List<TotalUnicomModel>> { IsSuccess = false, StatusCode = 500, Description = $"Failed Getting Total Unicom Data of user {e.Message}", Data = new List<TotalUnicomModel>()};
                throw new Exception(e.Message);
            }

            Console.WriteLine("TEST FROM Total Unicom LOADER");
            Console.WriteLine(apiResponse);
            //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


            return apiResponse;

        }
        public async Task<ApiResponseModel<List<ReportsDataUnilevelLeadershipCommission>>> LoadLoadLeaderShipUni(string date_from)
        {
            try
            {
                var leadershipResponse = await reportsClient.GetUnilevelSalesLeadership(date_from);
                return leadershipResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<ReportsDataUnilevelLeadershipCommission>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<ResidualSalesMatch>>> LoadRSM(string date_from, string date_to)
        {
            try
            {
                var historyResponse = await reportsClient.GetResidualSalesMatch(date_from, date_to);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<ResidualSalesMatch>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<ReportsDataPairsHistory>>> LoadPairsHistory(string date_from, string date_to)
        {

            var apiResponse = new ApiResponseModel<List<ReportsDataPairsHistory>>();

            AuthApiResponseData myCreds = credentials;

            try
            {
                var response = await reportsClient.GetPairsHistory(date_from, date_to);
                if (response.IsSuccess)
                {

                }
                Console.WriteLine("TEST FROM LOADER RESPONSE - History");
                Console.WriteLine(response);
                apiResponse = response;
                //data = apiResponse.Data;
            }
            catch (Exception e)
            {
                throw e;
            }
            Console.WriteLine("TEST FROM Pair LOADER");
            Console.WriteLine(apiResponse);
            //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


            return apiResponse;

        }

        public async Task<ApiResponseModel<ReportsDataPairsHistory>> LoadLatestPair(string date_from, string date_to)
        {

            var apiResponse = new ApiResponseModel<ReportsDataPairsHistory>();

            AuthApiResponseData myCreds = credentials;

            try
            {
                var response = await reportsClient.GetLatestPairHistory(date_from, date_to);
                if (response.IsSuccess)
                {

                }
                Console.WriteLine("TEST FROM LOADER RESPONSE - History");
                Console.WriteLine(response);
                apiResponse = response;
                //data = apiResponse.Data;
            }
            catch (Exception e)
            {
                throw e;
            }
            Console.WriteLine("TEST FROM Pair LOADER");
            Console.WriteLine(apiResponse);
            //var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


            return apiResponse;

        }

        public async Task<ApiResponseModel<CommissionCtrModel>> LoadCtr()
        {
            try
            {
                var accountData = await reportsClient.GetCommCtr();
                return accountData;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<CommissionCtrModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }


           
        }

        public async Task<ApiResponseModel<SystemAccessModel>> LoadAccess()
        {
            try
            {
                var accountData = await reportsClient.GetSystemAccess();
                return accountData;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<SystemAccessModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }



        }
        public async Task<ApiResponseModel<accountRank>> LoadRanks()
        {
            try
            {
                var accountData = await reportsClient.GetRank();
                return accountData;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<accountRank>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

    }
    //test coverage


}
