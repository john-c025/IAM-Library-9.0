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
using IAM_Library.appWallet.api;
using IAM_Library.appWallet.models.dashboard;
using System.Net;
using IAM_Library.models.general;
using IAM_Library.models.wallet;
using IAM_Library.appWallet.models;
using IAM_Library.models.registration;
using System.IO;

namespace IAM_Library.appWallet.account
{
    public class WalletAccountsApiClient //inherit from MainApiClient Class
    {

        private readonly string _apiBaseUrl;
        private readonly string _apiEndpoint;
        private readonly string _accessToken;
        public HttpClient _httpClient;

        public WalletAccountsApiClient(string apiBaseUrl, WalletAuthResponseData auth, HttpClient httpClient) // constructor
        {
            //_httpClient = httpClient;
            _accessToken = auth.signature;
            _apiBaseUrl = apiBaseUrl;
            _apiEndpoint = Encryption.decodeString(_wallet_endpoints.walletGetAccountDetailsFull);
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader));
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

        }
        //
        public async Task<WalletAccountDetailData> GetAccountDataAsync(string accountKey)
        {
            string apiKey = Encryption.decodeString(_wallet_endpoints.api_key);
            _httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Add("ApiKey",apiKey);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            var responseData = "";
            var _apiEndpoint = Encryption.decodeString(_wallet_endpoints.walletGetAccountDetailsFull);

            try
            {

                var apiUrl = _apiBaseUrl + _apiEndpoint + accountKey;
                Console.WriteLine($" API Endpoint DASHBOARD is {apiUrl}");

                // here's the problem
                var response = await _httpClient.GetAsync(apiUrl);

                Console.WriteLine($" API Endpoint is {apiUrl}");
                Console.WriteLine($"Response from API Client after Account Detail Retrieval : Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();

                    var apiResponseData = JsonConvert.DeserializeObject<WalletAccountDetailData>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - Signature is: {apiResponseData.primaryInfo.primaryID}");
                    return apiResponseData;

                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    var apiResponseData = JsonConvert.DeserializeObject<WalletAccountDetailData>(responseData);
                    return apiResponseData;

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during retrieval of data:  {ex.Message}");
                return _defaults_wal.invalidWalletAccount;
                // return new ApiResponseData { accountKey = "Not Found", signature = $"invalid_user {ex.Message}" };
            }
        }



        // WALLET _-------_

        public async Task<WalletBalanceModel> LoadAccountBalance(string accountId)
        {

            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            try
            {
                var endpoint = Encryption.decodeString(_wallet_endpoints.getWalletAccountBalanceEndpoint);
                var apiUrl = _apiBaseUrl + endpoint + accountId;

                // here's the problem
                var response = await _httpClient.GetAsync(apiUrl);

                Console.WriteLine($" API Endpoint is {apiUrl}");
                Console.WriteLine($"Response from API Client after Balance : Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();

                    var apiResponseData = JsonConvert.DeserializeObject<WalletBalanceModel>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - Signature is: {apiResponseData.primaryInfo.primaryID}");
                    return apiResponseData;

                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    var apiResponseData = JsonConvert.DeserializeObject<WalletBalanceModel>(responseData);
                    return apiResponseData;

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during retrieval of data:  {ex.Message}");
                return _defaults_wal.invalidBalance;
                // return new ApiResponseData { accountKey = "Not Found", signature = $"invalid_user {ex.Message}" };
            }
        }




        public async Task<ApiResponseModel<List<WalletTransactionsModel>>> LoadWalletTransactionsList(string accountId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            Console.WriteLine("Loading wallet list");
            var endpoint = Encryption.decodeString(_wallet_endpoints.getWalletTransactionListEndpoint);
            var apiUrl = _apiBaseUrl + endpoint + accountId;


            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Wallet Transactions List : {apiUrl}");
                var GET_History_transaction_list_response = await _httpClient.GetAsync(apiUrl);
                if (GET_History_transaction_list_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    var historyListRepsonse = JsonConvert.DeserializeObject<List<WalletTransactionsModel>>(responseData);
                    Console.WriteLine("[DEBUG] History transaction Content : " + responseData);
                    Console.WriteLine($"DEBUG] Response count is {historyListRepsonse.Count}");
                    return new ApiResponseModel<List<WalletTransactionsModel>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Fetched List of Wallet Transactions!",
                        Data = historyListRepsonse
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_History_transaction_list_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<List<WalletTransactionsModel>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Transaction List! {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<WalletTransactionsModel>>> LoadCashoutWalletTransactionsList(string accountId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            Console.WriteLine("Loading wallet list");
            var endpoint = Encryption.decodeString(_wallet_endpoints.getWalletTransactionListEndpoint);
            var apiUrl = _apiBaseUrl + endpoint + accountId;


            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Wallet Transactions List : {apiUrl}");
                var GET_History_transaction_list_response = await _httpClient.GetAsync(apiUrl);
                if (GET_History_transaction_list_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    var historyListRepsonse = JsonConvert.DeserializeObject<List<WalletTransactionsModel>>(responseData);
                    Console.WriteLine("[DEBUG] History transaction Content : " + responseData);

                    return new ApiResponseModel<List<WalletTransactionsModel>>
                    {
                        IsSuccess = false,
                        StatusCode = 200,
                        Description = "Could not fetch List of Wallet Transactions!",
                        Data = historyListRepsonse
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_History_transaction_list_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<List<WalletTransactionsModel>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Transaction List! {e.Message}",
                    Data = null
                };
            }
        }



        // POST Cashout

        public async Task<ApiResponseModel<MainCashoutResponseModel>> POSTCashoutTransaction(POSTCashoutModel postData)
        {
            var responseData = "";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);


            string api_cashout_url = _apiBaseUrl + Encryption.decodeString(_wallet_endpoints.postCashoutEndpoint);
            Console.WriteLine($"[DEBUG] Built Url for Cashout is {api_cashout_url} ---- > TESTING AUTH FOR CASHOUT");
            Console.WriteLine($"[DEBUG] Token in Code: {_httpClient.DefaultRequestHeaders.Authorization}");
            try
            {
                var jsonWithdrawData = JsonConvert.SerializeObject(postData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonWithdrawData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data for cashout: {jsonWithdrawData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_cashout_url, byteContent);


                    Console.WriteLine($"[DEBUG] Response Status Code for Cashout: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<MainCashoutResponseModel>(responseData);
                        Console.WriteLine("[DEBUG] Cashout Response : " + responseData);

                        return new ApiResponseModel<MainCashoutResponseModel>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = deserializedResponse?.message ?? "Cashout Successful!",
                            Data = deserializedResponse
                        };
                    }
                    else
                    {
                        string errorContent = await responseClient.Content.ReadAsStringAsync();
                        string errorMessage;

                        try
                        {
                            // Try to deserialize as MainCashoutResponseModel first to get the message
                            var cashoutErrorResponse = JsonConvert.DeserializeObject<MainCashoutResponseModel>(errorContent);
                            if (cashoutErrorResponse != null && !string.IsNullOrEmpty(cashoutErrorResponse.message))
                            {
                                errorMessage = cashoutErrorResponse.message;
                            }
                            else
                            {
                                // Fall back to ErrorModel
                                var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                                errorMessage = errorJson?.Message ?? $"An error occurred.";
                            }
                        }
                        catch (JsonException)
                        {
                            // If deserialization fails, try ErrorModel
                            try
                            {
                                var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                                errorMessage = errorJson?.Message ?? errorContent;
                            }
                            catch
                            {
                                errorMessage = errorContent;
                            }
                            Console.WriteLine($"Exception Error on Parsing Error Content - {errorMessage}");
                        }

                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<MainCashoutResponseModel>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = errorMessage,
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Cashout API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on EXCEPTION WITHDRAWAL");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<MainCashoutResponseModel>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Cashout: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<MainCashoutResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }

        // Generate 2FA

        public async Task<ApiResponseModel<Generated2FAResponseModel>> Generate2FA(POST2FAModel postData)
        {
            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            string api_cashout_url = _apiBaseUrl + Encryption.decodeString(_wallet_endpoints.generate2FA);
            Console.WriteLine($"[DEBUG] Built Url for 2FA is {api_cashout_url}");

            try
            {
                var jsonWithdrawData = JsonConvert.SerializeObject(postData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonWithdrawData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data for 2FA: {jsonWithdrawData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_cashout_url, byteContent);


                    Console.WriteLine($"[DEBUG] Response Status Code: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<Generated2FAResponseModel>(responseData);
                        Console.WriteLine("[DEBUG] 2FA Response : " + responseData);

                        return new ApiResponseModel<Generated2FAResponseModel>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "2FA Sent!",
                            Data = deserializedResponse
                        };
                    }
                    else
                    {
                        string errorContent = await responseClient.Content.ReadAsStringAsync();
                        string errorMessage;

                        try
                        {
                            var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                            errorMessage = errorJson?.Message ?? $"An error occurred. {errorJson.Message} ";
                        }
                        catch (JsonException)
                        {
                            errorMessage = errorContent;
                            Console.WriteLine($"Exception Error on Parsing Error Content - {errorMessage}");
                        }

                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<Generated2FAResponseModel>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"2FA API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on EXCEPTION 2FA");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<Generated2FAResponseModel>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Cashout: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<Generated2FAResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<LogoutPOSTResponse>> LogoutUser(LogoutPOSTModel postData)
        {
            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();

            var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
            var endpoint = Encryption.decodeString(_wallet_endpoints.logoutNew);
            string api_cashout_url = baseUrl + endpoint;
            Console.WriteLine($"[DEBUG] Built Url for Logout is {api_cashout_url}");

            try
            {
                var jsonWithdrawData = JsonConvert.SerializeObject(postData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonWithdrawData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data for 2FA: {jsonWithdrawData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_cashout_url, byteContent);


                    Console.WriteLine($"[DEBUG] Response Status Code: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<LogoutPOSTResponse>(responseData);
                        Console.WriteLine("[DEBUG] 2FA Response : " + responseData);

                        return new ApiResponseModel<LogoutPOSTResponse>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Logged Out!",
                            Data = deserializedResponse
                        };
                    }
                    else
                    {
                        string errorContent = await responseClient.Content.ReadAsStringAsync();
                        string errorMessage;

                        try
                        {
                            var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                            errorMessage = errorJson?.Message ?? $"An error occurred. {errorJson.Message} ";
                        }
                        catch (JsonException)
                        {
                            errorMessage = errorContent;
                            Console.WriteLine($"Exception Error on Parsing Error Content - {errorMessage}");
                        }

                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<LogoutPOSTResponse>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Logout API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on EXCEPTION Logout");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<LogoutPOSTResponse>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Logout: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<LogoutPOSTResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }


        // Verify 2fa
        public async Task<ApiResponseModel<Generated2FAResponseModel>> Verify2FA(POSTVerify2FAModel postData)
        {
            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);


            var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
            var endpoint = Encryption.decodeString(_wallet_endpoints.verify2FANew);
            string api_cashout_url = baseUrl + endpoint; //Encryption.decodeString(_wallet_endpoints.verify2FA); 
            Console.WriteLine($"[DEBUG] Built Url for 2FA is {api_cashout_url}");

            try
            {
                var jsonWithdrawData = JsonConvert.SerializeObject(postData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonWithdrawData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data for 2FA Verify: {jsonWithdrawData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_cashout_url, byteContent);


                    Console.WriteLine($"[DEBUG] Response Status Code: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<Generated2FAResponseModel>(responseData);
                        Console.WriteLine("[DEBUG] 2FA Response : " + responseData);

                        return new ApiResponseModel<Generated2FAResponseModel>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "2FA Sent!",
                            Data = deserializedResponse
                        };
                    }
                    else
                    {
                        string errorContent = await responseClient.Content.ReadAsStringAsync();
                        string errorMessage;

                        try
                        {
                            var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                            errorMessage = errorJson?.Message ?? $"An error occurred. {errorJson.Message} ";
                        }
                        catch (JsonException)
                        {
                            errorMessage = errorContent;
                            Console.WriteLine($"Exception Error on Parsing Error Content - {errorMessage}");
                        }

                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<Generated2FAResponseModel>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"2FA API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on EXCEPTION 2FA Verify");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<Generated2FAResponseModel>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Cashout: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<Generated2FAResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }


        // POST Send Load
        public async Task<ApiResponseModel<MainLoadResponse>> POSTSendLoad(SendLoadPOSTModel postData)
        {
            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            string api_cashout_url = _apiBaseUrl + Encryption.decodeString(_wallet_endpoints.postSendload);
            Console.WriteLine($"[DEBUG] Built Url for Loading is {api_cashout_url}");

            try
            {
                var jsonWithdrawData = JsonConvert.SerializeObject(postData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonWithdrawData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data for Loading: {jsonWithdrawData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_cashout_url, byteContent);


                    Console.WriteLine($"[DEBUG] Response Status Code: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<MainLoadResponse>(responseData);
                        Console.WriteLine("[DEBUG] Loading Response : " + responseData);

                        return new ApiResponseModel<MainLoadResponse>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Loading Successful!",
                            Data = deserializedResponse
                        };
                    }
                    else
                    {
                        string errorContent = await responseClient.Content.ReadAsStringAsync();
                        string errorMessage;

                        try
                        {
                            var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                            errorMessage = errorJson?.Message ?? $"An error occurred. {errorJson.Message} ";
                        }
                        catch (JsonException)
                        {
                            errorMessage = errorContent;
                            Console.WriteLine($"Exception Error on Parsing Error Content - {errorMessage}");
                        }

                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<MainLoadResponse>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Loading API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on EXCEPTION Loading");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<MainLoadResponse>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Loading: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<MainLoadResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }



        // Generate Transaction Reference Number
        public async Task<GeneratedReferenceModel> GenerateTransactionReferenceNumber(string tranType)
        {

            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

            try
            {
                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var apiEndpointWithParam = Encryption.decodeString(_wallet_endpoints.getGeneratedReferenceNew);


                var endpoint = Encryption.decodeString(_wallet_endpoints.getGeneratedReferenceNumber);
                var apiUrl = baseUrl + apiEndpointWithParam + tranType; //endpoint + tranType; //baseUrl+apiEndpointWithParam+tranType;
                Console.WriteLine($" API Endpoint for generation is {apiUrl}");
                // here's the problem
                var response = await _httpClient.GetAsync(apiUrl);


                Console.WriteLine($"Response from API Client after Generation : Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Code from generation {responseData}");
                    var apiResponseData = JsonConvert.DeserializeObject<GeneratedReferenceModel>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - Signature is: {apiResponseData.primaryInfo.primaryID}");
                    return apiResponseData;

                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    var apiResponseData = JsonConvert.DeserializeObject<GeneratedReferenceModel>(responseData);
                    return apiResponseData;

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during generation of reference number:  {ex.Message}");
                return new GeneratedReferenceModel { autoNum = "INVALID" };
                // return new ApiResponseData { accountKey = "Not Found", signature = $"invalid_user {ex.Message}" };
            }
        }


        // GET Bank Cashout List

        public async Task<ApiResponseModel<List<BankCashoutList>>> LoadBankCashoutList()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
            Console.WriteLine($"[DEBUG] from API Bank list to building url base url {_apiBaseUrl}");
            string endpoint = Encryption.decodeString(_wallet_endpoints.getCashoutBankListEndpoint);
            string api_url = _apiBaseUrl + endpoint;

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Bank List : {api_url}");
                var GET_History_transaction_list_response = await _httpClient.GetAsync(api_url);
                if (GET_History_transaction_list_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    var historyListRepsonse = JsonConvert.DeserializeObject<List<BankCashoutList>>(responseData);
                    Console.WriteLine("[DEBUG] Bank API Content : " + responseData);

                    return new ApiResponseModel<List<BankCashoutList>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "List of Wallet Bank List Fetched!",
                        Data = historyListRepsonse
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_History_transaction_list_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<List<BankCashoutList>>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Bank List! {e.Message}",
                    Data = null
                };
            }
        }

        // Load Networks


        public async Task<ApiResponseModel<List<TelcoModel>>> LoadNetworks()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
            Console.WriteLine($"[DEBUG] from API Bank list to building url base url {_apiBaseUrl}");

            string baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
            string endpointUrl = Encryption.decodeString(_wallet_endpoints.getTelcoListNew);
            //old V
            string api_url = baseUrl + endpointUrl;// Encryption.decodeString(_wallet_endpoints.getEloadTelcoList); // baseUrl+endpointUrl;

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Telco List : {api_url}");
                var GET_History_transaction_list_response = await _httpClient.GetAsync(api_url);
                if (GET_History_transaction_list_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    var historyListRepsonse = JsonConvert.DeserializeObject<List<TelcoModel>>(responseData);
                    Console.WriteLine("[DEBUG] Bank API Content : " + responseData);

                    return new ApiResponseModel<List<TelcoModel>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "List of Wallet Telco List Fetched!",
                        Data = historyListRepsonse
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_History_transaction_list_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<List<TelcoModel>>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Telco List! {e.Message}",
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<List<TelcoProduct>>> LoadPromoByNetwork(string telco)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);


            string baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
            string endpoint = Encryption.decodeString(_wallet_endpoints.getLoadPromosByNameNew);

            string api_url = baseUrl + endpoint + telco;//Encryption.decodeString(_wallet_endpoints.getEloadPromosByTelcoName) + telco; // baseUrl + endpoint + telco;

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Promo List : {api_url}");
                var GET_History_transaction_list_response = await _httpClient.GetAsync(api_url);
                if (GET_History_transaction_list_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    var historyListRepsonse = JsonConvert.DeserializeObject<List<TelcoProduct>>(responseData);
                    Console.WriteLine("[DEBUG] Load Promo API Content : " + responseData);

                    return new ApiResponseModel<List<TelcoProduct>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "List of Promo List Fetched!",
                        Data = historyListRepsonse
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_History_transaction_list_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<List<TelcoProduct>>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Promo List! {e.Message}",
                    Data = null
                };
            }
        }


        /*
        public async Task<ApiResponseModel<List<CashinProcedure>>> LoadCashinProcedures(string accountId,string code)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);


            string api_url = Encryption.decodeString(_wallet_endpoints.loadCashinEndpoint) + code + Encryption.decodeString(_wallet_endpoints.loadCashParam) +accountId;

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Cashin List : {api_url}");
                var GET_History_transaction_list_response = await _httpClient.GetAsync(api_url);
                Console.WriteLine($"[Debug] Response from cashin list is {GET_History_transaction_list_response}");
                if (GET_History_transaction_list_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    var historyListRepsonse = JsonConvert.DeserializeObject<List<CashinProcedure>>(responseData);
                    Console.WriteLine("[DEBUG] Load Cashin API Content : " + responseData);
                    Console.WriteLine("[DEBUG] Load Cashin API Content  -- 2: " + historyListRepsonse);
                    return new ApiResponseModel<List<CashinProcedure>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "List of Cashin List Fetched!",
                        Data = historyListRepsonse
                    };
                }
                else
                {
                    Console.WriteLine(GET_History_transaction_list_response.Content.ToString());
                    throw new HttpRequestException(message: GET_History_transaction_list_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<List<CashinProcedure>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Promo List! {e.Message}",
                    Data = null
                };
            }
        }

        */

        public async Task<ApiResponseModel<List<CashinProcedure>>> LoadCashinProcedures(string accountId, string code)
        {
            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(code))
            {
                return new ApiResponseModel<List<CashinProcedure>>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Description = "Invalid parameters: Account ID or Code is null/empty.",
                    Data = null
                };
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
                var endpoint = Encryption.decodeString(_wallet_endpoints.loadCashinListNew);

                string api_url = baseUrl + endpoint + code + Encryption.decodeString(_wallet_endpoints.loadCashParam) + accountId;
                //$"{Encryption.decodeString(_wallet_endpoints.loadCashinEndpoint)}{code}" +
                //$"{Encryption.decodeString(_wallet_endpoints.loadCashParam)}{accountId}"; //baseUrl+endpoint+code+{Encryption.decodeString(_wallet_endpoints.loadCashParam)}{accountId}

                Console.WriteLine($"[DEBUG] Fetching Cashin Procedures: {api_url}");
                var response = await _httpClient.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var procedures = JsonConvert.DeserializeObject<List<CashinProcedure>>(content);

                if (procedures == null || !procedures.Any())
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<List<CashinProcedure>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "Cashin procedures fetched successfully.",
                    Data = procedures
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in LoadCashinProcedures: {e.Message}");
                return new ApiResponseModel<List<CashinProcedure>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }








        // IAM INstapay
        public async Task<InstapayBalanceModel> LoadInstapayBalance()
        {

            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", Encryption.decodeString(_wallet_endpoints.api_key));

            try
            {
                var endpoint = Encryption.decodeString(_wallet_endpoints.getIamInstapayBalance);
                var apiUrl = _apiBaseUrl + endpoint;
                // _httpClient.DefaultRequestHeaders.Add("ApiKey", Encryption.decodeString(_wallet_endpoints.api_key));
                // here's the problem
                var response = await _httpClient.GetAsync(apiUrl);

                Console.WriteLine($" API Endpoint is {apiUrl}");
                Console.WriteLine($"Response from API Client after Account Detail Retrieval : Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();

                    var apiResponseData = JsonConvert.DeserializeObject<InstapayBalanceModel>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - Signature is: {apiResponseData.primaryInfo.primaryID}");
                    return apiResponseData;

                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    var apiResponseData = JsonConvert.DeserializeObject<InstapayBalanceModel>(responseData);
                    return apiResponseData;

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during retrieval of data:  {ex.Message}");
                return _defaults_wal.invalidInstapay;
                // return new ApiResponseData { accountKey = "Not Found", signature = $"invalid_user {ex.Message}" };
            }
        }




        // Generate Transaction Reference Number
        public async Task<GetCashoutFeeModel> GetCashoutFee()
        {

            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            try
            {
                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
                var endpointNew = Encryption.decodeString(_wallet_endpoints.COProcessingFeeNew);

                var endpoint = baseUrl + endpointNew;
                var apiUrl = endpoint;
                Console.WriteLine($" API Endpoint for cashout fee is {apiUrl}");
                // here's the problem
                var response = await _httpClient.GetAsync(apiUrl);


                Console.WriteLine($"Response from API Client after Generation of otp: Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Code from generation {responseData}");
                    var apiResponseData = JsonConvert.DeserializeObject<GetCashoutFeeModel>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - Signature is: {apiResponseData.primaryInfo.primaryID}");
                    return apiResponseData;

                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    var apiResponseData = JsonConvert.DeserializeObject<GetCashoutFeeModel>(responseData);
                    return apiResponseData;

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during generation of reference number:  {ex.Message}");
                return new GetCashoutFeeModel { tranDesc = "INVALID", amount = 0.00 };
                // return new ApiResponseData { accountKey = "Not Found", signature = $"invalid_user {ex.Message}" };
            }
        }



        // Get cashin 
        public async Task<GetCashinFeeModel> GetCashinFee()
        {

            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Add("ApiKey",apiKey);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            try
            {
                var baseUrl = Encryption.decodeString(_wallet_endpoints.live);
                var endpointNew = Encryption.decodeString(_wallet_endpoints.CIProcessingFeeNew);
                var endpoint = Encryption.decodeString(_wallet_endpoints.CIProcessingFee);
                var apiUrl = endpoint;
                Console.WriteLine($" API Endpoint for cashin fee is {apiUrl}");
                // here's the problem
                var response = await _httpClient.GetAsync(apiUrl);


                Console.WriteLine($"Response from API Client after Generation : Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Code from generation {responseData}");
                    var apiResponseData = JsonConvert.DeserializeObject<GetCashinFeeModel>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - Signature is: {apiResponseData.primaryInfo.primaryID}");
                    return apiResponseData;

                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    var apiResponseData = JsonConvert.DeserializeObject<GetCashinFeeModel>(responseData);
                    return apiResponseData;

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during generation of reference number:  {ex.Message}");
                return new GetCashinFeeModel { tranDesc = "INVALID", amount = 0.00 };
                // return new ApiResponseData { accountKey = "Not Found", signature = $"invalid_user {ex.Message}" };
            }
        }






        // Update Contact Details
        // WALA din to sa live hahah

        public async Task<ApiResponseModel<MainUpdateContactResponseModel>> POSTUpdateContactDetails(MainContactInfo postData)
        {
            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            string api_update_contact_url = _apiBaseUrl + Encryption.decodeString(_wallet_endpoints.updateContactDetails);
            Console.WriteLine($"[DEBUG] Built Url for Update Contact Details is {api_update_contact_url}");

            try
            {
                var jsonContactData = JsonConvert.SerializeObject(postData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonContactData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data for update contact: {jsonContactData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_update_contact_url, byteContent);

                    Console.WriteLine($"[DEBUG] Response Status Code: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<MainUpdateContactResponseModel>(responseData);
                        Console.WriteLine("[DEBUG] Update Contact Response : " + responseData);

                        return new ApiResponseModel<MainUpdateContactResponseModel>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Contact Details Updated Successfully!",
                            Data = deserializedResponse
                        };
                    }
                    else
                    {
                        string errorContent = await responseClient.Content.ReadAsStringAsync();
                        string errorMessage;

                        try
                        {
                            var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                            errorMessage = errorJson?.Message ?? $"An error occurred. {errorJson.Message} ";
                        }
                        catch (JsonException)
                        {
                            errorMessage = errorContent;
                            Console.WriteLine($"Exception Error on Parsing Error Content - {errorMessage}");
                        }

                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<MainUpdateContactResponseModel>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Update Contact API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on EXCEPTION UPDATE CONTACT");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<MainUpdateContactResponseModel>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Contact Update: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<MainUpdateContactResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<MainUpdateContactResponseModel>> POSTUpdateContactNumber(ContactInfo postData)
        {
            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            string api_update_contact_url = _apiBaseUrl + Encryption.decodeString(_wallet_endpoints.UpdateContactNoApi);
            Console.WriteLine($"[DEBUG] Built Url for Update Contact No Details is {api_update_contact_url}");

            try
            {
                var jsonContactData = JsonConvert.SerializeObject(postData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonContactData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data for update contact: {jsonContactData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_update_contact_url, byteContent);

                    Console.WriteLine($"[DEBUG] Response Status Code: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<MainUpdateContactResponseModel>(responseData);
                        Console.WriteLine("[DEBUG] Update Contact Response : " + responseData);

                        return new ApiResponseModel<MainUpdateContactResponseModel>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Contact Details Updated Successfully!",
                            Data = deserializedResponse
                        };
                    }
                    else
                    {
                        string errorContent = await responseClient.Content.ReadAsStringAsync();
                        string errorMessage;

                        try
                        {
                            var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                            errorMessage = errorJson?.Message ?? $"An error occurred. {errorJson.Message} ";
                        }
                        catch (JsonException)
                        {
                            errorMessage = errorContent;
                            Console.WriteLine($"Exception Error on Parsing Error Content - {errorMessage}");
                        }

                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<MainUpdateContactResponseModel>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Update Contact API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on EXCEPTION UPDATE CONTACT");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<MainUpdateContactResponseModel>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Contact Update: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<MainUpdateContactResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }













        // KYC API Client


        public async Task<ApiResponseModel<List<IdTypeListModel>>> LoadKYCIDList()
        {

            string apiKey = Encryption.decodeString(_wallet_endpoints.api_key);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
                _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var endpoint = Encryption.decodeString(_wallet_endpoints.LoadKYCIDListEndpoint);

                string api_url = $"{baseUrl}" + $"{endpoint}";

                Console.WriteLine($"[DEBUG] Fetching ID Types: {api_url}");
                var response = await _httpClient.GetAsync(api_url);
                Console.WriteLine($"[DEBUG] Fetching ID Types: {response}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var procedures = JsonConvert.DeserializeObject<List<IdTypeListModel>>(content);
                Console.WriteLine($"[DEBUG] Fetching ID Types: {procedures}");
                if (procedures == null || !procedures.Any())
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<List<IdTypeListModel>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "ID Types fetched successfully.",
                    Data = procedures
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in KYC ID LIst: {e.Message}");
                return new ApiResponseModel<List<IdTypeListModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<List<FileTypeListeModel>>> LoadKYCFileTypeList()
        {


            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
                _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var endpoint = Encryption.decodeString(_wallet_endpoints.LoadKYCFileTypesEndpoint);

                string api_url = $"{baseUrl}" +
                                 $"{endpoint}";

                Console.WriteLine($"[DEBUG] Fetching File Types: {api_url}");
                var response = await _httpClient.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var procedures = JsonConvert.DeserializeObject<List<FileTypeListeModel>>(content);

                if (procedures == null || !procedures.Any())
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<List<FileTypeListeModel>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "File Types fetched successfully.",
                    Data = procedures
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in KYC File List Loading: {e.Message}");
                return new ApiResponseModel<List<FileTypeListeModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }
        private string CleanBase64String(string base64String)
        {
            const string base64Prefix = "data:image/jpeg;base64,";

            if (base64String.StartsWith(base64Prefix))
            {
                return base64String.Substring(base64Prefix.Length);
            }

            return base64String; // Return as-is if no prefix is found
        }

        public async Task<ApiResponseModel<KYCUploadResponse>> POSTUploadKYC(KYCUploadModel postData)
        {
            var responseData = "";

            _httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
            _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

            string api_upload_url = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC) + Encryption.decodeString(_wallet_endpoints.UploadKYC);
            try
            {
                using (var multipartContent = new MultipartFormDataContent())
                {
                    // Handle file content
                    if (File.Exists(postData.FilePath))
                    {
                        Console.WriteLine("[DEBUG] Using file from physical path");

                        // Read the file from the given physical path
                        var fileContent = new ByteArrayContent(File.ReadAllBytes(postData.FilePath));
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                        // Get the file name from the path and add to multipart content
                        if (postData.FileTypeID == 0) // Image
                        {
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        }
                        else if (postData.FileTypeID == 1) // Selfie Crap
                        {
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                        }

                        else if (postData.FileTypeID == 2) // Video
                        {
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
                        }

                        var fileName = Path.GetFileName(postData.FilePath);
                        multipartContent.Add(fileContent, "FilePath", fileName);
                    }
                    else if (IsBase64String(postData.FilePath))
                    {
                        Console.WriteLine("[DEBUG] Using base64 string as file content");

                        // Clean the base64 string and convert it to a byte array
                        var cleanedBase64String = CleanBase64String(postData.FilePath);
                        var fileBytes = Convert.FromBase64String(cleanedBase64String);

                        // Create ByteArrayContent from the file bytes
                        var fileContent = new ByteArrayContent(fileBytes);
                        if (postData.FileTypeID == 0) // Image
                        {
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                            multipartContent.Add(fileContent, "FilePath", "image.jpg");
                        }
                        else if (postData.FileTypeID == 1) // Video
                        {
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                            multipartContent.Add(fileContent, "FilePath", "image.jpg");
                        }
                        else if (postData.FileTypeID == 2) // Video
                        {
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
                            multipartContent.Add(fileContent, "FilePath", "video.mp4");
                        }

                        // Add to multipart content with a file name (you can adjust this dynamically)

                    }

                    else
                    {
                        throw new Exception("Invalid file data provided - must be either a valid file path or base64 string");
                    }

                    // Add all required form fields
                    // ... existing code ...

                    // Add all required form fields

                    multipartContent.Add(new StringContent(postData.CardFname ?? ""), "CardFname");
                    multipartContent.Add(new StringContent(postData.CardMname ?? ""), "CardMname");
                    multipartContent.Add(new StringContent(postData.CardSname ?? ""), "CardSname");
                    multipartContent.Add(new StringContent(postData.AccountID ?? ""), "AccountID");
                    multipartContent.Add(new StringContent(postData.KYCID.ToString()), "KYCID");
                    multipartContent.Add(new StringContent(postData.IDCode.ToString()), "IDCode");  // Changed this line
                    multipartContent.Add(new StringContent(postData.FileTypeID.ToString()), "FileTypeID");
                    multipartContent.Add(new StringContent(postData.Status.ToString()), "Status");
                    multipartContent.Add(new StringContent(postData.CardNumber ?? ""), "CardNumber");


                    // Add date fields (assuming they are DateTime, not DateTime?)
                    multipartContent.Add(new StringContent(postData.UpdateDate.ToString("o")), "UpdateDate");
                    multipartContent.Add(new StringContent(postData.SvrDate.ToString("o")), "SvrDate");

                    // Add optional fields
                    if (!string.IsNullOrEmpty(postData.VerifiedBy))
                    {
                        multipartContent.Add(new StringContent(postData.VerifiedBy), "VerifiedBy");
                    }

                    // ... rest of existing code ...

                    Console.WriteLine($"[DEBUG] Uploading KYC document with FileTypeID: {postData.FileTypeID}");
                    Console.WriteLine($"[DEBUG] AccountID: {postData.AccountID}");
                    Console.WriteLine($"[DEBUG] CardNumber: {postData.CardNumber}");

                    try
                    {
                        var responseClient = await _httpClient.PostAsync(api_upload_url, multipartContent);

                        Console.WriteLine($"[DEBUG] Response Status Code for Upload: {responseClient.StatusCode}");

                        if (responseClient.IsSuccessStatusCode)
                        {
                            responseData = await responseClient.Content.ReadAsStringAsync();
                            var deserializedResponse = JsonConvert.DeserializeObject<KYCUploadResponse>(responseData);
                            Console.WriteLine("[DEBUG] Upload Response: " + responseData);

                            return new ApiResponseModel<KYCUploadResponse>
                            {
                                IsSuccess = true,
                                StatusCode = 200,
                                Description = "Upload Successful!",
                                Data = deserializedResponse
                            };
                        }
                        else
                        {
                            string errorContent = await responseClient.Content.ReadAsStringAsync();
                            string errorMessage;

                            try
                            {
                                var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                                errorMessage = errorJson?.Message ?? $"An error occurred. {errorJson?.Message} ";
                            }
                            catch (JsonException)
                            {
                                errorMessage = errorContent;
                                Console.WriteLine($"[ERROR] Exception Error on Parsing Error Content - {errorMessage}");
                            }

                            Console.WriteLine($"[ERROR] Critical Error - {errorMessage}");
                            Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                            return new ApiResponseModel<KYCUploadResponse>
                            {
                                IsSuccess = false,
                                StatusCode = (int)responseClient.StatusCode,
                                Description = $"{errorMessage}",
                                Data = null
                            };
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[ERROR] Response Error on EXCEPTION UPLOAD");
                        Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                        return new ApiResponseModel<KYCUploadResponse>
                        {
                            IsSuccess = false,
                            StatusCode = 500,
                            Description = $"Error Encountered during Upload: {e.Message}",
                            Data = null
                        };
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<KYCUploadResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }

        private bool IsBase64String(string base64)
        {
            if (string.IsNullOrEmpty(base64)) return false;
            try
            {
                var buffer = Convert.FromBase64String(base64);
                return buffer.Length > 0;
            }
            catch
            {
                return false;
            }
        }
        // ... existing code ...


        public async Task<ApiResponseModel<List<FileTypeListeModel>>> LoadKYCLevels()
        {

            string apiKey = Encryption.decodeString(_wallet_endpoints.api_key);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
                //_httpClient.DefaultRequestHeaders.Add("ApiKey", apiKey);
                _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var endpoint = Encryption.decodeString(_wallet_endpoints.LoadKYCFileTypesEndpoint);

                string api_url = $"{baseUrl}" +
                                 $"{endpoint}";

                Console.WriteLine($"[DEBUG] Fetching File Types: {api_url}");
                var response = await _httpClient.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var procedures = JsonConvert.DeserializeObject<List<FileTypeListeModel>>(content);

                if (procedures == null || !procedures.Any())
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<List<FileTypeListeModel>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "File Types fetched successfully.",
                    Data = procedures
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in KYC Levels: {e.Message}");
                return new ApiResponseModel<List<FileTypeListeModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<MainKYCDetailsResponse>> LoadMemberKYCDetails(int kycID, int fileType, string accountId)
        {
            string apiKey = Encryption.decodeString(_wallet_endpoints.api_key);


            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
                //_httpClient.DefaultRequestHeaders.Add("ApiKey", apiKey);
                _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var endpoint_1 = Encryption.decodeString(_wallet_endpoints.LoadKYCDetailsBase);
                var endpoint_2 = Encryption.decodeString(_wallet_endpoints.LoadKYCDetailsKYCParam);
                var endpoint_3 = Encryption.decodeString(_wallet_endpoints.LoadKYCDetailsFileTypParam);
                var endpoint_4 = Encryption.decodeString(_wallet_endpoints.LoadKYCDetailsAccountIDParam);
                string api_url = $"{baseUrl}" +
                                 $"{endpoint_1}" + $"{endpoint_2}" + kycID + "&" + $"{endpoint_3}" + fileType + "&" + $"{endpoint_4}" + accountId;

                Console.WriteLine($"[DEBUG] Fetching KYC Details: {api_url}");
                var response = await _httpClient.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var details = JsonConvert.DeserializeObject<MainKYCDetailsResponse>(content);

                if (details == null)
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<MainKYCDetailsResponse>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "File Types fetched successfully.",
                    Data = details
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in Member KYC Details: {e.Message}");
                return new ApiResponseModel<MainKYCDetailsResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<MainAccountKYCResponse>> LoadMainAccountKYCLevel(string accountId)
        {
            string apiKey = Encryption.decodeString(_wallet_endpoints.api_key);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
                //_httpClient.DefaultRequestHeaders.Add("ApiKey",apiKey);
                _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var endpoint = Encryption.decodeString(_wallet_endpoints.LoadMainKYCLevel);
                string api_url = $"{baseUrl}{endpoint}{accountId}";

                Console.WriteLine($"[DEBUG] Fetching Main KYC Level: {api_url}");
                var response = await _httpClient.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var details = JsonConvert.DeserializeObject<MainAccountKYCResponse>(content);

                if (details == null)
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<MainAccountKYCResponse>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "Main KYC Level fetched successfully.",
                    Data = details
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in LoadMainKYCLevel: {e.Message}");
                return new ApiResponseModel<MainAccountKYCResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// GET /v1/KYC/GetAccountKYCHistory?accountid= — ApiKey header (same as other KYC util calls).
        /// </summary>
        public async Task<ApiResponseModel<List<WalletAccountKYCHistoryItem>>> LoadAccountKYCHistory(string accountId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountId))
                {
                    return new ApiResponseModel<List<WalletAccountKYCHistoryItem>>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Description = "accountId is required.",
                        Data = null
                    };
                }

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var path = _wallet_endpoints.GetAccountKYCHistory;
                string api_url = $"{baseUrl}{path}?accountid={Uri.EscapeDataString(accountId)}";

                Console.WriteLine($"[DEBUG] Fetching KYC Account History: {api_url}");
                var response = await _httpClient.GetAsync(api_url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var wrapper = JsonConvert.DeserializeObject<GetAccountKYCHistoryApiResponse>(content);
                    var list = wrapper?.history ?? new List<WalletAccountKYCHistoryItem>();
                    return new ApiResponseModel<List<WalletAccountKYCHistoryItem>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "KYC history fetched successfully.",
                        Data = list
                    };
                }

                return new ApiResponseModel<List<WalletAccountKYCHistoryItem>>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    Description = content,
                    Data = null
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in LoadAccountKYCHistory: {e.Message}");
                return new ApiResponseModel<List<WalletAccountKYCHistoryItem>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }

        //Bills Payment

        public async Task<ApiResponseModel<BillsPaymentConfigModel>> LoadBillsPaymentSettings(string id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
                var endpoint = Encryption.decodeString(_wallet_endpoints.GetBillPaymentSettings);
                string api_url = $"{baseUrl}{endpoint}/{id}";

                Console.WriteLine($"[DEBUG] Fetching Main KYC Level: {api_url}");
                var response = await _httpClient.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var details = JsonConvert.DeserializeObject<BillsPaymentConfigModel>(content);

                if (details == null)
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<BillsPaymentConfigModel>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "Main KYC Level fetched successfully.",
                    Data = details
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in LoadMainKYCLevel: {e.Message}");
                return new ApiResponseModel<BillsPaymentConfigModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<BillerInfoModel>> LoadBillerInfoByName(string biller_name)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
                var endpoint = Encryption.decodeString(_wallet_endpoints.GetBillsPaymentBillerDetails);
                string api_url = $"{baseUrl}{endpoint}/{biller_name}";

                Console.WriteLine($"[DEBUG] Fetching Main KYC Level: {api_url}");
                var response = await _httpClient.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var details = JsonConvert.DeserializeObject<BillerInfoModel>(content);

                if (details == null)
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<BillerInfoModel>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "Main KYC Level fetched successfully.",
                    Data = details
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in LoadMainKYCLevel: {e.Message}");
                return new ApiResponseModel<BillerInfoModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }



        public async Task<ApiResponseModel<List<BillerInfoModel>>> LoadBillerList()
        {

            string apiKey = Encryption.decodeString(_wallet_endpoints.api_key);

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
                var endpoint = Encryption.decodeString(_wallet_endpoints.GetBillsPaymentBillerList);

                string api_url = $"{baseUrl}" +
                                 $"{endpoint}";

                Console.WriteLine($"[DEBUG] Fetching File Types: {api_url}");
                var response = await _httpClient.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var procedures = JsonConvert.DeserializeObject<List<BillerInfoModel>>(content);

                if (procedures == null || !procedures.Any())
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<List<BillerInfoModel>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "File Types fetched successfully.",
                    Data = procedures
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in Biller List: {e.Message}");
                return new ApiResponseModel<List<BillerInfoModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<BillsPaymentTokenModel>> GetBillerPaymentToken(int id)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
                var endpoint = Encryption.decodeString(_wallet_endpoints.GetBillsPaymentToken);
                string api_url = $"{baseUrl}{endpoint}/{id}";

                Console.WriteLine($"[DEBUG] Fetching Main KYC Level: {api_url}");
                var response = await _httpClient.GetAsync(api_url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP Error: {response.StatusCode}. Message: {errorMessage}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var details = JsonConvert.DeserializeObject<BillsPaymentTokenModel>(content);

                if (details == null)
                {
                    throw new Exception("Empty or null response data from API.");
                }

                return new ApiResponseModel<BillsPaymentTokenModel>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Description = "Main KYC Level fetched successfully.",
                    Data = details
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in LoadMainKYCLevel: {e.Message}");
                return new ApiResponseModel<BillsPaymentTokenModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }


        // Get Processing Fee
        public async Task<GetBillsPaymentFeeModel> GetBillerPaymentFee()
        {

            var responseData = "";
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            try
            {
                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWallet);
                var endpointNew = Encryption.decodeString(_wallet_endpoints.GetBillsPaymentProcessingFee);

                var endpoint = baseUrl + endpointNew;
                var apiUrl = endpoint;
                Console.WriteLine($" API Endpoint for cashout fee is {apiUrl}");
                // here's the problem
                var response = await _httpClient.GetAsync(apiUrl);


                Console.WriteLine($"Response from API Client after Generation : Code is {response.StatusCode} . . .  & API Signature is {response}");


                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Code from generation {responseData}");
                    var apiResponseData = JsonConvert.DeserializeObject<GetBillsPaymentFeeModel>(responseData);
                    // var accesStatus = new AuthResponseModel(new AuthResponseModel(), new ApiResponseModel());
                    //Console.WriteLine($"ApiClient - Signature is: {apiResponseData.primaryInfo.primaryID}");
                    return apiResponseData;

                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                    var apiResponseData = JsonConvert.DeserializeObject<GetBillsPaymentFeeModel>(responseData);
                    return apiResponseData;

                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error during generation of reference number:  {ex.Message}");
                return new GetBillsPaymentFeeModel { tranDesc = "INVALID", amount = 0.00 };
                // return new ApiResponseData { accountKey = "Not Found", signature = $"invalid_user {ex.Message}" };
            }
        }





        public async Task<ApiResponseModel<PostBillsTransactionResponseModel>> PostBillerPayment(PostBillsPaymentTransactionModel postData)
        {
            var responseData = "";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);


            string api_cashout_url = _apiBaseUrl + Encryption.decodeString(_wallet_endpoints.PostBillsPayment);
            Console.WriteLine($"[DEBUG] Built Url for Cashout is {api_cashout_url} ---- > TESTING AUTH FOR CASHOUT");
            Console.WriteLine($"[DEBUG] Token in Code: {_httpClient.DefaultRequestHeaders.Authorization}");

            try
            {
                var jsonWithdrawData = JsonConvert.SerializeObject(postData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonWithdrawData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data for cashout: {jsonWithdrawData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_cashout_url, byteContent);


                    Console.WriteLine($"[DEBUG] Response Status Code for Cashout: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<PostBillsTransactionResponseModel>(responseData);
                        Console.WriteLine("[DEBUG] Cashout Response : " + responseData);

                        return new ApiResponseModel<PostBillsTransactionResponseModel>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Cashout Successful!",
                            Data = deserializedResponse
                        };
                    }
                    else
                    {
                        string errorContent = await responseClient.Content.ReadAsStringAsync();
                        string errorMessage;

                        try
                        {
                            var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                            errorMessage = errorJson?.Message ?? $"An error occurred. {errorJson.Message} ";
                        }
                        catch (JsonException)
                        {
                            errorMessage = errorContent;
                            Console.WriteLine($"Exception Error on Parsing Error Content - {errorMessage}");
                        }

                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<PostBillsTransactionResponseModel>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Cashout API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on EXCEPTION WITHDRAWAL");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<PostBillsTransactionResponseModel>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Cashout: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<PostBillsTransactionResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }


        // FUND TRANSFER ------------- TODO


        public async Task<ApiResponseModel<PostBillsTransactionResponseModel>> POSTFundTransfer(POSTFundTransferModel postData)
        {
            var responseData = "";
            var startTime = DateTime.Now;

            Console.WriteLine($"[DEBUG] ===== POSTFundTransfer STARTED at {startTime:yyyy-MM-dd HH:mm:ss.fff} =====");
            Console.WriteLine($"[DEBUG] Input Data - AccountId: {postData?.idno ?? "NULL"}, wtax: {postData?.wtax ?? 0},pfee: {postData?.process_fee ?? 0}, Amount: {postData?.amount ?? 0}, net: {postData?.netamt ?? 0}, Recipient: {postData?.memname ?? "NULL"}");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            string api_cashout_url = _apiBaseUrl + Encryption.decodeString(_wallet_endpoints.PostFundTransfer);
            Console.WriteLine($"[DEBUG] Built URL for FUND TRANSFER: {api_cashout_url}");
            Console.WriteLine($"[DEBUG] Auth Header Type: {Encryption.decodeString(_constants.authHeader)}");
            Console.WriteLine($"[DEBUG] Access Token (first 20 chars): {(_accessToken?.Length > 20 ? _accessToken.Substring(0, 20) + "..." : _accessToken ?? "NULL")}");
            Console.WriteLine($"[DEBUG] Full Authorization Header: {_httpClient.DefaultRequestHeaders.Authorization}");

            try
            {
                var jsonWithdrawData = JsonConvert.SerializeObject(postData, Formatting.Indented);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonWithdrawData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data for fund transfer:");
                Console.WriteLine($"[DEBUG] {jsonWithdrawData}");
                Console.WriteLine($"[DEBUG] Content Length: {buffer.Length} bytes");
                Console.WriteLine($"[DEBUG] Content Type: {byteContent.Headers.ContentType}");

                try
                {
                    Console.WriteLine($"[DEBUG] Sending POST request to: {api_cashout_url}");
                    var requestStartTime = DateTime.Now;

                    var responseClient = await _httpClient.PostAsync(api_cashout_url, byteContent);

                    var requestEndTime = DateTime.Now;
                    var requestDuration = requestEndTime - requestStartTime;

                    Console.WriteLine($"[DEBUG] Request completed in {requestDuration.TotalMilliseconds}ms");
                    Console.WriteLine($"[DEBUG] Response Status Code: {responseClient.StatusCode}");
                    Console.WriteLine($"[DEBUG] Response Reason Phrase: {responseClient.ReasonPhrase}");
                    Console.WriteLine($"[DEBUG] Response Headers Count: {responseClient.Headers.Count()}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        Console.WriteLine($"[DEBUG] Success Response Content Length: {responseData.Length} characters");
                        Console.WriteLine($"[DEBUG] Success Response Content: {responseData}");

                        try
                        {
                            var deserializedResponse = JsonConvert.DeserializeObject<PostBillsTransactionResponseModel>(responseData);
                            Console.WriteLine($"[DEBUG] Successfully deserialized response");
                            Console.WriteLine($"[DEBUG] Deserialized Response: {JsonConvert.SerializeObject(deserializedResponse, Formatting.Indented)}");

                            var totalDuration = DateTime.Now - startTime;
                            Console.WriteLine($"[DEBUG] ===== POSTFundTransfer SUCCESS - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                            return new ApiResponseModel<PostBillsTransactionResponseModel>
                            {
                                IsSuccess = true,
                                StatusCode = 200,
                                Description = "Fund transfer Successful!",
                                Data = deserializedResponse
                            };
                        }
                        catch (JsonException jsonEx)
                        {
                            Console.WriteLine($"[ERROR] JSON Deserialization failed: {jsonEx.Message}");
                            Console.WriteLine($"[ERROR] JSON Stack Trace: {jsonEx.StackTrace}");
                            throw;
                        }
                    }
                    else
                    {
                        string errorContent = await responseClient.Content.ReadAsStringAsync();
                        Console.WriteLine($"[DEBUG] Error Response Content Length: {errorContent.Length} characters");
                        Console.WriteLine($"[DEBUG] Error Response Content: {errorContent}");

                        string errorMessage;

                        try
                        {
                            var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                            errorMessage = errorJson?.Message ?? $"An error occurred. {errorJson?.Message}";
                            Console.WriteLine($"[DEBUG] Parsed Error Message: {errorMessage}");
                        }
                        catch (JsonException jsonEx)
                        {
                            errorMessage = errorContent;
                            Console.WriteLine($"[ERROR] Exception Error on Parsing Error Content: {jsonEx.Message}");
                            Console.WriteLine($"[ERROR] Raw Error Content: {errorMessage}");
                        }

                        Console.WriteLine($"[ERROR] Critical Error - Status: {responseClient.StatusCode}, Message: {errorMessage}");
                        Console.WriteLine($"[ERROR] Request URI: {responseClient.RequestMessage?.RequestUri}");
                        Console.WriteLine($"[ERROR] Request Method: {responseClient.RequestMessage?.Method}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== POSTFundTransfer FAILED - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return new ApiResponseModel<PostBillsTransactionResponseModel>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"[ERROR] Response Error on EXCEPTION WITHDRAWAL");
                    Console.WriteLine($"[ERROR] Exception Type: {e.GetType().Name}");
                    Console.WriteLine($"[ERROR] Exception Message: {e.Message}");
                    Console.WriteLine($"[ERROR] Exception Stack Trace: {e.StackTrace}");
                    Console.WriteLine($"[ERROR] Inner Exception: {e.InnerException?.Message ?? "None"}");

                    var totalDuration = DateTime.Now - startTime;
                    Console.WriteLine($"[DEBUG] ===== POSTFundTransfer EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                    return new ApiResponseModel<PostBillsTransactionResponseModel>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Fund transfer: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ERROR] HttpRequestException occurred");
                Console.WriteLine($"[ERROR] HttpRequestException Message: {ex.Message}");
                Console.WriteLine($"[ERROR] HttpRequestException Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== POSTFundTransfer HTTP EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return new ApiResponseModel<PostBillsTransactionResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<List<CommissionDetailsResponseList>>> LoadCommissionDetailsList(string accountId)
        {
            var startTime = DateTime.Now;

            Console.WriteLine($"[DEBUG] ===== LoadCommissionDetailsList STARTED at {startTime:yyyy-MM-dd HH:mm:ss.fff} =====");
            Console.WriteLine($"[DEBUG] Input AccountId: {accountId ?? "NULL"}");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            Console.WriteLine($"[DEBUG] Auth Header Type: {Encryption.decodeString(_constants.authHeader)}");
            Console.WriteLine($"[DEBUG] Access Token (first 20 chars): {(_accessToken?.Length > 20 ? _accessToken.Substring(0, 20) + "..." : _accessToken ?? "NULL")}");

            var endpoint = Encryption.decodeString(_wallet_endpoints.GetCommissionDetailsList);
            var apiUrl = _apiBaseUrl + endpoint + accountId;

            Console.WriteLine($"[DEBUG] Decoded Endpoint: {endpoint}");
            Console.WriteLine($"[DEBUG] API Base URL: {_apiBaseUrl}");
            Console.WriteLine($"[DEBUG] Full API URL: {apiUrl}");

            try
            {
                Console.WriteLine($"[DEBUG] Sending GET request to: {apiUrl}");
                var requestStartTime = DateTime.Now;

                var GET_History_transaction_list_response = await _httpClient.GetAsync(apiUrl);

                var requestEndTime = DateTime.Now;
                var requestDuration = requestEndTime - requestStartTime;

                Console.WriteLine($"[DEBUG] Request completed in {requestDuration.TotalMilliseconds}ms");
                Console.WriteLine($"[DEBUG] Response Status Code: {GET_History_transaction_list_response.StatusCode}");
                Console.WriteLine($"[DEBUG] Response Reason Phrase: {GET_History_transaction_list_response.ReasonPhrase}");
                Console.WriteLine($"[DEBUG] Response Headers Count: {GET_History_transaction_list_response.Headers.Count()}");

                if (GET_History_transaction_list_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[DEBUG] Success Response Content Length: {responseData.Length} characters");
                    Console.WriteLine($"[DEBUG] Success Response Content: {responseData}");

                    try
                    {
                        var historyListRepsonse = JsonConvert.DeserializeObject<List<CommissionDetailsResponseList>>(responseData);
                        Console.WriteLine($"[DEBUG] Successfully deserialized commission details list");
                        Console.WriteLine($"[DEBUG] Commission Details Count: {historyListRepsonse?.Count ?? 0}");

                        if (historyListRepsonse?.Count > 0)
                        {
                            Console.WriteLine($"[DEBUG] First Item Sample: {JsonConvert.SerializeObject(historyListRepsonse.FirstOrDefault(), Formatting.Indented)}");
                        }

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== LoadCommissionDetailsList SUCCESS - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return new ApiResponseModel<List<CommissionDetailsResponseList>>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Fetched List of Fund Transfers!",
                            Data = historyListRepsonse
                        };
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"[ERROR] JSON Deserialization failed: {jsonEx.Message}");
                        Console.WriteLine($"[ERROR] JSON Stack Trace: {jsonEx.StackTrace}");
                        throw;
                    }
                }
                else
                {
                    var errorContent = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[ERROR] Error Response Content: {errorContent}");
                    Console.WriteLine($"[ERROR] Error Status Code: {GET_History_transaction_list_response.StatusCode}");

                    var totalDuration = DateTime.Now - startTime;
                    Console.WriteLine($"[DEBUG] ===== LoadCommissionDetailsList FAILED - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                    throw new HttpRequestException(message: $"API call failed with status {GET_History_transaction_list_response.StatusCode}: {errorContent}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in LoadCommissionDetailsList");
                Console.WriteLine($"[ERROR] Exception Type: {e.GetType().Name}");
                Console.WriteLine($"[ERROR] Exception Message: {e.Message}");
                Console.WriteLine($"[ERROR] Exception Stack Trace: {e.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {e.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== LoadCommissionDetailsList EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return new ApiResponseModel<List<CommissionDetailsResponseList>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Cannot Fetch Transaction List! {e.Message}",
                    Data = null
                };
            }
        }


        public async Task<CommissionBalanceResponse> LoadCommissionbalance(string accountId)
        {
            var responseData = "";
            var startTime = DateTime.Now;

            Console.WriteLine($"[DEBUG] ===== LoadCommissionbalance STARTED at {startTime:yyyy-MM-dd HH:mm:ss.fff} =====");
            Console.WriteLine($"[DEBUG] Input AccountId: {accountId ?? "NULL"}");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            Console.WriteLine($"[DEBUG] Auth Header Type: {Encryption.decodeString(_constants.authHeader)}");
            Console.WriteLine($"[DEBUG] Access Token (first 20 chars): {(_accessToken?.Length > 20 ? _accessToken.Substring(0, 20) + "..." : _accessToken ?? "NULL")}");

            try
            {
                var endpoint = Encryption.decodeString(_wallet_endpoints.GetCommissionBalance);
                var apiUrl = _apiBaseUrl + endpoint + accountId;

                Console.WriteLine($"[DEBUG] Decoded Endpoint: {endpoint}");
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
                        var apiResponseData = JsonConvert.DeserializeObject<CommissionBalanceResponse>(responseData);
                        Console.WriteLine($"[DEBUG] Successfully deserialized commission balance response");
                        Console.WriteLine($"[DEBUG] Deserialized Response: {JsonConvert.SerializeObject(apiResponseData, Formatting.Indented)}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== LoadCommissionbalance SUCCESS - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return apiResponseData;
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"[ERROR] JSON Deserialization failed: {jsonEx.Message}");
                        Console.WriteLine($"[ERROR] JSON Stack Trace: {jsonEx.StackTrace}");
                        Console.WriteLine($"[ERROR] Raw Response Data: {responseData}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== LoadCommissionbalance JSON ERROR - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return _defaults_wal.invalidCommissionBalance;
                    }
                }
                else
                {
                    Console.WriteLine($"[WARNING] Non-success status code received: {response.StatusCode}");
                    Console.WriteLine($"[WARNING] Attempting to deserialize error response");

                    try
                    {
                        var apiResponseData = JsonConvert.DeserializeObject<CommissionBalanceResponse>(responseData);
                        Console.WriteLine($"[DEBUG] Successfully deserialized error response");
                        Console.WriteLine($"[DEBUG] Error Response Data: {JsonConvert.SerializeObject(apiResponseData, Formatting.Indented)}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== LoadCommissionbalance ERROR RESPONSE - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return apiResponseData;
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"[ERROR] Failed to deserialize error response: {jsonEx.Message}");
                        Console.WriteLine($"[ERROR] Raw Error Response: {responseData}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== LoadCommissionbalance DESERIALIZATION ERROR - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return _defaults_wal.invalidCommissionBalance;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ERROR] HttpRequestException in LoadCommissionbalance");
                Console.WriteLine($"[ERROR] HttpRequestException Message: {ex.Message}");
                Console.WriteLine($"[ERROR] HttpRequestException Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== LoadCommissionbalance HTTP EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return _defaults_wal.invalidCommissionBalance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] General Exception in LoadCommissionbalance");
                Console.WriteLine($"[ERROR] Exception Type: {ex.GetType().Name}");
                Console.WriteLine($"[ERROR] Exception Message: {ex.Message}");
                Console.WriteLine($"[ERROR] Exception Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== LoadCommissionbalance GENERAL EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return _defaults_wal.invalidCommissionBalance;
            }
        }







        // Get Transfer Processing Fee
        public async Task<GetTransferProcessingFeeModel> GetTransferProcessingFee()
        {
            var responseData = "";
            var startTime = DateTime.Now;

            Console.WriteLine($"[DEBUG] ===== GetTransferProcessingFee STARTED at {startTime:yyyy-MM-dd HH:mm:ss.fff} =====");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            Console.WriteLine($"[DEBUG] Auth Header Type: {Encryption.decodeString(_constants.authHeader)}");
            Console.WriteLine($"[DEBUG] Access Token (first 20 chars): {(_accessToken?.Length > 20 ? _accessToken.Substring(0, 20) + "..." : _accessToken ?? "NULL")}");

            try
            {
                var endpoint = Encryption.decodeString(_wallet_endpoints.GetTransferProcessingFee);
                var apiUrl = _apiBaseUrl + endpoint;

                Console.WriteLine($"[DEBUG] Decoded Endpoint: {endpoint}");
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
                        var apiResponseData = JsonConvert.DeserializeObject<GetTransferProcessingFeeModel>(responseData);
                        Console.WriteLine($"[DEBUG] Successfully deserialized transfer processing fee response");
                        Console.WriteLine($"[DEBUG] Deserialized Response: {JsonConvert.SerializeObject(apiResponseData, Formatting.Indented)}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetTransferProcessingFee SUCCESS - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return apiResponseData;
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"[ERROR] JSON Deserialization failed: {jsonEx.Message}");
                        Console.WriteLine($"[ERROR] JSON Stack Trace: {jsonEx.StackTrace}");
                        Console.WriteLine($"[ERROR] Raw Response Data: {responseData}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetTransferProcessingFee JSON ERROR - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return _defaults_wal.invalidTransferProcessingFee;
                    }
                }
                else
                {
                    Console.WriteLine($"[WARNING] Non-success status code received: {response.StatusCode}");
                    Console.WriteLine($"[WARNING] Attempting to deserialize error response");

                    try
                    {
                        var apiResponseData = JsonConvert.DeserializeObject<GetTransferProcessingFeeModel>(responseData);
                        Console.WriteLine($"[DEBUG] Successfully deserialized error response");
                        Console.WriteLine($"[DEBUG] Error Response Data: {JsonConvert.SerializeObject(apiResponseData, Formatting.Indented)}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetTransferProcessingFee ERROR RESPONSE - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return apiResponseData;
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"[ERROR] Failed to deserialize error response: {jsonEx.Message}");
                        Console.WriteLine($"[ERROR] Raw Error Response: {responseData}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetTransferProcessingFee DESERIALIZATION ERROR - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return _defaults_wal.invalidTransferProcessingFee;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ERROR] HttpRequestException in GetTransferProcessingFee");
                Console.WriteLine($"[ERROR] HttpRequestException Message: {ex.Message}");
                Console.WriteLine($"[ERROR] HttpRequestException Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== GetTransferProcessingFee HTTP EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return _defaults_wal.invalidTransferProcessingFee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] General Exception in GetTransferProcessingFee");
                Console.WriteLine($"[ERROR] Exception Type: {ex.GetType().Name}");
                Console.WriteLine($"[ERROR] Exception Message: {ex.Message}");
                Console.WriteLine($"[ERROR] Exception Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== GetTransferProcessingFee GENERAL EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return _defaults_wal.invalidTransferProcessingFee;
            }
        }

        // Get WTax Fee
        public async Task<GetWtaxFeeModel> GetWtaxFee()
        {
            var responseData = "";
            var startTime = DateTime.Now;

            Console.WriteLine($"[DEBUG] ===== GetWtaxFee STARTED at {startTime:yyyy-MM-dd HH:mm:ss.fff} =====");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

            Console.WriteLine($"[DEBUG] Auth Header Type: {Encryption.decodeString(_constants.authHeader)}");
            Console.WriteLine($"[DEBUG] Access Token (first 20 chars): {(_accessToken?.Length > 20 ? _accessToken.Substring(0, 20) + "..." : _accessToken ?? "NULL")}");

            try
            {
                var endpoint = Encryption.decodeString(_wallet_endpoints.GetWtaxFee);
                var apiUrl = _apiBaseUrl + endpoint;

                Console.WriteLine($"[DEBUG] Decoded Endpoint: {endpoint}");
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
                        var apiResponseData = JsonConvert.DeserializeObject<GetWtaxFeeModel>(responseData);
                        Console.WriteLine($"[DEBUG] Successfully deserialized wtax fee response");
                        Console.WriteLine($"[DEBUG] Deserialized Response: {JsonConvert.SerializeObject(apiResponseData, Formatting.Indented)}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetWtaxFee SUCCESS - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return apiResponseData;
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"[ERROR] JSON Deserialization failed: {jsonEx.Message}");
                        Console.WriteLine($"[ERROR] JSON Stack Trace: {jsonEx.StackTrace}");
                        Console.WriteLine($"[ERROR] Raw Response Data: {responseData}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetWtaxFee JSON ERROR - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return _defaults_wal.invalidWtaxFee;
                    }
                }
                else
                {
                    Console.WriteLine($"[WARNING] Non-success status code received: {response.StatusCode}");
                    Console.WriteLine($"[WARNING] Attempting to deserialize error response");

                    try
                    {
                        var apiResponseData = JsonConvert.DeserializeObject<GetWtaxFeeModel>(responseData);
                        Console.WriteLine($"[DEBUG] Successfully deserialized error response");
                        Console.WriteLine($"[DEBUG] Error Response Data: {JsonConvert.SerializeObject(apiResponseData, Formatting.Indented)}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetWtaxFee ERROR RESPONSE - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return apiResponseData;
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"[ERROR] Failed to deserialize error response: {jsonEx.Message}");
                        Console.WriteLine($"[ERROR] Raw Error Response: {responseData}");

                        var totalDuration = DateTime.Now - startTime;
                        Console.WriteLine($"[DEBUG] ===== GetWtaxFee DESERIALIZATION ERROR - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                        return _defaults_wal.invalidWtaxFee;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"[ERROR] HttpRequestException in GetWtaxFee");
                Console.WriteLine($"[ERROR] HttpRequestException Message: {ex.Message}");
                Console.WriteLine($"[ERROR] HttpRequestException Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== GetWtaxFee HTTP EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return _defaults_wal.invalidWtaxFee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] General Exception in GetWtaxFee");
                Console.WriteLine($"[ERROR] Exception Type: {ex.GetType().Name}");
                Console.WriteLine($"[ERROR] Exception Message: {ex.Message}");
                Console.WriteLine($"[ERROR] Exception Stack Trace: {ex.StackTrace}");
                Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException?.Message ?? "None"}");

                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== GetWtaxFee GENERAL EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");

                return _defaults_wal.invalidWtaxFee;
            }
        }


        /// <summary>
        /// GET /v1/Announcements/GetActiveAnnouncements (optional query: announcementType, urgencyType).
        /// Uses ApiKey header (no Bearer).
        /// </summary>
        public async Task<ApiResponseModel<List<WalletAnnouncementItem>>> GetActiveAnnouncements(
            int? announcementType = null,
            int? urgencyType = null)
        {
            var startTime = DateTime.Now;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

            var endpoint = _wallet_endpoints.GetActiveAnnouncements;
            var queryParts = new List<string>();
            if (announcementType.HasValue)
                queryParts.Add($"announcementType={announcementType.Value}");
            if (urgencyType.HasValue)
                queryParts.Add($"urgencyType={urgencyType.Value}");
            var qs = queryParts.Count > 0 ? "?" + string.Join("&", queryParts) : "";
            var apiUrl = _apiBaseUrl + endpoint + qs;

            try
            {
                Console.WriteLine($"[DEBUG] ===== GetActiveAnnouncements START =====");
                Console.WriteLine($"[DEBUG] URL: {apiUrl}");
                Console.WriteLine($"[DEBUG] Params: announcementType={(announcementType.HasValue ? announcementType.Value.ToString() : "null")}, urgencyType={(urgencyType.HasValue ? urgencyType.Value.ToString() : "null")}");

                var response = await _httpClient.GetAsync(apiUrl);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[DEBUG] HTTP {(int)response.StatusCode} ({response.StatusCode}) for GetActiveAnnouncements");
                Console.WriteLine($"[DEBUG] ContentLength={(content?.Length ?? 0)} Preview={(string.IsNullOrEmpty(content) ? "<empty>" : content.Substring(0, Math.Min(500, content.Length)))}");

                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<WalletAnnouncementItem>>(content);
                    var totalDuration = DateTime.Now - startTime;
                    Console.WriteLine($"[DEBUG] ===== GetActiveAnnouncements SUCCESS - Total Duration: {totalDuration.TotalMilliseconds}ms =====");
                    return new ApiResponseModel<List<WalletAnnouncementItem>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Announcements fetched.",
                        Data = list
                    };
                }

                {
                    var totalDuration = DateTime.Now - startTime;
                    Console.WriteLine($"[DEBUG] ===== GetActiveAnnouncements FAIL - Total Duration: {totalDuration.TotalMilliseconds}ms =====");
                }
                return new ApiResponseModel<List<WalletAnnouncementItem>>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    Description = content,
                    Data = null
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[DEBUG] ===== GetActiveAnnouncements EXCEPTION =====");
                Console.WriteLine($"[DEBUG] ExceptionType={e.GetType().Name} Message={e.Message}");
                Console.WriteLine($"[DEBUG] InnerException={e.InnerException?.Message ?? "None"}");
                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== GetActiveAnnouncements EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");
                return new ApiResponseModel<List<WalletAnnouncementItem>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Cannot fetch announcements: {e.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// GET /v1/Announcements/GetAllAnnouncements (optional query: announcementType, status).
        /// Uses ApiKey header (no Bearer).
        /// </summary>
        public async Task<ApiResponseModel<List<WalletAnnouncementItem>>> GetAllAnnouncements(
            int? announcementType = null,
            bool? status = null)
        {
            var startTime = DateTime.Now;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

            var endpoint = _wallet_endpoints.GetAllAnnouncements;
            var queryParts = new List<string>();
            if (announcementType.HasValue)
                queryParts.Add($"announcementType={announcementType.Value}");
            if (status.HasValue)
                queryParts.Add($"status={status.Value.ToString().ToLowerInvariant()}");
            var qs = queryParts.Count > 0 ? "?" + string.Join("&", queryParts) : "";
            var apiUrl = _apiBaseUrl + endpoint + qs;

            try
            {
                Console.WriteLine($"[DEBUG] ===== GetAllAnnouncements START =====");
                Console.WriteLine($"[DEBUG] URL: {apiUrl}");
                Console.WriteLine($"[DEBUG] Params: announcementType={(announcementType.HasValue ? announcementType.Value.ToString() : "null")}, status={(status.HasValue ? status.Value.ToString() : "null")}");

                var response = await _httpClient.GetAsync(apiUrl);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[DEBUG] HTTP {(int)response.StatusCode} ({response.StatusCode}) for GetAllAnnouncements");
                Console.WriteLine($"[DEBUG] ContentLength={(content?.Length ?? 0)} Preview={(string.IsNullOrEmpty(content) ? "<empty>" : content.Substring(0, Math.Min(500, content.Length)))}");

                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<WalletAnnouncementItem>>(content);
                    var totalDuration = DateTime.Now - startTime;
                    Console.WriteLine($"[DEBUG] ===== GetAllAnnouncements SUCCESS - Total Duration: {totalDuration.TotalMilliseconds}ms =====");
                    return new ApiResponseModel<List<WalletAnnouncementItem>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Announcements fetched.",
                        Data = list
                    };
                }

                {
                    var totalDuration = DateTime.Now - startTime;
                    Console.WriteLine($"[DEBUG] ===== GetAllAnnouncements FAIL - Total Duration: {totalDuration.TotalMilliseconds}ms =====");
                }
                return new ApiResponseModel<List<WalletAnnouncementItem>>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    Description = content,
                    Data = null
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[DEBUG] ===== GetAllAnnouncements EXCEPTION =====");
                Console.WriteLine($"[DEBUG] ExceptionType={e.GetType().Name} Message={e.Message}");
                Console.WriteLine($"[DEBUG] InnerException={e.InnerException?.Message ?? "None"}");
                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== GetAllAnnouncements EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");
                return new ApiResponseModel<List<WalletAnnouncementItem>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Cannot fetch announcements: {e.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// GET /v1/Announcements/GetAnnouncementById/{announcementId}.
        /// Uses ApiKey header (no Bearer).
        /// </summary>
        public async Task<ApiResponseModel<WalletAnnouncementItem>> GetAnnouncementById(int announcementId)
        {
            var startTime = DateTime.Now;
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

            var endpoint = _wallet_endpoints.GetAnnouncementById;
            var apiUrl = _apiBaseUrl + endpoint + announcementId;

            try
            {
                Console.WriteLine($"[DEBUG] ===== GetAnnouncementById START =====");
                Console.WriteLine($"[DEBUG] URL: {apiUrl}");
                Console.WriteLine($"[DEBUG] Params: announcementId={announcementId}");

                var response = await _httpClient.GetAsync(apiUrl);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[DEBUG] HTTP {(int)response.StatusCode} ({response.StatusCode}) for GetAnnouncementById");
                Console.WriteLine($"[DEBUG] ContentLength={(content?.Length ?? 0)} Preview={(string.IsNullOrEmpty(content) ? "<empty>" : content.Substring(0, Math.Min(500, content.Length)))}");

                if (response.IsSuccessStatusCode)
                {
                    var item = JsonConvert.DeserializeObject<WalletAnnouncementItem>(content);
                    var totalDuration = DateTime.Now - startTime;
                    Console.WriteLine($"[DEBUG] ===== GetAnnouncementById SUCCESS - Total Duration: {totalDuration.TotalMilliseconds}ms =====");
                    return new ApiResponseModel<WalletAnnouncementItem>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Announcement fetched.",
                        Data = item
                    };
                }

                {
                    var totalDuration = DateTime.Now - startTime;
                    Console.WriteLine($"[DEBUG] ===== GetAnnouncementById FAIL - Total Duration: {totalDuration.TotalMilliseconds}ms =====");
                }
                return new ApiResponseModel<WalletAnnouncementItem>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    Description = content,
                    Data = null
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[DEBUG] ===== GetAnnouncementById EXCEPTION =====");
                Console.WriteLine($"[DEBUG] ExceptionType={e.GetType().Name} Message={e.Message}");
                Console.WriteLine($"[DEBUG] InnerException={e.InnerException?.Message ?? "None"}");
                var totalDuration = DateTime.Now - startTime;
                Console.WriteLine($"[DEBUG] ===== GetAnnouncementById EXCEPTION - Total Duration: {totalDuration.TotalMilliseconds}ms =====");
                return new ApiResponseModel<WalletAnnouncementItem>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Cannot fetch announcement: {e.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// GET /util/v1/LoadRaffle?status=
        /// </summary>
        public async Task<ApiResponseModel<List<RaffleListItem>>> LoadRaffle(bool status)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var endpoint = Encryption.decodeString(_wallet_endpoints.LoadRaffle);
                var apiUrl = $"{baseUrl}{endpoint}?status={status.ToString().ToLowerInvariant()}";

                Console.WriteLine($"[DEBUG] Fetching Raffle List: {apiUrl}");
                var response = await _httpClient.GetAsync(apiUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<RaffleListItem>>(content);
                    return new ApiResponseModel<List<RaffleListItem>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Raffle list fetched successfully.",
                        Data = list
                    };
                }

                return new ApiResponseModel<List<RaffleListItem>>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    Description = content,
                    Data = null
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in LoadRaffle: {e.Message}");
                return new ApiResponseModel<List<RaffleListItem>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// GET /util/v1/LoadMemberRaffleTickets?Option=&amp;RaffleID=&amp;AccountKey=
        /// </summary>
        public async Task<ApiResponseModel<List<MemberRaffleTicket>>> LoadMemberRaffleTickets(
            int option,
            int raffleId,
            string accountKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountKey))
                {
                    return new ApiResponseModel<List<MemberRaffleTicket>>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Description = "accountKey is required.",
                        Data = null
                    };
                }

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var endpoint = Encryption.decodeString(_wallet_endpoints.LoadMemberRaffleTickets);
                var apiUrl =
                    $"{baseUrl}{endpoint}?Option={option}&RaffleID={raffleId}&AccountKey={Uri.EscapeDataString(accountKey)}";

                Console.WriteLine($"[DEBUG] Fetching Member Raffle Tickets: {apiUrl}");
                var response = await _httpClient.GetAsync(apiUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var list = JsonConvert.DeserializeObject<List<MemberRaffleTicket>>(content);
                    return new ApiResponseModel<List<MemberRaffleTicket>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Member raffle tickets fetched successfully.",
                        Data = list
                    };
                }

                return new ApiResponseModel<List<MemberRaffleTicket>>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    Description = content,
                    Data = null
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in LoadMemberRaffleTickets: {e.Message}");
                return new ApiResponseModel<List<MemberRaffleTicket>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }

        /// <summary>
        /// GET /util/v1/GetRaffleTicketCtr?Option=&amp;RaffleID=&amp;AccountKey=
        /// </summary>
        public async Task<ApiResponseModel<RaffleTicketCounter>> GetRaffleTicketCtr(
            int option,
            int raffleId,
            string accountKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountKey))
                {
                    return new ApiResponseModel<RaffleTicketCounter>
                    {
                        IsSuccess = false,
                        StatusCode = 400,
                        Description = "accountKey is required.",
                        Data = null
                    };
                }

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);

                var baseUrl = Encryption.decodeString(_wallet_endpoints.baseUrlWalletKYC);
                var endpoint = Encryption.decodeString(_wallet_endpoints.GetRaffleTicketCtr);
                var apiUrl =
                    $"{baseUrl}{endpoint}?Option={option}&RaffleID={raffleId}&AccountKey={Uri.EscapeDataString(accountKey)}";

                Console.WriteLine($"[DEBUG] Fetching Raffle Ticket Count: {apiUrl}");
                var response = await _httpClient.GetAsync(apiUrl);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var counter = JsonConvert.DeserializeObject<RaffleTicketCounter>(content);
                    return new ApiResponseModel<RaffleTicketCounter>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Raffle ticket count fetched successfully.",
                        Data = counter
                    };
                }

                return new ApiResponseModel<RaffleTicketCounter>
                {
                    IsSuccess = false,
                    StatusCode = (int)response.StatusCode,
                    Description = content,
                    Data = null
                };
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] Exception in GetRaffleTicketCtr: {e.Message}");
                return new ApiResponseModel<RaffleTicketCounter>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error: {e.Message}",
                    Data = null
                };
            }
        }

        // GET Module Status list
        public async Task<ApiResponseModel<ModuleStatus>> LoadModuleStatusList(int sysId)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Encryption.decodeString(_constants.authHeader), _accessToken);
            //_httpClient.DefaultRequestHeaders.Add("ApiKey", "f24b51dfd6fda3a6fb20882c1554790e");


            string endpoint = Encryption.decodeString(_wallet_endpoints.GetModuleStatus);
            string api_url = $"{_apiBaseUrl}{endpoint}?sysid={sysId}";
            Console.WriteLine($"[DEBUG] API Built for Get Module Status : {api_url}");

            try
            {
                var response = await _httpClient.GetAsync(api_url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("[DEBUG] Module Status API Content : " + content);

                    var list = JsonConvert.DeserializeObject<ModuleStatus>(content);

                    return new ApiResponseModel<ModuleStatus>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Module status list fetched!",
                        Data = list
                    };
                }
                else
                {
                    throw new HttpRequestException(message: await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<ModuleStatus>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot fetch Module Status! {e.Message}",
                    Data = null
                };
            }
        }



    }




}
