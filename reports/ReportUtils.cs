using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IAM_Library;
using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using IAM_Library.models.reports;
using Newtonsoft.Json;

namespace IAM_Library.reports
{
   

    public class ReportUtils
    {
        IDictionary<DateOnly, DateOnly> coverageDict = new Dictionary<DateOnly, DateOnly>();
        /*
         * public async Task<IDictionary> ReportUtilsLoader(HttpClient _httpClient, AuthApiResponseData credentials)
        {
            var apiBaseUrl = $"{Encryption.decodeString(_constants.authBaseUrl)}";
        }
        */


        
        public async Task<ReportsDataCommissionCoverage> GetCoverageDates(HttpClient _httpClient, String apiUrl)
        {
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
            var responseData = "";

            try
            {

                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();

                    var apiResponseData = JsonConvert.DeserializeObject<ReportsDataCommissionCoverage>(responseData);

                    return apiResponseData;

                }
                else
                {
                    //Console.WriteLine(new ApiResponseData { accountKey = "Not Found", signature = "invalid user" });
                    //responseData = await response.Content.ReadAsStringAsync();

                    //var apiResponseData = JsonConvert.DeserializeObject<ReportsDataCommissionCoverage>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - name is: {apiResponseData.primaryInfo.fname}");
                    return _defaults.invalidDateCoverageList; //create default invalid model


                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during retrieval of data:  {ex.Message}");
                //return _constants.invalidAccountDetailDataResponse;
                return _defaults.invalidDateCoverageList;
            }
        }
    }

    
}
