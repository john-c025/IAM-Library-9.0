using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using IAM_Library.models.general;
using IAM_Library.models.registration;
using IAM_Library.models.wallet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.wallet.api
{
    public class MainWalletAPIClient
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
            { "LoadWalletCommHistoryBase", Encryption.decodeString(_constants.walletCommHistory) },
            { "WithdrawWalletComm", Encryption.decodeString(_constants.postWalletWithdrawComm) },
            { "IdNumberParam", Encryption.decodeString(_constants.idNumberParam) },
            { "TranTypeParam", Encryption.decodeString(_constants.tranTypeParam) },
            { "RefNoParam", Encryption.decodeString(_constants.refNoParam) },
            { "ReportsCAccKey", Encryption.decodeString(_constants.reportsCSummaryAccKey) },
            { "GenerateAutoNumAPI", Encryption.decodeString(_constants.generateAutoNum) },
            { "EncashmentTypeParam", Encryption.decodeString(_constants.encashmentTypeParam) },
            {"LoadEncashmentTypeBase",Encryption.decodeString(_constants.loadEncashmntTypes) },
            {"GetEncashmentDetailsBase",Encryption.decodeString(_constants.getEncashmentDetailsBase) },
            {"GetBankAccountDetails",Encryption.decodeString(_constants.GETBankDetailsById) },
            {"IdNumberParamAlt",Encryption.decodeString(_constants.idNumberParamQR) },
            {"loadWalletJ4U",Encryption.decodeString(_constants.walletJ4UBase) },
            {"updateJ4USelection",Encryption.decodeString(_constants.walletJ4UUpdateSelected) },
            {"J4UWithdrawWalletBase",Encryption.decodeString(_constants.J4UWithdrawWalletBase) },
            {"J4UPreview",Encryption.decodeString(_constants.walletJ4UPreviewBase) },



        };

        public MainWalletAPIClient(string apiBaseUrl, AuthApiResponseData accessCredentials, HttpClient httpClient) // constructor
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

        public async Task<ApiResponseModel<BankAccountDetail>> LoadBankAccountDetails(string idNumber)
        {
            Console.WriteLine($"[DEBUG] from API Bank Details client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "GetBankAccountDetails", _apiConfig, (_apiConfig.Endpoints["IdNumberParamAlt"], idNumber));




            try
            {
                Console.WriteLine($"[DEBUG] API Built for Bank Account : {api_verification_activation}");
                var GET_wallet_balance_response = await _httpClient.GetAsync(api_verification_activation);
                if (GET_wallet_balance_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_wallet_balance_response.Content.ReadAsStringAsync();
                    var verification_response_asJson = JsonConvert.DeserializeObject<BankAccountDetail>(responseData);
                    Console.WriteLine("[DEBUG] Bank Response Content : " + responseData);

                    return new ApiResponseModel<BankAccountDetail>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Bank Details Fetched!",
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
                Console.WriteLine($"[DEBUG] from API Bank Details client to building url base url {_apiConfig.BaseUrl}");
                Console.WriteLine("ERROR ON BANK FETCH" + e.Message);
                return new ApiResponseModel<BankAccountDetail>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot fetch Bank Details! {e.Message}",
                    Data = null
                };
            }
        }





        public async Task<ApiResponseModel<WalletJ4uUpdaterepsonseModel>> UpdateSelectionForJ4URecord(WalletJ4UModel updateModel,string idNumber)
        {
            var responseData = "";

            // Test for GitLink
            Console.WriteLine($"[DEBUG] from Report client to building url base url {_apiConfig.BaseUrl}");
            string api_registration_uri = custom.BuildUrl(_apiBaseUrl, "updateJ4USelection", _apiConfig, (_apiConfig.Endpoints["IdNumberParam"], idNumber));
            Console.WriteLine($"[DEBUG] Built Url for selection is {api_registration_uri}");


            try
            {
                var jsonRegData = JsonConvert.SerializeObject(updateModel);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRegData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Log the JSON data being sent
                Console.WriteLine($"[DEBUG] JSON Data for Registration: {jsonRegData}");

                try
                {
                    var responseClient = await _httpClient.PutAsync(api_registration_uri, byteContent);


                    Console.WriteLine("[DEBUG] Registration Before Condition Status Code Check : " + responseClient);

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<WalletJ4uUpdaterepsonseModel>(responseData);
                        Console.WriteLine("[DEBUG] Bank Detail Update Response : " + responseData);

                        return new ApiResponseModel<WalletJ4uUpdaterepsonseModel>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = " Selection J4U Details Update was a success!",
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

                        // Log the error content and status code
                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<WalletJ4uUpdaterepsonseModel>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $" Selection J4U Details API failed: {errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $" Selection J4U  API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine("[ERROR] Response Error on EXCEPTION REGISTRATION");
                    Console.WriteLine($"[DEBUG] Exception for selection j4u update: {e.Message}");
                    return new ApiResponseModel<WalletJ4uUpdaterepsonseModel>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Selection J4U : {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                //Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<WalletJ4uUpdaterepsonseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred." + ex.Message,
                    Data = null
                };
            }
        }

        
        public async Task<ApiResponseModel<J4UPreview>> LoadJ4UPreview(string idNumber)
        {
            Console.WriteLine($"[DEBUG] from API Registration J4U Preview client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "J4UPreview", _apiConfig, (_apiConfig.Endpoints["IdNumberParam"], idNumber));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Balance : {api_verification_activation}");
                var GET_wallet_balance_response = await _httpClient.GetAsync(api_verification_activation);
                if (GET_wallet_balance_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_wallet_balance_response.Content.ReadAsStringAsync();
                    var verification_response_asJson = JsonConvert.DeserializeObject<J4UPreview>(responseData);
                    Console.WriteLine("[DEBUG] j4u preview Response Content : " + responseData);

                    return new ApiResponseModel<J4UPreview>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Preview Fetched!",
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
                return new ApiResponseModel<J4UPreview>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch J4U Preview! {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<WalletBalanceGETResponseModel>> LoadWalletBalance(string idNumber)
        {
            Console.WriteLine($"[DEBUG] from API Registration Wallet Balance client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "GetWalletBalanceBase", _apiConfig, (_apiConfig.Endpoints["IdNumberParam"], idNumber));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Balance : {api_verification_activation}");
                var GET_wallet_balance_response = await _httpClient.GetAsync(api_verification_activation);
                if (GET_wallet_balance_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_wallet_balance_response.Content.ReadAsStringAsync();
                    var verification_response_asJson = JsonConvert.DeserializeObject<WalletBalanceGETResponseModel>(responseData);
                    Console.WriteLine("[DEBUG] balance Response Content : " + responseData);

                    return new ApiResponseModel<WalletBalanceGETResponseModel>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Wallet Balance Fetched!",
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
                return new ApiResponseModel<WalletBalanceGETResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Wallet Balance! {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<WalletJ4UModel>>> LoadJ4UWallet(string accountKey)
        {
            Console.WriteLine($"[DEBUG] from API Registration History List client to building url base url {_apiConfig.BaseUrl}");
            string GEThistoryUrl = custom.BuildUrl(_apiBaseUrl, "loadWalletJ4U", _apiConfig, (_apiConfig.Endpoints["ReportsCAccKey"], accountKey));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Get History List : {GEThistoryUrl}");
                var GET_History_transaction_list_response = await _httpClient.GetAsync(GEThistoryUrl);
                if (GET_History_transaction_list_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    var historyListRepsonse = JsonConvert.DeserializeObject<List<WalletJ4UModel>>(responseData);
                    Console.WriteLine("[DEBUG] History transaction Content : " + responseData);

                    return new ApiResponseModel<List<WalletJ4UModel>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "List of Transactions Fetched!",
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
                return new ApiResponseModel<List<WalletJ4UModel>>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Transaction List! {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<TransactionListModel>>> LoadTransactionHistoryList(string accountKey)
        {
            Console.WriteLine($"[DEBUG] from API Registration History List client to building url base url {_apiConfig.BaseUrl}");
            string GEThistoryUrl = custom.BuildUrl(_apiBaseUrl, "LoadWalletCommHistoryBase", _apiConfig, (_apiConfig.Endpoints["ReportsCAccKey"], accountKey));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Get History List : {GEThistoryUrl}");
                var GET_History_transaction_list_response = await _httpClient.GetAsync(GEThistoryUrl);
                if (GET_History_transaction_list_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_History_transaction_list_response.Content.ReadAsStringAsync();
                    var historyListRepsonse = JsonConvert.DeserializeObject<List<TransactionListModel>>(responseData);
                    Console.WriteLine("[DEBUG] History transaction Content : " + responseData);

                    return new ApiResponseModel<List<TransactionListModel>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "List of Transactions Fetched!",
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
                return new ApiResponseModel<List<TransactionListModel>>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Transaction List! {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<SuccessfulWithdrawalResponse>> WithdrawWalletCommission(WithdrawWalletPOSTModel postData)
        {
            var responseData = "";
            Console.WriteLine($"[DEBUG] from Report client to building url base url {_apiConfig.BaseUrl}");
            string api_withdraw_uri = custom.BuildUrl(_apiBaseUrl, "WithdrawWalletComm", _apiConfig);
            Console.WriteLine($"[DEBUG] Built Url is {api_withdraw_uri}");

            try
            {
                var jsonWithdrawData = JsonConvert.SerializeObject(postData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonWithdrawData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data: {jsonWithdrawData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_withdraw_uri, byteContent);


                    Console.WriteLine($"[DEBUG] Response Status Code: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<SuccessfulWithdrawalResponse>(responseData);
                        Console.WriteLine("[DEBUG] Withdrawal Response : " + responseData);

                        return new ApiResponseModel<SuccessfulWithdrawalResponse>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Withdrawal Successful!",
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

                        return new ApiResponseModel<SuccessfulWithdrawalResponse>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Withdrawal API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on EXCEPTION WITHDRAWAL");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<SuccessfulWithdrawalResponse>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Withdrawal: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<SuccessfulWithdrawalResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<EnashmentGetDetailsModel>> GetEncashmentDetails(string refno, string idno)
        {
            Console.WriteLine($"[DEBUG] from API Registration Encashment Details client to building url base url {_apiConfig.BaseUrl}");
            string api_encashment_details_url = custom.BuildUrl(_apiBaseUrl, "GetEncashmentBase", _apiConfig, (_apiConfig.Endpoints["RefNoParam"], refno), (_apiConfig.Endpoints["IdNumberParam"], idno));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Encashment Details: {api_encashment_details_url}");
                var GET_encashment_details_response = await _httpClient.GetAsync(api_encashment_details_url);
                if (GET_encashment_details_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_encashment_details_response.Content.ReadAsStringAsync();
                    var encashment_details_asJson = JsonConvert.DeserializeObject<EnashmentGetDetailsModel>(responseData);
                    Console.WriteLine("[DEBUG] Encashment Details Response Content : " + responseData);

                    return new ApiResponseModel<EnashmentGetDetailsModel>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Encashment Details Fetched!",
                        Data = encashment_details_asJson
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_encashment_details_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<EnashmentGetDetailsModel>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Encashment Details! {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<AutoGenNumberTransaction>> GenerateAutoNumberTrasaction(string encashmentTypeParam)
        {
            Console.WriteLine($"[DEBUG] from API Registration Encashment Details client to building url base url {_apiConfig.BaseUrl}");
            string autoGen = custom.BuildUrl(_apiBaseUrl, "GenerateAutoNumAPI", _apiConfig, (_apiConfig.Endpoints["TranTypeParam"], "11"), (_apiConfig.Endpoints["EncashmentTypeParam"], encashmentTypeParam));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Encashment Details: {autoGen}");
                var GET_autoGen = await _httpClient.GetAsync(autoGen);
                if (GET_autoGen.IsSuccessStatusCode)
                {
                    var responseData = await GET_autoGen.Content.ReadAsStringAsync();
                    var autogen = JsonConvert.DeserializeObject<AutoGenNumberTransaction>(responseData);
                    Console.WriteLine("[DEBUG] Encashment Ref Response Content : " + responseData);

                    return new ApiResponseModel<AutoGenNumberTransaction>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Encashment Ref Number Generated!",
                        Data = autogen
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_autoGen.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<AutoGenNumberTransaction>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Generate Reference Number! {e.Message}",
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<AutoGenNumberTransaction>> GenerateAutoNumberTrasactionJ4U()
        {
            Console.WriteLine($"[DEBUG] from API Registration Encashment Details client to building url base url {_apiConfig.BaseUrl}");
            string autoGen = custom.BuildUrl(_apiBaseUrl, "GenerateAutoNumAPI", _apiConfig, (_apiConfig.Endpoints["TranTypeParam"], "22"));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Encashment Details: {autoGen}");
                var GET_autoGen = await _httpClient.GetAsync(autoGen);
                if (GET_autoGen.IsSuccessStatusCode)
                {
                    var responseData = await GET_autoGen.Content.ReadAsStringAsync();
                    var autogen = JsonConvert.DeserializeObject<AutoGenNumberTransaction>(responseData);
                    Console.WriteLine("[DEBUG] Encashment Ref Response Content : " + responseData);

                    return new ApiResponseModel<AutoGenNumberTransaction>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Encashment Ref Number Generated!",
                        Data = autogen
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_autoGen.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<AutoGenNumberTransaction>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Generate Reference Number! {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<EncashmentTypeModel>>> LoadEncashmentTypes()
        {
            Console.WriteLine($"[DEBUG] ncashment type loading client to building url base url {_apiConfig.BaseUrl}");
            string GETEncashmentTypeList = custom.BuildUrl(_apiBaseUrl, "LoadEncashmentTypeBase", _apiConfig);

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Encashment Type List : {GETEncashmentTypeList}");
                var GET_EncashmentList_response = await _httpClient.GetAsync(GETEncashmentTypeList);
                if (GET_EncashmentList_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_EncashmentList_response.Content.ReadAsStringAsync();
                    var encashmentList = JsonConvert.DeserializeObject<List<EncashmentTypeModel>>(responseData);
                    Console.WriteLine("[DEBUG] History transaction Content : " + responseData);

                    return new ApiResponseModel<List<EncashmentTypeModel>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "List of Encashments Fetched!",
                        Data = encashmentList
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_EncashmentList_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<List<EncashmentTypeModel>>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Encashment List! {e.Message}",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<GetEncashmentDetailModel>> GetEncashmentTransactionDetails(string refno,string idno)
        {
            Console.WriteLine($"[DEBUG] from API Registration History List client to building url base url {_apiConfig.BaseUrl}");
            string GETEDetailsUrl = custom.BuildUrl(_apiBaseUrl, "GetEncashmentDetailsBase", _apiConfig, (_apiConfig.Endpoints["RefNoParam"], refno), (_apiConfig.Endpoints["IdNumberParam"], idno));
            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Encashment Type List : {GETEDetailsUrl}");
                var GETEDetailsUrl_response = await _httpClient.GetAsync(GETEDetailsUrl);
                if (GETEDetailsUrl_response.IsSuccessStatusCode)
                {
                    var responseData = await GETEDetailsUrl_response.Content.ReadAsStringAsync();
                    var encashmentDetails = JsonConvert.DeserializeObject< GetEncashmentDetailModel> (responseData);
                    Console.WriteLine("[DEBUG] History transaction Content : " + responseData);

                    return new ApiResponseModel<GetEncashmentDetailModel>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Encashment Details Fetched!",
                        Data = encashmentDetails
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GETEDetailsUrl_response.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<GetEncashmentDetailModel>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Fetch Encashment Details! {e.Message}",
                    Data = null
                };
            }

        }

        //
        public async Task<ApiResponseModel<J4UWithdrawalResponse>> WithdrawJ4U(string id)
        {
            var referenceNumber = await GenerateAutoNumberTrasactionJ4U();
            Console.WriteLine($"Generated Reference Number {referenceNumber.Data.autoNum}");
            var responseData = "";
            var j4uINput = new J4UEncashModel { refno = referenceNumber.Data.autoNum, idNumber = id };

            Console.WriteLine($"[DEBUG] from Report client to building url base url {_apiConfig.BaseUrl}");

            string api_withdraw_uri = custom.BuildUrl(_apiBaseUrl, "J4UWithdrawWalletBase", _apiConfig);

            Console.WriteLine($"[DEBUG] Built Url for J4U is {api_withdraw_uri}");

            try
            {
                var jsonWithdrawData = JsonConvert.SerializeObject(j4uINput);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonWithdrawData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Console.WriteLine($"[DEBUG] JSON Data: {jsonWithdrawData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_withdraw_uri, byteContent);


                    Console.WriteLine($"[DEBUG] Response Status Code: {responseClient.StatusCode}");

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<J4UWithdrawalResponse>(responseData);
                        Console.WriteLine("[DEBUG] Withdrawal Response : " + responseData);

                        return new ApiResponseModel<J4UWithdrawalResponse>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Withdrawal Successful!",
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

                        return new ApiResponseModel<J4UWithdrawalResponse>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"{errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $" J4U Withdrawal API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Response Error on J4U EXCEPTION WITHDRAWAL");
                    Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<J4UWithdrawalResponse>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during J4U Withdrawal: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<J4UWithdrawalResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<AutoGenNumberTransaction>> LoadMemo(string encashmentTypeParam)
        {
            Console.WriteLine($"[DEBUG] from API Registration Encashment Details client to building url base url {_apiConfig.BaseUrl}");
            string autoGen = custom.BuildUrl(_apiBaseUrl, "GenerateAutoNumAPI", _apiConfig, (_apiConfig.Endpoints["TranTypeParam"], "11"), (_apiConfig.Endpoints["EncashmentTypeParam"], encashmentTypeParam));

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Encashment Details: {autoGen}");
                var GET_autoGen = await _httpClient.GetAsync(autoGen);
                if (GET_autoGen.IsSuccessStatusCode)
                {
                    var responseData = await GET_autoGen.Content.ReadAsStringAsync();
                    var autogen = JsonConvert.DeserializeObject<AutoGenNumberTransaction>(responseData);
                    Console.WriteLine("[DEBUG] Encashment Ref Response Content : " + responseData);

                    return new ApiResponseModel<AutoGenNumberTransaction>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "Encashment Ref Number Generated!",
                        Data = autogen
                    };
                }
                else
                {
                    throw new HttpRequestException(message: GET_autoGen.Content.ToString());
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<AutoGenNumberTransaction>
                {
                    IsSuccess = false,
                    StatusCode = 200,
                    Description = $"Cannot Generate Reference Number! {e.Message}",
                    Data = null
                };
            }
        }

        //










        public async Task<ApiResponseModel<DateTime>> GetDateTimeToday()
        {
            var client = new TcpClient("time.nist.gov", 13);
            try
            {
                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    var response = streamReader.ReadToEnd();
                    var utcDateTimeString = response.Substring(7, 17);
                    var utcDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

                    // Convert UTC time to Philippine Standard Time
                    TimeZoneInfo philippineTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
                    var philippineDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, philippineTimeZone);

                    Console.WriteLine($"from API -> Date today {philippineDateTime.DayOfWeek}");
                    return new ApiResponseModel<DateTime>
                    {
                        Data = philippineDateTime,
                        IsSuccess = client.Connected,
                        Description = philippineDateTime.DayOfWeek.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"from API -> Could not fetch date and time today: {ex.Message}");
                throw new Exception("Could not fetch date and time today", ex);
            }
        }


        /*
        public async Task<ApiResponseModel<UpdateBankDetailsResponse>> SelectJ4UCommission(UserUpdateBankDetailsModel updateModel)
        {
            var responseData = "";
            // Test for GitLink
            Console.WriteLine($"[DEBUG] from Report client to building url base url {_apiConfig.BaseUrl}");
            string api_registration_uri = custom.BuildUrl(_apiBaseUrl, "UpdateBankDetails", _apiConfig);
            Console.WriteLine($"[DEBUG] Built Url is {api_registration_uri}");


            try
            {
                var jsonRegData = JsonConvert.SerializeObject(updateModel);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRegData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Log the JSON data being sent
                Console.WriteLine($"[DEBUG] JSON Data for Registration: {jsonRegData}");

                try
                {
                    var responseClient = await _httpClient.PutAsync(api_registration_uri, byteContent);


                    Console.WriteLine("[DEBUG] Registration Before Condition Status Code Check : " + responseClient);

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<UpdateBankDetailsResponse>(responseData);
                        Console.WriteLine("[DEBUG] Bank Detail Update Response : " + responseData);

                        return new ApiResponseModel<UpdateBankDetailsResponse>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = " Bank Details Update was a success!",
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

                        // Log the error content and status code
                        Console.WriteLine($"Critical Error - {errorMessage}");
                        Console.WriteLine($"[DEBUG] Error Content: {errorContent}");

                        return new ApiResponseModel<UpdateBankDetailsResponse>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $" Bank Details API failed: {errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $" Bank Details API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine("[ERROR] Response Error on EXCEPTION REGISTRATION");
                    Console.WriteLine($"[DEBUG] Exception for bank update: {e.Message}");
                    return new ApiResponseModel<UpdateBankDetailsResponse>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Bank Update: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                //Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<UpdateBankDetailsResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            

        }
        */

    }
}