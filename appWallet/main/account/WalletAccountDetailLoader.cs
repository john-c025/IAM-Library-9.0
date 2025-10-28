using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using IAM_Library.models.dashboard;
using IAM_Library.dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using IAM_Library.appWallet.models.wallet;
using IAM_Library.appWallet.models.dashboard;
using IAM_Library.appWallet.models;
using IAM_Library.appWallet.api;
using IAM_Library.auth;
using IAM_Library.models.general;
using IAM_Library.models.wallet;
using System.Net.Http;
using IAM_Library.models.registration;
using IAM_Library.appWallet.main.account;
namespace IAM_Library.appWallet.account
{


    public class WalletAccountDetailLoader() //test WIP
    {
        WalletAccountDetailData getAccountData;
        WalletAccountsApiClient walletClient;
        public async Task<WalletAccountDetailData> LoadAccountData(WalletAuthResponseData credentials, HttpClient _httpClient)
        {
            var apiBaseUrl = $"{Encryption.decodeString(_wallet_endpoints.baseUrlWallet)}";

            WalletAuthResponseData myCreds = credentials;
            //var accountAPIClient = new WalletAccountsApiClient(apiBaseUrl, myCreds.signature, _httpClient);
            var walletClient = new WalletAccountsApiClient(apiBaseUrl, myCreds, _httpClient);
            try
            {
                getAccountData = await walletClient.GetAccountDataAsync(credentials.accountKey); //test inject encoded creds
            }
            catch (Exception e)
            {
                getAccountData = _defaults_wal.invalidWalletAccount;
            }
            return getAccountData;


        }


        public async Task<WalletBalanceModel> LoadWalletBalance(WalletAuthResponseData credentials, HttpClient _httpClient, string accountId)
        {
            var apiBaseUrl = $"{Encryption.decodeString(_wallet_endpoints.baseUrlWallet)}";
            WalletBalanceModel balance = new();
            WalletAuthResponseData myCreds = credentials;
            //var accountAPIClient = new WalletAccountsApiClient(apiBaseUrl, myCreds.signature, _httpClient);
            var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials,_httpClient);
            try
            {
                balance = await accountAPIClient.LoadAccountBalance(accountId); //test inject encoded creds
            }
            catch (Exception e)
            {
                balance = _defaults_wal.invalidBalance;
            }
            return balance;
        }


        public async Task<ApiResponseModel<List<WalletTransactionsModel>>> LoadWalletTransactionsList(WalletAuthResponseData credentials, HttpClient _httpClient, string accountId)
        {
            var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
            try
            {
                Console.WriteLine("Running Loader for List Trasactions");
                var historyResponse = await accountAPIClient.LoadWalletTransactionsList(accountId);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<WalletTransactionsModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<MainCashoutResponseModel>> SendPOSTCashoutTransaction(WalletAuthResponseData credentials, HttpClient _httpClient, POSTCashoutModel postData){

            var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
            try
            {
                var withdrawalResponse = await accountAPIClient.POSTCashoutTransaction(postData);
                return withdrawalResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<MainCashoutResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<MainLoadResponse>> SendPOSTLoadingTransaction(WalletAuthResponseData credentials, HttpClient _httpClient, SendLoadPOSTModel postData)
        {

            var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
            try
            {
                var withdrawalResponse = await accountAPIClient.POSTSendLoad(postData);
                return withdrawalResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<MainLoadResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<Generated2FAResponseModel>> Send2FA(WalletAuthResponseData credentials, HttpClient _httpClient, POST2FAModel postData)
		{

			var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
			try
			{
				var withdrawalResponse = await accountAPIClient.Generate2FA(postData);
				return withdrawalResponse;
			}
			catch (Exception ex)
			{
				return new ApiResponseModel<Generated2FAResponseModel>
				{
					IsSuccess = false,
					StatusCode = 500,
					Description = ex.Message,
					Data = null
				};
			}
		}

        public async Task<ApiResponseModel<Generated2FAResponseModel>> Verify2FA(WalletAuthResponseData credentials, HttpClient _httpClient, POSTVerify2FAModel postData)
        {

            var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
            try
            {
                var withdrawalResponse = await accountAPIClient.Verify2FA(postData);
                return withdrawalResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<Generated2FAResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<LogoutPOSTResponse>> LogoutUserAysnc(WalletAuthResponseData credentials, HttpClient _httpClient, LogoutPOSTModel postData)
        {

            var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
            try
            {
                var withdrawalResponse = await accountAPIClient.LogoutUser(postData);
                return withdrawalResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<LogoutPOSTResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<List<BankCashoutList>>> LoadBankCashoutList(WalletAuthResponseData credentials, HttpClient _httpClient)
        {
            try
            {
                var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
                
                var historyResponse = await accountAPIClient.LoadBankCashoutList();
                Console.WriteLine("Data from Bank list: " + historyResponse.Data.Count);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<BankCashoutList>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<TelcoModel>>> LoadTelcoList(WalletAuthResponseData credentials, HttpClient _httpClient)
        {
            try
            {
                var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);

                var historyResponse = await accountAPIClient.LoadNetworks();
                Console.WriteLine("Data from Telco list: " + historyResponse.Data.Count);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<TelcoModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<TelcoProduct>>> LoadPromoList(WalletAuthResponseData credentials, HttpClient _httpClient,string telco)
        {
            try
            {
                var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);

                var historyResponse = await accountAPIClient.LoadPromoByNetwork(telco);
                Console.WriteLine("Data from Telco list: " + historyResponse.Data.Count);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<TelcoProduct>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<List<CashinProcedure>>> LoadCashinProcedures(WalletAuthResponseData credentials, HttpClient _httpClient,string accountId,string code)
        {
            try
            {
                var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);

                var historyResponse = await accountAPIClient.LoadCashinProcedures(accountId,code);

                Console.WriteLine("Data from Cashin list: " + historyResponse.Data.Count);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<CashinProcedure>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }



        public async Task<GeneratedReferenceModel> GenerateReferenceNumber(WalletAuthResponseData credentials, HttpClient _httpClient, string trantype)
        {

            var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
            try
            {
                Console.WriteLine("Testing Generation");
                return await accountAPIClient.GenerateTransactionReferenceNumber(trantype);
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed Generation {e.Message}");
                return new GeneratedReferenceModel { autoNum = "INVALID" };
            }
            
        }



		public async Task<GetCashoutFeeModel> GetCashoutFeeLoader(WalletAuthResponseData credentials, HttpClient _httpClient)
		{

			var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
			try
			{
				Console.WriteLine("Testing Generation");
				return await accountAPIClient.GetCashoutFee();

			}
			catch (Exception e)
			{
				Console.WriteLine($"Failed Generation {e.Message}");
				return new GetCashoutFeeModel { tranDesc = "INVALID", amount=0.00 };
			} 

		}

		public async Task<GetCashinFeeModel> GetCashinFeeLoader(WalletAuthResponseData credentials, HttpClient _httpClient)
		{

			var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
			try
			{
				Console.WriteLine("Testing Generation");
				return await accountAPIClient.GetCashinFee();

			}
			catch (Exception e)
			{
				Console.WriteLine($"Failed Generation {e.Message}");
				return new GetCashinFeeModel { tranDesc = "INVALID", amount = 0.00 };
			}

		}



        public async Task<ApiResponseModel<MainUpdateContactResponseModel>> UpdateContactInfoWithOTP(WalletAuthResponseData credentials, HttpClient _httpClient, MainContactInfo postData)
        {

            var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
            try
            {
                var withdrawalResponse = await accountAPIClient.POSTUpdateContactDetails(postData);
                return withdrawalResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<MainUpdateContactResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<MainUpdateContactResponseModel>> UpdateContactInfoWithoutOTP(WalletAuthResponseData credentials, HttpClient _httpClient, ContactInfo postData)
        {

            var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);
            try
            {
                var withdrawalResponse = await accountAPIClient.POSTUpdateContactNumber(postData);
                return withdrawalResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<MainUpdateContactResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<List<IdTypeListModel>>> LoadKYCIDlist(WalletAuthResponseData credentials, HttpClient _httpClient)
        {
            try
            {
                var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);

                var jsonResponse = await accountAPIClient.LoadKYCIDList();
                Console.WriteLine("Data from KYC ID list: " + jsonResponse.Data.Count);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<IdTypeListModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<List<FileTypeListeModel>>> LoadKYCFileTypelist(WalletAuthResponseData credentials, HttpClient _httpClient)
        {
            try
            {
                var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);

                var jsonResponse = await accountAPIClient.LoadKYCFileTypeList();
                Console.WriteLine("Data from KYC FIle Type list: " + jsonResponse.Data.Count);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<FileTypeListeModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<KYCUploadResponse>> POSTSendKYCForVerification(WalletAuthResponseData credentials, HttpClient httpClient, KYCUploadModel postData)
        {
            try
            {
                // Validate input parameters
                if (credentials == null)
                    throw new ArgumentNullException(nameof(credentials), "Credentials cannot be null");

                if (httpClient == null)
                    throw new ArgumentNullException(nameof(httpClient), "HttpClient cannot be null");

                if (postData == null)
                    throw new ArgumentNullException(nameof(postData), "KYC upload data cannot be null");

                // Validate required KYC data
                if (string.IsNullOrEmpty(postData.FilePath))
                    throw new ArgumentException("FilePath is required", nameof(postData));

                // Initialize the API client with the necessary credentials and HTTP client
                var accountAPIClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );

                Console.WriteLine("[DEBUG] Initiating KYC upload process");
                Console.WriteLine($"[DEBUG] FileTypeID: {postData.FileTypeID}");
                Console.WriteLine($"[DEBUG] AccountID: {postData.AccountID}");

                // Call the POSTUploadKYC method to upload the KYC data
                var uploadResponse = await accountAPIClient.POSTUploadKYC(postData);

                if (uploadResponse.IsSuccess)
                {
                    Console.WriteLine("[DEBUG] KYC upload completed successfully");
                }
                else
                {
                    Console.WriteLine($"[WARNING] KYC upload failed with status code: {uploadResponse.StatusCode}");
                }

                return uploadResponse;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"[ERROR] Validation error during KYC upload: {ex.Message}");
                return new ApiResponseModel<KYCUploadResponse>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Description = ex.Message,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception during KYC upload: {ex.Message}");
                Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
                return new ApiResponseModel<KYCUploadResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Internal error during KYC upload: {ex.Message}",
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<MainKYCDetailsResponse>> LoadKYCMemberDetails(WalletAuthResponseData credentials, HttpClient _httpClient, int kycID, int fileType, string accountId)
        {
            try
            {
                var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);

                var jsonResponse = await accountAPIClient.LoadMemberKYCDetails(kycID,fileType,accountId);
                Console.WriteLine("Data from KYC FIleDetals: " + jsonResponse.Data);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<MainKYCDetailsResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<MainAccountKYCResponse>> LoadMainAccountKYCLevelDetails(WalletAuthResponseData credentials, HttpClient _httpClient, string accountId)
        {
            try
            {
                var accountAPIClient = new WalletAccountsApiClient(Encryption.decodeString(_wallet_endpoints.baseUrlWallet), credentials, _httpClient);

                var jsonResponse = await accountAPIClient.LoadMainAccountKYCLevel(accountId);
                Console.WriteLine("Data from Main Account KYC Level: " + jsonResponse.Data);
                return jsonResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<MainAccountKYCResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        // Bills paymentssss


      
        public async Task<ApiResponseModel<BillsPaymentConfigModel>> LoadBillsPaymentSettingsLoader(
            WalletAuthResponseData credentials, HttpClient httpClient, string id)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.LoadBillsPaymentSettings(id);
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<BillsPaymentConfigModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<BillerInfoModel>> LoadBillerInfoByNameLoader(
            WalletAuthResponseData credentials, HttpClient httpClient, string billerName)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.LoadBillerInfoByName(billerName);
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<BillerInfoModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<BillerInfoModel>>> LoadBillerListLoader(
            WalletAuthResponseData credentials, HttpClient httpClient)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.LoadBillerList();
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<BillerInfoModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<BillsPaymentTokenModel>> GetBillerPaymentTokenLoader(
            WalletAuthResponseData credentials, HttpClient httpClient, int id)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.GetBillerPaymentToken(id);
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<BillsPaymentTokenModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<GetBillsPaymentFeeModel> GetBillerPaymentFeeLoader(
            WalletAuthResponseData credentials, HttpClient httpClient)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.GetBillerPaymentFee();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in GetBillerPaymentFeeLoader: {ex.Message}");
                return new GetBillsPaymentFeeModel { tranDesc = "INVALID", amount = 0.00 };
            }
        }

        public async Task<ApiResponseModel<PostBillsTransactionResponseModel>> PostBillerPaymentLoader(
            WalletAuthResponseData credentials, HttpClient httpClient, PostBillsPaymentTransactionModel postData)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.PostBillerPayment(postData);
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<PostBillsTransactionResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        // FUND TRANSFER  ---------------------


        public async Task<ApiResponseModel<PostBillsTransactionResponseModel>> POSTFundTransferLoader(
    WalletAuthResponseData credentials, HttpClient httpClient, POSTFundTransferModel postData)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.POSTFundTransfer(postData);
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<PostBillsTransactionResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<CommissionDetailsResponseList>>> LoadCommissionDetailsListLoader(
            WalletAuthResponseData credentials, HttpClient httpClient, string accountId)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.LoadCommissionDetailsList(accountId);
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<CommissionDetailsResponseList>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<CommissionBalanceResponse> LoadCommissionbalanceLoader(
            WalletAuthResponseData credentials, HttpClient httpClient, string accountId)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.LoadCommissionbalance(accountId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in LoadCommissionbalanceLoader: {ex.Message}");
                return _defaults_wal.invalidCommissionBalance;
            }
        }





        // Get Transfer Processing Fee Loader
        public async Task<GetTransferProcessingFeeModel> GetTransferProcessingFeeLoader(
            WalletAuthResponseData credentials, HttpClient httpClient)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.GetTransferProcessingFee();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in GetTransferProcessingFeeLoader: {ex.Message}");
                return _defaults_wal.invalidTransferProcessingFee;
            }
        }

        // Get WTax Fee Loader
        public async Task<GetWtaxFeeModel> GetWtaxFeeLoader(
            WalletAuthResponseData credentials, HttpClient httpClient)
        {
            try
            {
                var apiClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    httpClient
                );
                return await apiClient.GetWtaxFee();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in GetWtaxFeeLoader: {ex.Message}");
                return _defaults_wal.invalidWtaxFee;
            }
        }




        // App level

        public async Task<GetAppInfoModel> GetAppInfoLoader(
            WalletAuthResponseData credentials,
            HttpClient httpClient,
            int appId = 1)
        {
            try
            {
                var apiClient = new AppApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet), // Base URL
                    httpClient
                );
                return await apiClient.GetAppInfo(appId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Exception in GetAppInfoLoader: {ex.Message}");
                return _defaults_wal.invalidAppInfo;
            }
        }
        public async Task<ApiResponseModel<ModuleStatus>> LoadModuleStatusList(
        WalletAuthResponseData credentials,
        HttpClient _httpClient,
        int sysId)
        {
            try
            {
                var accountAPIClient = new WalletAccountsApiClient(
                    Encryption.decodeString(_wallet_endpoints.baseUrlWallet),
                    credentials,
                    _httpClient);

                var response = await accountAPIClient.LoadModuleStatusList(sysId);
                Console.WriteLine("Data from Module Status: " + (response.Data));
                return response;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<ModuleStatus>
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

