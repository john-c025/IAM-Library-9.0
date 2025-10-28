

using IAM_Library;
using System.Reflection.Metadata;
using Newtonsoft.Json;
using IAM_Library.models.auth;
using IAM_Library.models.general;
using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.auth;
using System.Security.Authentication;
using IAM_Library.appWallet.api;
using IAM_Library.appWallet.models;

namespace IAM_Library.appWallet.auth
{
    public class AuthWalletClientLoader
    {
        Encryption encryption = new Encryption();
        public static CredentialModel AuthEncode(string username, string password) => new CredentialModel
            (Encryption.encodeString(username), Encryption.encodeString(password));

        // Authenticate(AuthEncode(mainhead00,batman));
        //remove <string> type

        /*
        public async Task<AuthApiResponseData> Authenticate(CredentialModel creds, HttpClient httpClient)

        {
            AuthApiResponseData authResponse;
            ApiResponseModel<object> responseFromApi;

            String my_dec_usr = Encryption.decodeString(creds.usrname); //base64 encoding lang to
            String my_dec_pw = Encryption.decodeString(creds.password); ///base64 encoding lang to

            //TEMPORRY - WAIT FOR ENCRYPTION SCHEME//
            //encode to sir's encoding scheme before passin to authentication 
            String oCodedUsername = Encryption.MD5Hash(my_dec_usr); //replace this with -> encode with sir angelo's encryption
            String oCodedPassword = Encryption.MD5Hash(my_dec_pw); ; //replace this with -> encode with sir angelo's encryption

            //TEST 1
            //Console.WriteLine($"Auth: {my_dec_usr}:{oCodedUsername} AND {my_dec_pw}:{oCodedPassword}");


            var apiBaseUrl = $"{Encryption.decodeString(_constants.authBaseUrl)}";



            //objet creation
            var auth_apiClient = new ApiClient(apiBaseUrl + Encryption.decodeString(_wallet_endpoints.walletAuthtenticate), httpClient);



            //TEST 3
            Console.WriteLine($"library console: {auth_apiClient}");
            try
            {



                //replace with encoded username and encoded password yung dalawang params
                authResponse = await auth_apiClient.AuthenticateAsync(my_dec_usr, oCodedUsername, oCodedPassword); //test inject encoded creds


                if (authResponse.signature != "null" || authResponse.signature != "invalid user")
                {
                    responseFromApi = new ApiResponseModel<object> { IsSuccess = true, StatusCode = 200, Description = "auth success!", Data = "Authentication was successful" };


                    //Console.WriteLine($"Repsonse Code: {responseFromApi.StatusCode}");
                    //Console.WriteLine($"Token Signature Expiry: {encryption.GetExpiryTimestamp(authResponse.signature)}");

                    //return responseFromApi; //Return Response Status, with access bool

                    return new AuthApiResponseData { accountKey = authResponse.accountKey, signature = authResponse.signature };
                }

            }
            catch (Exception e)
            {
                authResponse = new AuthApiResponseData { accountKey = "null", signature = "null" };//test inject encoded creds
                                                                                                   //TEST 5
                responseFromApi = new ApiResponseModel<object> { IsSuccess = false, StatusCode = 500, Description = "auth failed!", Data = "Authentication has failed" };
                Console.WriteLine($"Error from lib: {authResponse.signature} {e.ToString()}");


            }

            //TEST 6
            //Console.WriteLine($"{apiBaseUrl} is the API Url and Response is {authResponse.signature}");
            // if(auth_apiClient.

            //return responseFromApi
            return new AuthApiResponseData { accountKey = Encryption.decodeString(_constants.invalidIndicator), signature = Encryption.decodeString(_constants.invalidIndicator) };

        }
        // Auth Response:


        

        public async Task<ApiResponseModel<AuthApiResponseData>> AuthenticateWithResponse(CredentialModel credentials, HttpClient httpClient)
        {
            AuthApiResponseData authResponse;
            ApiResponseModel<AuthApiResponseData> responseFromApi = null;

            String my_dec_usr = Encryption.decodeString(credentials.usrname); //base64 encoding lang to
            String my_dec_pw = Encryption.decodeString(credentials.password); ///base64 encoding lang to

            //TEMPORRY - WAIT FOR ENCRYPTION SCHEME//
            //encode to sir's encoding scheme before passin to authentication 
            String oCodedUsername = Encryption.encodeString(my_dec_usr); //replace this with -> encode with sir angelo's encryption
            String oCodedPassword = Encryption.encodeString(my_dec_pw); ; //replace this with -> encode with sir angelo's encryption

            //TEST 1
            Console.WriteLine($"Auth: {my_dec_usr}:{oCodedUsername} AND {my_dec_pw}:{oCodedPassword}");


            var apiBaseUrl = $"{Encryption.decodeString(_constants.authBaseUrl)}";

            //objet creation
            var auth_apiClient = new ApiClient(apiBaseUrl + Encryption.decodeString(_constants.authSigGenEndpoint), httpClient);

            //TEST 3
            Console.WriteLine($"library console: {auth_apiClient}");
            try
            {

                //replace with encoded username and encoded password yung dalawang params
                authResponse = await auth_apiClient.AuthenticateAsync(my_dec_usr, "6edd6743a3e381f4ed7e88107966ec71", "25d55ad283aa400af464c76d713c07ad"); //test inject encoded creds


                if (authResponse.signature != "null" || authResponse.signature != "invalid user")
                {
                    Console.WriteLine("TEST FOR GETTING SIG AT AUTH LIBRARY: " + authResponse.signature);
                    Console.WriteLine($"Token Signature Expiry: {encryption.GetExpiryTimestamp(authResponse.signature)}");
                    responseFromApi = new ApiResponseModel<AuthApiResponseData> { IsSuccess = true, StatusCode = 200, Description = $"user with sig {authResponse.signature}, Login was a Success! Login will Expire at {encryption.GetExpiryTimestamp(authResponse.signature)}", Data = authResponse };
                    Console.WriteLine($"Repsonse Code: {responseFromApi.StatusCode}");


                }

            }
            catch (Exception e)
            {

                responseFromApi = new ApiResponseModel<AuthApiResponseData> { IsSuccess = false, StatusCode = 401, Description = "Auth failed!", Data = null };



            }
            return responseFromApi;
        }
        */





        public async Task<ApiResponseModel<object>> GetAuthResponse(AuthApiResponseData authResponse, CredentialModel cred)
        {
            var response = _defaults_wal.defaultApiResponse;

            var invalidIndicator = Encryption.decodeString(_constants.invalidIndicator); // invalid indicator


            if (authResponse.accountKey == invalidIndicator && authResponse.signature == invalidIndicator)
            {
                response = new ApiResponseModel<object> { IsSuccess = false, StatusCode = 500, Description = "Authentication failed!" };

            }
            else
            {
                response = new ApiResponseModel<object> { IsSuccess = true, StatusCode = 200, Description = $"AuthResponse Method: {authResponse.signature} Login was a Success! Login will Expire at {encryption.GetExpiryTimestamp(authResponse.signature)}" };


            }

            return response;



        }
		public async Task<AuthResponseWalletModel> WalletAuthenticateAPI(CredentialModel credentials, HttpClient httpClient, DeviceDetails deviceData)
		{
			AuthResponseWalletModel authResponse = new();
			ApiResponseModel<object> responseFromApi = new();

			string my_dec_usr = Encryption.decodeString(credentials.usrname); // Decoded username (base64)
			string my_dec_pw = Encryption.decodeString(credentials.password); // Decoded password (base64)
			string oCodedUsername = Encryption.MD5Hash(my_dec_usr); // MD5 hashed username
			string oCodedPassword = Encryption.MD5Hash(my_dec_pw); // MD5 hashed password

			Console.WriteLine($"Auth: {my_dec_usr}:{oCodedUsername} AND {my_dec_pw}:{oCodedPassword}");
			var apiBaseUrl = $"{Encryption.decodeString(_wallet_endpoints.baseUrlWallet)}";
			Console.WriteLine($"Auth URL Wallet = {apiBaseUrl + Encryption.decodeString(_wallet_endpoints.walletAuthtenticate)}");

			var authApiClient = new ApiClient(apiBaseUrl + Encryption.decodeString(_wallet_endpoints.walletAuthtenticate), httpClient);

			Console.WriteLine($"Library console: {authApiClient}");
			try
			{
				// Call AuthenticateAsyncWallet
				var authenticationValue = await authApiClient.AuthenticateAsyncWallet(my_dec_usr, oCodedUsername, oCodedPassword, deviceData);

				Console.WriteLine($"AUTH LOADER WALLET - Signature is: {authenticationValue.signature}");
				Console.WriteLine($"AUTH LOADER WALLET - Session ID is: {authenticationValue.sesssionID}");

				// Determine success or failure
				if (authenticationValue.signature != "invalid_user" && authenticationValue.signature != "invalid_response_format")
				{
					Console.WriteLine("Authentication successful.");
					responseFromApi = new ApiResponseModel<object>
					{
						IsSuccess = true,
						StatusCode = 200,
						Description = $"Successfully signed-in! Login will expire at {encryption.GetExpiryTimestamp(authenticationValue.signature)}",
						Data = "Success",
						MiscData = "Success"
					};
				}
				else
				{
					Console.WriteLine("Authentication failed.");
					responseFromApi = new ApiResponseModel<object>
					{
						IsSuccess = false,
						StatusCode = 400,
						Description = $"Login failed: {authenticationValue.signature}", // Pass error message
						Data = "Failed",
						MiscData = "Failed"
					};
				}

				authResponse = new AuthResponseWalletModel
				{
					WalletAuthData = authenticationValue,
					apiResponse = responseFromApi
				};
				Console.WriteLine($"Auth Response: {responseFromApi}");
				return authResponse;
			}
			catch (Exception e)
			{
				Console.WriteLine("Error on loader: " + e.Message);
				authResponse = new AuthResponseWalletModel
				{
					WalletAuthData = null,
					apiResponse = new ApiResponseModel<object>
					{
						IsSuccess = false,
						StatusCode = 500,
						Description = $"Authentication failed with exception: {e.Message}",
						Data = "Authentication has failed"
					}
				};
				Console.WriteLine("Error in API Client for wallet auth: " + e.Message);
				return authResponse;
			}
		}



		/*
        public async Task<AuthResponseWalletModel> WalletAuthenticateAPI(CredentialModel credentials, HttpClient httpClient, DeviceDetails deviceData)
        {
            AuthResponseWalletModel authResponse = new();
            ApiResponseModel<object> responseFromApi = new();

            String my_dec_usr = Encryption.decodeString(credentials.usrname); //base64 encoding lang to
            String my_dec_pw = Encryption.decodeString(credentials.password); ///base64 encoding lang to
            String oCodedUsername = Encryption.MD5Hash(my_dec_usr);
            String oCodedPassword = Encryption.MD5Hash(my_dec_pw); ;
            

            Console.WriteLine($"Auth: {my_dec_usr}:{oCodedUsername} AND {my_dec_pw}:{oCodedPassword}");
            var apiBaseUrl = $"{Encryption.decodeString(_wallet_endpoints.baseUrlWallet)}";
            Console.WriteLine($"Auth URL Wallet = {apiBaseUrl + Encryption.decodeString(_wallet_endpoints.walletAuthtenticate)}");

            //objet creation
            var auth_apiClient = new ApiClient(apiBaseUrl + Encryption.decodeString(_wallet_endpoints.walletAuthtenticate), httpClient);

            Console.WriteLine($"library console: {auth_apiClient}");
            try
            {
                var authenticationValue = await auth_apiClient.AuthenticateAsyncWallet(my_dec_usr, oCodedUsername, oCodedPassword, deviceData);

                Console.WriteLine($"AUTH LOADER WALLET - Signature is: {authenticationValue.signature} ");
                Console.WriteLine($"AUTH LOADER WALLET - Session ID is: {authenticationValue.sesssionID}");

                if (authenticationValue.signature != "invalid_user")
                {
                    Console.WriteLine("Passed 1");
                    // INSPECT HERE TOM! ---- [1]
                    responseFromApi = new ApiResponseModel<object> { IsSuccess = true, StatusCode = 200, Description = $"Successfully signed-in!, Login will Expire at {encryption.GetExpiryTimestamp(authenticationValue.signature)}",Data="Success",MiscData="Success"};
                    
                }
                else
                {
                    Console.WriteLine("Failed 1");
                    responseFromApi = new ApiResponseModel<object> { IsSuccess = false, StatusCode = 500, Description = $"Login has failed! Please Retry", Data = "Failed", MiscData = "Failed" };
                   
                }

                authResponse = new AuthResponseWalletModel { WalletAuthData = authenticationValue, apiResponse = responseFromApi };//test inject encoded creds
                Console.WriteLine($"Auth Response " + responseFromApi);
                return authResponse;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error on loader! : " + e.Message );
                authResponse = new AuthResponseWalletModel
                {
                    WalletAuthData = null,
                    apiResponse = new ApiResponseModel<object> // Initialize apiResponse here
                    {
                        IsSuccess = false,
                        StatusCode = 500,
                        Description = $"auth failed! {e.Message}",
                        Data = "Authentication has failed"
                    }
                };
                Console.WriteLine("Error API Client for wallet auth " + e.Message);
                return authResponse;
            }
          
           
        }
        */
	}
}