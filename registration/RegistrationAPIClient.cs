using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using IAM_Library.models.dashboard;
using IAM_Library.models.geneology;
using IAM_Library.models.general;
using IAM_Library.models.registration;
using IAM_Library.models.reports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.registration
{
    public class RegistrationAPIClient
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

            {"RegistrationBase",Encryption.decodeString(_constants.RegistrationBase) },
            { "verificationActivation", Encryption.decodeString(_constants.verificationActivation) },
            { "getExtremeUpline", Encryption.decodeString(_constants.getextremeupline) },
            {"checkCrossLineBase",Encryption.decodeString(_constants.checkCrosslineBase) },
            { "sponsorIdParam", Encryption.decodeString(_constants.sponsorIdParam) },
            { "activeNoParam", Encryption.decodeString(_constants.activeNoParam) },
            { "pinNoParam", Encryption.decodeString(_constants.pinNoParam) },
            { "grpParam", Encryption.decodeString(_constants.grpParam) },
            { "uplineParam", Encryption.decodeString(_constants.uplineIdParam) },
            //Update Profile
            { "UpdatePrimaryDetails", Encryption.decodeString(_constants.PUTUpdatePrimaryDetails) },
            { "UpdateBankDetails", Encryption.decodeString(_constants.PUTUpdateBankDetails) },
            { "UploadID", Encryption.decodeString(_constants.POSTUploadID) },
            //load bank 
            { "LoadBankList", Encryption.decodeString(_constants.GETLoadBankList) },
            { "CheckDuplicateUsername", Encryption.decodeString(_constants.checkDuplicateUserName) },
            { "UserNameParam", Encryption.decodeString(_constants.usernameParam) },



        };

        public RegistrationAPIClient(string apiBaseUrl, AuthApiResponseData accessCredentials, AccountDetailData loggedAccData, HttpClient httpClient) // constructor
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

        public async Task<ApiResponseModel<VerificationMainModel>> VerifyActivationForDetails(VerificationInputModel verificationData, AccountDetailData loggedAccData)
        {
            
            Console.WriteLine($"[DEBUG] from API Verification client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "verificationActivation", _apiConfig, (_apiConfig.Endpoints["sponsorIdParam"], loggedAccData.primaryInfo.idNumber), (_apiConfig.Endpoints["activeNoParam"], verificationData.activeno), (_apiConfig.Endpoints["pinNoParam"], verificationData.pinno));
            Console.WriteLine($"Verification URL : {api_verification_activation}");
            try
            {
               var verification_response = await _httpClient.GetAsync(api_verification_activation);
                if (verification_response.IsSuccessStatusCode)
                {
                    var responseData = await verification_response.Content.ReadAsStringAsync();
                    var verification_response_asJson = JsonConvert.DeserializeObject<VerificationMainModel>(responseData);
                    Console.WriteLine("[DEBUG] Verification Response Content : " + responseData);
                    
                    return new ApiResponseModel<VerificationMainModel> { IsSuccess = true, StatusCode = 200, Description = $"Verification was a Success!", Data = verification_response_asJson };

                }
                else
                {
                    
                    string errorContent = await verification_response.Content.ReadAsStringAsync();
                    string errorMessage;

                        try
                        {
                            Console.WriteLine($"[DEBUG] Error encountered : Building JSON Error Model : {errorContent}");
                            var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                            errorMessage = errorJson?.Message ?? "An error occurred.";

                            return new ApiResponseModel<VerificationMainModel>
                            {
                                IsSuccess = false,
                                StatusCode = (int)verification_response.StatusCode,
                                Description = $"{errorContent}",
                                Data = null
                            };
                        }
                        catch (JsonException)
                        {
                        
                            errorMessage = errorContent;
                            Console.WriteLine($"{errorMessage}");
                            throw new Exception($"{errorMessage}");

                          
                        }

                        //Console.WriteLine($"Critical Error in Verification : {errorMessage}");

                        return new ApiResponseModel<VerificationMainModel>
                        {
                            IsSuccess = false,
                            StatusCode = (int)verification_response.StatusCode,
                            Description = $"Verification failed: {errorMessage}",
                            Data = null
                        };
                }
            }
            catch(Exception e)
            {
                return new ApiResponseModel<VerificationMainModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Verification was not successful! {e.Message}",
                    Data = null
                };

            }
        }


        public static bool isExtremeRight(VerificationMainModel data)
        {

            if(data.condition.ext_Left == false && data.condition.ext_Right == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<ApiResponseModel<UplineId>> GetUplineIdFunction(String sponsorIDParam, int grpParam)
        {
            //Console.WriteLine($"[DEBUG] from API Registration Upline client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "getExtremeUpline", _apiConfig, (_apiConfig.Endpoints["sponsorIdParam"], sponsorIDParam), (_apiConfig.Endpoints["grpParam"], grpParam.ToString()));
            {
                try
                {


                    //Console.WriteLine($"[DEBUG] API Built for Get Upline : {api_verification_activation}");
                    var verification_response = await _httpClient.GetAsync(api_verification_activation);
                    if (verification_response.IsSuccessStatusCode)
                    {
                        var responseData = await verification_response.Content.ReadAsStringAsync();
                        var verification_response_asJson = JsonConvert.DeserializeObject<UplineId>(responseData);
                        //Console.WriteLine("[DEBUG] Upline Response Content : " + responseData);

                        return new ApiResponseModel<UplineId> { IsSuccess = true, StatusCode = 200, Description = "Upline ID Fetched!", Data = verification_response_asJson };

                    }
                    else
                    {
                        throw new HttpRequestException(message: verification_response.Content.ToString());
                    }
                }
                catch (Exception e)
                {
                    return new ApiResponseModel<UplineId> { IsSuccess = false, StatusCode = 200, Description = $"Cannot Fetch UplineID! {e.Message}", Data = null };


                }

            }
        }



        public async Task<ApiResponseModel<DupeUserResponse>> CheckDupeUser(String username)
        {
            //Console.WriteLine($"[DEBUG] from API Registration Upline client to building url base url {_apiConfig.BaseUrl}");
            string api_verification_activation = custom.BuildUrl(_apiBaseUrl, "CheckDuplicateUsername", _apiConfig, (_apiConfig.Endpoints["UserNameParam"], username));
            {
                try
                {


                    //Console.WriteLine($"[DEBUG] API Built for Get Upline : {api_verification_activation}");
                    var verification_response = await _httpClient.GetAsync(api_verification_activation);
                    if (verification_response.IsSuccessStatusCode)
                    {
                        var responseData = await verification_response.Content.ReadAsStringAsync();
                        var verification_response_asJson = JsonConvert.DeserializeObject<DupeUserResponse>(responseData);
                        //Console.WriteLine("[DEBUG] Upline Response Content : " + responseData);

                        return new ApiResponseModel<DupeUserResponse> { IsSuccess = true, StatusCode = 200, Description = "Upline ID Fetched!", Data = verification_response_asJson };

                    }
                    else
                    {
                        throw new HttpRequestException(message: verification_response.Content.ToString());
                    }
                }
                catch (Exception e)
                {
                    return new ApiResponseModel<DupeUserResponse> { IsSuccess = false, StatusCode = 200, Description = $"Cannot Fetch UplineID! {e.Message}", Data = null };


                }

            }
        }

        public async Task<ApiResponseModel<RegistrationResponse>> RegisterUser2(int grpParam,string user,string pw,string emailParam,string contactNoParam,string countryParam,string provinceParam,string cityParam,string? homeAddressParam, VerificationMainModel verifiedData, UplineId retrievedUpline)
        {
            var responseData = "";
            //Console.WriteLine($"[DEBUG] from Report client to building url base url {_apiConfig.BaseUrl}");
            string api_registration_uri = custom.BuildUrl(_apiBaseUrl, "RegistrationBase", _apiConfig);
            //Console.WriteLine($"[DEBUG] Built Url is {api_registration_uri}");

            var builtRegistration = new RegistrationModel {  refno = verifiedData.activation.refno, pinNo = verifiedData.activation.pinNo, idNumber = verifiedData.activation.idNumber,username=user,password=pw, email = emailParam, contactNo = contactNoParam, sponsorID = retrievedUpline.sponsorID, uplineID = retrievedUpline.uplineID, grp = grpParam, country = countryParam, province = provinceParam, city = cityParam, homeAddress = homeAddressParam };

            try
            {
                var jsonRegData = JsonConvert.SerializeObject(builtRegistration);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRegData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //Console.WriteLine($"Byte Content is : {buffer}");

                try
                {
                    
                    var responseClient = await _httpClient.PostAsJsonAsync(api_registration_uri, byteContent);

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<RegistrationResponse>(responseData);
                        //Console.WriteLine("[DEBUG] Registration Response : " + responseData);

                        return new ApiResponseModel<RegistrationResponse> { IsSuccess = true, StatusCode = 200, Description = "Upline ID Fetched!", Data = deserializedResponse };
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
                            //Console.WriteLine($"Exception Error on Parsing Error Content - {errorMessage}");
                        }
                        //Console.WriteLine($"Critical Error - {errorMessage}");

                        return new ApiResponseModel<RegistrationResponse>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"Registration API failed: {errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Registration API failed: { errorMessage } : {responseClient.RequestMessage} ");
                    }
                }
                catch(Exception e)
                {
                    //Console.WriteLine("[ERORR] Response Error on EXCEPTION REGISTRATION");
                    return new ApiResponseModel<RegistrationResponse> { IsSuccess = false,StatusCode=500, Description=$"Error Encountered during Registration : {e.Message}",Data=null };
                }

                

            }
            catch (HttpRequestException ex)
            {
                return null;
            }

        }
        public async Task<ApiResponseModel<CrossLineValidationResponse>> CheckIfCrossLine(string sponsorIdParam,string uplineIdParam)
        {
            //Console.WriteLine($"[DEBUG] from API CrosslineCheck client to building url base url {_apiConfig.BaseUrl}");
            string api_crossline = custom.BuildUrl(_apiBaseUrl, "checkCrossLineBase", _apiConfig, (_apiConfig.Endpoints["sponsorIdParam"], sponsorIdParam), (_apiConfig.Endpoints["uplineParam"], uplineIdParam));
            //Console.WriteLine($"Verification URL : {api_crossline}");
            try
            {
                var crossline_check = await _httpClient.GetAsync(api_crossline);
                if (crossline_check.IsSuccessStatusCode)
                {
                    var responseData = await crossline_check.Content.ReadAsStringAsync();
                    var crossline_check_asJson = JsonConvert.DeserializeObject<CrossLineValidationResponse>(responseData);
                    //Console.WriteLine("[DEBUG] Crossline Validation Response Content : " + responseData);

                    return new ApiResponseModel<CrossLineValidationResponse> { IsSuccess = true, StatusCode = 200, Description = $"Verification was a Success!", Data = crossline_check_asJson };

                }
                else
                {



                    string errorContent = await crossline_check.Content.ReadAsStringAsync();
                    string errorMessage;

                    try
                    {

                        var errorJson = JsonConvert.DeserializeObject<ErrorModel>(errorContent);
                        errorMessage = errorJson?.Message ?? "An error occurred during registration, user is a duplicate downline";
                    }
                    catch (JsonException)
                    {

                        errorMessage = errorContent;
                    }
                    //Console.WriteLine($"Critical Error: {errorMessage}");

                    return new ApiResponseModel<CrossLineValidationResponse>
                    {
                        IsSuccess = false,
                        StatusCode = (int)crossline_check.StatusCode,
                        Description = $"Crossline Validation failed: {errorMessage}",
                        Data = null
                    };
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<CrossLineValidationResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Crossline Validation was not successful! {e.Message}",
                    Data = null
                };

            }
        }


       

        public async Task<ApiResponseModel<RegistrationResponse>> RegisterUser(
            int grpParam,
            string user,
            string pw,
            string sponsorId,
            string? emailParam,
            string? contactNoParam,
            string? countryParam,
            string? provinceParam,
            string? cityParam,
            string? homeAddressParam,
            VerificationMainModel verifiedData,
            UplineId retrievedUpline)
        {
            var responseData = "";
            // Test for GitLink
            //Console.WriteLine($"[DEBUG] from Report client to building url base url {_apiConfig.BaseUrl}");
            string api_registration_uri = custom.BuildUrl(_apiBaseUrl, "RegistrationBase", _apiConfig);
            //Console.WriteLine($"[DEBUG] Built Url is {api_registration_uri}");

            string temp = custom.temporaryUserGenerator();

            var builtRegistration = new RegistrationModel
            {
                refno = verifiedData.activation.refno,
                activeNo=verifiedData.activation.activeNo,
                pinNo = verifiedData.activation.pinNo,
                idNumber = verifiedData.activation.idNumber,
                username = user,
                password = pw,
                email = $"{verifiedData.activation.idNumber}@gmail.com", //temp
                contactNo = "09191011691", //temp
                sponsorID = sponsorId,
                uplineID = retrievedUpline.uplineID,
                grp = grpParam,
                country = countryParam,
                province = provinceParam,
                city = cityParam,
                homeAddress = homeAddressParam,
                fname =temp,
                mname = temp,
                sname=temp
            };

            try
            {
                var jsonRegData = JsonConvert.SerializeObject(builtRegistration);
                var buffer = System.Text.Encoding.UTF8.GetBytes(jsonRegData);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Log the JSON data being sent
                Console.WriteLine($"[DEBUG] JSON Data for Registration: {jsonRegData}");

                try
                {
                    var responseClient = await _httpClient.PostAsync(api_registration_uri, byteContent);

               
                    Console.WriteLine("[DEBUG] Registration Before Condition Status Code Check : " + responseClient);

                    if (responseClient.IsSuccessStatusCode)
                    {
                        responseData = await responseClient.Content.ReadAsStringAsync();
                        var deserializedResponse = JsonConvert.DeserializeObject<RegistrationResponse>(responseData);
                        Console.WriteLine("[DEBUG] Registration Response : " + responseData);

                        return new ApiResponseModel<RegistrationResponse>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Registration was a success!",
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

                        return new ApiResponseModel<RegistrationResponse>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"Registration API failed: {errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Registration API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine("[ERROR] Response Error on EXCEPTION REGISTRATION");
                    //Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<RegistrationResponse>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Registration: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                //Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<RegistrationResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }








        public async Task<ApiResponseModel<UpdatePrimaryDetailsResponse>> UpdatePrimaryDetails(UserUpdatePrimaryDetailsModel updateModel)
        {
            var responseData = "";
            // Test for GitLink
            Console.WriteLine($"[DEBUG] from Update to building url base url {_apiConfig.BaseUrl}");
            string api_registration_uri = custom.BuildUrl(_apiBaseUrl, "UpdatePrimaryDetails", _apiConfig);
            //Console.WriteLine($"[DEBUG] Built Url is {api_registration_uri}");


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
                        var deserializedResponse = JsonConvert.DeserializeObject<UpdatePrimaryDetailsResponse>(responseData);
                        Console.WriteLine("[DEBUG] Primary Details Update Response : " + responseData);

                        return new ApiResponseModel<UpdatePrimaryDetailsResponse>
                        {
                            IsSuccess = true,
                            StatusCode = 200,
                            Description = "Primary Details Update was a success!",
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

                        return new ApiResponseModel<UpdatePrimaryDetailsResponse>
                        {
                            IsSuccess = false,
                            StatusCode = (int)responseClient.StatusCode,
                            Description = $"Primary Details API failed: {errorMessage}",
                            Data = null
                        };

                        throw new HttpRequestException(message: $"Update Primary Details API failed: {errorMessage} : {responseClient.RequestMessage}");
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine("[ERROR] Response Error on EXCEPTION REGISTRATION");
                    //Console.WriteLine($"[DEBUG] Exception: {e.Message}");
                    return new ApiResponseModel<UpdatePrimaryDetailsResponse>
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"Error Encountered during Primary Detail Update: {e.Message}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                //Console.WriteLine("[ERROR] HttpRequestException: " + ex.Message);
                return new ApiResponseModel<UpdatePrimaryDetailsResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = "HttpRequestException occurred.",
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<UpdateBankDetailsResponse>> UpdateBankDetails(UserUpdateBankDetailsModel updateModel)
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
        }

        public async Task<ApiResponseModel<List<BankDetail>>> LoadBankDetails()
        {
            Console.WriteLine($"[DEBUG] Bank details loading client to building URL base URL {_apiConfig.BaseUrl}");
            string GETBankDetailsList = custom.BuildUrl(_apiBaseUrl, "LoadBankList", _apiConfig);

            try
            {
                Console.WriteLine($"[DEBUG] API Built for Get Bank Details List: {GETBankDetailsList}");
                var GET_BankDetailsList_response = await _httpClient.GetAsync(GETBankDetailsList);
                if (GET_BankDetailsList_response.IsSuccessStatusCode)
                {
                    var responseData = await GET_BankDetailsList_response.Content.ReadAsStringAsync();
                    var bankDetailsList = JsonConvert.DeserializeObject<List<BankDetail>>(responseData);
                    Console.WriteLine("[DEBUG] Bank details response content: " + responseData);

                    return new ApiResponseModel<List<BankDetail>>
                    {
                        IsSuccess = true,
                        StatusCode = 200,
                        Description = "List of Banks Fetched!",
                        Data = bankDetailsList
                    };
                }
                else
                {
                    string errorContent = await GET_BankDetailsList_response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error fetching bank details: {errorContent}");
                }
            }
            catch (Exception e)
            {
                return new ApiResponseModel<List<BankDetail>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Cannot Fetch Bank Details List! {e.Message}",
                    Data = null
                };
            }
        }



    }




}
