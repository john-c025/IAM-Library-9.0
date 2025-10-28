using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.dashboard;
using IAM_Library.models.auth;
using IAM_Library.models.dashboard;
using IAM_Library.models.general;
using IAM_Library.models.reports;
using IAM_Library.models.registration;
using IAM_Library.reports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IAM_Library.models.user;

namespace IAM_Library.registration
{
    public class RegistrationLoader(AuthApiResponseData credentials, HttpClient _httpClient,AccountDetailData accountData, VerificationInputModel verificationInput)
    {

        private static string apiBaseUrl = Encryption.decodeString(_constants.authBaseUrl);

        private RegistrationAPIClient registrationClient = new RegistrationAPIClient(apiBaseUrl, credentials, accountData,_httpClient);


        

        //Validation here
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailPattern);
        }

    public bool ValidateUsername(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }
            var userNamePattern = @"^[a-zA-Z0-9]{1,10}$";
            return Regex.IsMatch(userName, userNamePattern);
        }

        public bool ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length > 15)
            {
                return false;
            }

            return true;
        }


        
        public async Task<ApiResponseModel<VerificationMainModel>> VerifyActivation()
        {
            try
            {
                var verificationResponse = await registrationClient.VerifyActivationForDetails(verificationInput, accountData);
                return verificationResponse;
            }
            catch(Exception ex)
            {
                return new ApiResponseModel<VerificationMainModel> { IsSuccess = false, StatusCode = 500, Description = $"Error Encountered on Loader [Verification] {ex.Message}" };
            }
        }

        public async Task<ApiResponseModel<DupeUserResponse>> VerifyDuplicateUser(string username)
        {
            try
            {
                var verificationResponse = await registrationClient.CheckDupeUser(username);
                return verificationResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<DupeUserResponse> { IsSuccess = false, StatusCode = 500, Description = $"Error Encountered on Loader [Verification] {ex.Message}" };
            }
        }

        public async Task<ApiResponseModel<UplineId>> GeteUpline(string sponsor, int grp)
        {
            try
            {
                var uplineResponse = await registrationClient.GetUplineIdFunction(sponsor, grp);
                return uplineResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<UplineId> { IsSuccess = false, StatusCode = 500, Description = ex.Message };
            }
        }

        public async Task<ApiResponseModel<int>> GetExtremePosition()
        {
            //0 for left
            //1 for right
            //2 for both

            var posNumber = 0;
            try
            {
                var verifiedResponse = await VerifyActivation();
                if (verifiedResponse.IsSuccess && verifiedResponse.Data != null && verifiedResponse.Data.condition.isValidPackage)
                {
                    if (verifiedResponse.Data.condition.ext_Left)
                    {
                        posNumber = 0;
                    }
                    else if (verifiedResponse.Data.condition.ext_Left && verifiedResponse.Data.condition.ext_Right)
                    {
                        posNumber = 2;
                    }
                    else
                    {
                        posNumber = 1;
                    }
                }
                
                return new ApiResponseModel<int> { IsSuccess = false, StatusCode = 500, Description = $"User is fit for an extreme {posNumber} position", Data = posNumber };

            } catch (Exception ex)
            {
                return new ApiResponseModel<int> {  IsSuccess = false,StatusCode=500, Description = ex.Message};
            }
        }


        public async Task<ApiResponseModel<RegistrationResponse>> Register(int grpParam,UserRegistrationInputModel registerInput,string sponsorID, VerificationMainModel verifiedData,UplineId retirevedUpline)
        {
            
            try
            {
                Console.WriteLine($"Registration Function : Active no is : {verifiedData.activation.activeNo}");
                var response = new ApiResponseModel<RegistrationResponse>();
                return response = await registrationClient.RegisterUser(grpParam,registerInput.username, registerInput.password,sponsorID,registerInput.emailParam,registerInput.contactNoParam,registerInput.countryParam,registerInput.provinceParam,registerInput.cityParam,registerInput.homeAddrParam, verifiedData, retirevedUpline);
                
            }
            catch(Exception ex)
            {
                return new ApiResponseModel<RegistrationResponse> { IsSuccess=false,StatusCode=500,Description= $"Error on Loader [Registration] : {ex.Message}" };
            }
            
        }
        public async Task<ApiResponseModel<CrossLineValidationResponse>> CheckIfCrossLineValidation(string sponsorId,string uplineid)
        {
            try
            {
                var uplineResponse = await registrationClient.CheckIfCrossLine(sponsorId, uplineid);
                return uplineResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<CrossLineValidationResponse> { IsSuccess = false, StatusCode = 500, Description = ex.Message };
            }
        }
        // test git link

        // TESTED WORK HERE FOR 5.5.0-beta hotfix
        public async Task<ApiResponseModel<RegistrationResponse>> RegisterAIO(int extPosition,AccountDetailData loggedData, UserRegistrationInputModel registerInputs)
        {
            var finalRegistrationResponse = new ApiResponseModel<RegistrationResponse>();
            try
            {
                var verifiedResponse = await VerifyActivation();
                if (verifiedResponse.IsSuccess && verifiedResponse.Data != null) // && verifiedResponse.Data.condition.isValidPackage)
                {
                    ApiResponseModel<UplineId> secondLayerValidation = await GeteUpline(accountData.primaryInfo.idNumber, extPosition); // old accountData.primaryInfo.idNumber, accountData.primaryInfo.grp
                    if (secondLayerValidation.IsSuccess && secondLayerValidation.Data != null)
                    {
                        var crosslineVerification = await CheckIfCrossLineValidation(secondLayerValidation.Data.sponsorID, secondLayerValidation.Data.uplineID);
                        Console.WriteLine("From Second Layer " + secondLayerValidation.Data.uplineID);
                        if(crosslineVerification.IsSuccess && crosslineVerification.Data != null && !crosslineVerification.Data.isCrossline)
                        {

                            var checkDupeUser = await VerifyDuplicateUser(registerInputs.username);

                            if(!checkDupeUser.Data.isDuplicated){

                                var registrationResponse = await Register(extPosition, registerInputs, loggedData.primaryInfo.idNumber, verifiedResponse.Data, secondLayerValidation.Data); //pay attention to getupline, and the secondlayervalidation
                                Console.WriteLine($"AIO Third Layer Function : Active no is : {verifiedResponse.Data.activation.activeNo}");
                                if (registrationResponse.IsSuccess && registrationResponse.Data != null)
                                {
                                    finalRegistrationResponse = registrationResponse;
                                }
                                else
                                {
                                    throw new Exception(message: $"Error during Registration {registrationResponse.Description}");
                                }

                            }
                            else
                            {
                                throw new Exception(message: "Error in Registering user! username is a duplicate username");
                            }
                            


                        }
                        else
                        {
                            throw new Exception(message: $"Cannot register as user is duplicate downline : {crosslineVerification}");
                        }

                        
                    }
                    else
                    {
                        throw new Exception(message: $"Error during Upline Fetch {secondLayerValidation}");
                    }
                }
                else
                {
                    throw new Exception(message: $"Error during Activation {verifiedResponse}");
                }

                return finalRegistrationResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<RegistrationResponse> { IsSuccess = false, StatusCode = 500, Description = ex.Message };
            }


        }


        //AIO Registration
        public async Task<ApiResponseModel<RegistrationResponse>> RegisterAIOv2(int extPosition,string sponsorId,UserRegistrationInputModel registerInputs)
        {
            var finalRegistrationResponse = new ApiResponseModel<RegistrationResponse>();
            try
            {
                var verifiedResponse = await VerifyActivation();
                if (verifiedResponse.IsSuccess && verifiedResponse.Data != null && verifiedResponse.Data.condition.isValidPackage)
                {
                    var secondLayerValidation = await GeteUpline(verifiedResponse.Data.activation.idNumber, extPosition);
                    if (secondLayerValidation.IsSuccess && secondLayerValidation.Data != null)
                    {

                        var registrationResponse = await Register(extPosition,registerInputs, sponsorId, verifiedResponse.Data,secondLayerValidation.Data);
                        Console.WriteLine($"AIO Third Layer Function : Active no is : {verifiedResponse.Data.activation.activeNo}");
                        if (registrationResponse.IsSuccess && registrationResponse.Data != null)
                        {
                            finalRegistrationResponse = registrationResponse;
                        }
                        else
                        {
                            throw new Exception(message: $"Error during Registration {registrationResponse.Description}");
                        }
                    }
                    else
                    {
                        throw new Exception(message: $"Error during Upline Fetch {secondLayerValidation}");
                    }
                }
                else
                {
                    throw new Exception(message: $"Error during Activation {verifiedResponse}");
                }
                return finalRegistrationResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<RegistrationResponse> { IsSuccess = false, StatusCode = 500, Description = ex.Message };
            }


        }

        // Update user details

        public async Task<ApiResponseModel<UpdatePrimaryDetailsResponse>> UpdatePrimaryDetailsLoader(UserUpdatePrimaryDetailsModel updateModel)
        {
            try
            {
                Console.WriteLine($"UpdatePrimaryDetails Function : Updating primary details for user ID: {updateModel.idNumber}");
                var response = new ApiResponseModel<UpdatePrimaryDetailsResponse>();
                return response = await registrationClient.UpdatePrimaryDetails(updateModel);
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<UpdatePrimaryDetailsResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error on Loader [UpdatePrimaryDetails] : {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseModel<UpdateBankDetailsResponse>> UpdateBankDetailsLoader(UserUpdateBankDetailsModel updateModel)
        {
            try
            {
                Console.WriteLine($"UpdateBankDetails Function: Updating bank details for user ID: {updateModel.idNumber}");
                var response = new ApiResponseModel<UpdateBankDetailsResponse>();
                return response = await registrationClient.UpdateBankDetails(updateModel);
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<UpdateBankDetailsResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error on Loader [UpdateBankDetails] : {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseModel<List<BankDetail>>> LoadBankDetailsList()
        {
            try
            {
                var bankDetailsResponse = await registrationClient.LoadBankDetails();
                return bankDetailsResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<BankDetail>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }














    }
}
