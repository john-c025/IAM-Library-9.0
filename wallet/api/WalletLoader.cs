using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using IAM_Library.models.general;
using IAM_Library.models.wallet;
using IAM_Library.wallet.api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IAM_Library.wallet
{
    public class WalletLoader
    {
        private readonly MainWalletAPIClient _walletClient;

        public WalletLoader(AuthApiResponseData credentials, HttpClient httpClient)
        {
            string apiBaseUrl = Encryption.decodeString(_constants.authBaseUrl);
            _walletClient = new MainWalletAPIClient(apiBaseUrl, credentials, httpClient);
        }

        public async Task<ApiResponseModel<BankAccountDetail>> LoadBankDetailsById(string idNumber)
        {
            try
            {
                var balanceResponse = await _walletClient.LoadBankAccountDetails(idNumber);
                return balanceResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<BankAccountDetail>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<WalletBalanceGETResponseModel>> LoadWalletBalance(string idNumber)
        {
            try
            {
                var balanceResponse = await _walletClient.LoadWalletBalance(idNumber);
                return balanceResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<WalletBalanceGETResponseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<J4UPreview>> LoadJ4UPreviews(string idNumber)
        {
            try
            {
                var balanceResponse = await _walletClient.LoadJ4UPreview(idNumber);
                return balanceResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<J4UPreview>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<TransactionListModel>>> LoadTransactionHistoryList(string accountKey)
        {
            try
            {
                var historyResponse = await _walletClient.LoadTransactionHistoryList(accountKey);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<TransactionListModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<WalletJ4uUpdaterepsonseModel>> SelectJ4U(WalletJ4UModel updateModel,string idNo,string idNoIfNull, bool isSelected)
        {
            string id;
            if (string.IsNullOrEmpty(updateModel.idNumber))
            {
                id = idNoIfNull;
            }
            else
            {
                id = updateModel.idNumber;
            }

            var updatedRecord = new WalletJ4UModel
            {
                refno = updateModel.refno,
                tranDate = updateModel.tranDate,
                idNumber = id,
                package = updateModel.package,
                amt = updateModel.amt,
                isSelected = isSelected
            };
            try
            {
                Console.WriteLine($"UpdateBankDetails Function: Updating j4u selection details for user ID: {updateModel.idNumber}");
                var response = new ApiResponseModel<WalletJ4uUpdaterepsonseModel>();
                return response = await _walletClient.UpdateSelectionForJ4URecord(updatedRecord, updatedRecord.idNumber);
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<WalletJ4uUpdaterepsonseModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = $"Error on Loader [SelectJ4U] : {ex.Message}"
                };
            }
        }

        public async Task<ApiResponseModel<List<WalletJ4UModel>>> LoadJ4UWalletList(string accountKey)
        {
            try
            {
                var historyResponse = await _walletClient.LoadJ4UWallet(accountKey);
                return historyResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<WalletJ4UModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<List<EncashmentTypeModel>>> LoadEncashmentTypeList()
        {
            try
            {
                var encashmentListResponse = await _walletClient.LoadEncashmentTypes();
                return encashmentListResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<EncashmentTypeModel>>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<SuccessfulWithdrawalResponse>> WithdrawWalletCommission(string refnoParam, string idNumberParam, int tranTypeParam, double encashedAmtParam, string areaCodeParam)
        {
            WithdrawWalletPOSTModel postData = new WithdrawWalletPOSTModel { refno = refnoParam, idNumber = idNumberParam, tranType = tranTypeParam, encashedAmt = encashedAmtParam, areaCode = areaCodeParam };
            try
            {
                var withdrawalResponse = await _walletClient.WithdrawWalletCommission(postData);
                return withdrawalResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<SuccessfulWithdrawalResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<J4UWithdrawalResponse>> WithdrawWalletCommissionJ4U(string idNumberParam)
        {
            try
            {
                var withdrawalResponse = await _walletClient.WithdrawJ4U(idNumberParam);
                return withdrawalResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<J4UWithdrawalResponse>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<EnashmentGetDetailsModel>> GetEncashmentDetails(string refno, string idno)
        {
            try
            {
                var encashmentResponse = await _walletClient.GetEncashmentDetails(refno, idno);
                return encashmentResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<EnashmentGetDetailsModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponseModel<GetEncashmentDetailModel>> GetEncashmentDetailsLoader(string refno, string idno)
        {
            try
            {
                var encashmentResponse = await _walletClient.GetEncashmentTransactionDetails(refno, idno);
                return encashmentResponse;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<GetEncashmentDetailModel>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<ApiResponseModel<AutoGenNumberTransaction>> GenerateAutoNumber(string tranType)
        {
            try
            {
                var autoGeneratedNumber = await _walletClient.GenerateAutoNumberTrasaction(tranType);
                return autoGeneratedNumber;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<AutoGenNumberTransaction>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Description = ex.Message,
                    Data = null
                };
            }
        }


        public async Task<bool> isItTuesdayToday()
        {

            bool todayIsTuesday = false;


            try
            {
                var dateFromNetwork = await _walletClient.GetDateTimeToday();
                Console.WriteLine($"Date today {dateFromNetwork.Data} and day is {dateFromNetwork.Data.DayOfWeek.ToString()}");
                if (dateFromNetwork.IsSuccess)
                {
                    var Tuesday = DayOfWeek.Tuesday;


                       if (dateFromNetwork.Data.DayOfWeek == Tuesday)
                       {
                            todayIsTuesday = DateTime.Now.DayOfWeek == Tuesday;
                        
                       }

                    else
                    {
                        todayIsTuesday = false;
                    }

                }
                return todayIsTuesday;

            }

            catch(Exception ex)

            {
                Console.WriteLine($"Error in date retrieval");
                return false;
            }
        }
    }
}