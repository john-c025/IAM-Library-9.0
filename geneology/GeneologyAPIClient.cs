using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using IAM_Library.models.geneology;
using IAM_Library.models.general;
using IAM_Library.models.reports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.geneology
{
    public class GeneologyAPIClient
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

            {"GenealogyEndpointBase",Encryption.decodeString(_constants.geneologyBase) },
            { "ReportsCAccKey", Encryption.decodeString(_constants.reportsCSummaryAccKey) },
            {"GenealogyEndpointSummary",Encryption.decodeString(_constants.geneologySummary) }
         
        };

        public GeneologyAPIClient(string apiBaseUrl, AuthApiResponseData accessCredentials, HttpClient httpClient) // constructor
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
        public async Task<GeneologyMainModel> LoadGeneologyDown(string accountKey)
        {
            var responseData = "";
            Console.WriteLine($"from Report client to building url base url {_apiConfig.BaseUrl}");
            string api_geonology_endpoint_url = custom.BuildUrl(_apiBaseUrl, "GenealogyEndpointBase", _apiConfig, (_apiConfig.Endpoints["ReportsCAccKey"], accountKey));

            Console.WriteLine($"Built Url is {api_geonology_endpoint_url}");
            try
            {
                var response = await _httpClient.GetAsync(api_geonology_endpoint_url);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();


                    var geonologyData = JsonConvert.DeserializeObject<GeneologyMainModel>(responseData);
                    Console.WriteLine("Genealogy : " + responseData);
                    return geonologyData;
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
        public async Task<GeneologyMainModel> LoadGeneology()
        {
            var responseData = "";
            Console.WriteLine($"from Report client to building url base url {_apiConfig.BaseUrl}");
            string api_geonology_endpoint_url = custom.BuildUrl(_apiBaseUrl, "GenealogyEndpointBase", _apiConfig, (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey));

            Console.WriteLine($"Built Url is {api_geonology_endpoint_url}");
            try
            {
                var response = await _httpClient.GetAsync(api_geonology_endpoint_url);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();


                    var geonologyData = JsonConvert.DeserializeObject<GeneologyMainModel>(responseData);
                    Console.WriteLine("Genealogy : " + responseData);
                    return geonologyData;
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

        public async Task<ApiResponseModel<List<GenealogySummary>>> GetGenealogyHistory()
        {
            var responseData = "";
            var genealogyHistoryList = new List<GenealogySummary>();

            string api_dsc_endpoint_url = custom.BuildUrl(
                _apiBaseUrl,
                "GenealogyEndpointSummary",
                _apiConfig,
                (_apiConfig.Endpoints["ReportsCAccKey"], _accountKey)// Add parameter to fetch the latest record
            );

            Console.WriteLine($"Test URI Built for Genealogy History is : {api_dsc_endpoint_url}");

            try
            {
                var genHResponse = await _httpClient.GetAsync(api_dsc_endpoint_url);

                if (genHResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        responseData = await genHResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Data Fetch from Pairs History Commission Function : " + responseData);

                        
                        try
                        {
                            genealogyHistoryList = JsonConvert.DeserializeObject<List<GenealogySummary>>(responseData);

                            return new ApiResponseModel<List<GenealogySummary>> { IsSuccess = true, Data = genealogyHistoryList, Description = "Success in fetching data!", StatusCode = Convert.ToInt32(genHResponse.StatusCode) };


                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            throw new HttpRequestException("Could not parse response data.", ex);
                        }

                        return new ApiResponseModel<List<GenealogySummary>>();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        throw new HttpRequestException("Error encountered while processing the response data.", ex);
                    }
                }
                else
                {
                    string errorContent = await genHResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: Received non-success status code {genHResponse.StatusCode}");
                    Console.WriteLine($"Error Content: {errorContent}");
                    throw new HttpRequestException($"Non-success status code received: {genHResponse.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                return null;
            }
        }
    }
}
