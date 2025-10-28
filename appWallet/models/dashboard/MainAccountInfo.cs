using IAM_Library.models.general;
using IAM_Library.models.wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.appWallet.models.dashboard
{
    internal class WalletMainAccountInfoModel
    {
        public ApiResponseModel<object> apiResponse { get; set; }
        public WalletAccountDetailData walletData { get; set; }
    }
    // MONDAY - WORK ON ACCOUNT FETCHING AND CONTACT AND ADDRESS! -- oct 27 iam wallet
    public class WalletAccountDetailData
    {
        public StatusModel status { get; set; }
        public WalletPrimaryInfoModel primaryInfo { get; set; }
        //public RankInfoModel rankInfo { get; set; }
        public WalletContactInfoModel contactInfo { get; set; }
        public WalletAddressInfoModel addressInfo { get; set; }
        public CredentialModel? credential { get; set; }

    }   
    public class TelcoModel
    {
        public int id { get; set; }
        public string telco { get; set; }
        public int client_id { get; set; }

        List<TelcoModel> telcoList;

        public IEnumerator<TelcoModel> GetEnumerator()
        {
            foreach (var net in telcoList)
                yield return net;
        }
    }

    public class CashinProcedure
    {
        public string bank_code { get; set; }
        public int id { get; set; }
        public string steps_desc { get; set; }

        List<CashinProcedure> procedures;

        public IEnumerator<CashinProcedure> GetEnumerator()
        {
            foreach (var p in procedures)
                yield return p;
        }
    }


    public class TelcoProduct
    {
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_desc { get; set; }
        public string telco { get; set; }   
        public double price { get; set; }

        List<TelcoProduct> telcoList;

        public IEnumerator<TelcoProduct> GetEnumerator()
        {
            foreach (var net in telcoList)
                yield return net;
        }
    }


    public class LogoutPOSTModel
    {
        public int option { get; set; }
        public string accountKey { get; set; }
        public string deviceID { get; set; }
        public string sesssionID { get; set; }

 
    }

    public class POSTVerify2FAModel
    {
        public string tranNo { get; set; }
        public string accountKey { get; set; }
        public string sessionID { get; set; }
        public string actionID { get; set; }
        public string pin { get; set; }
    }



    public class POST2FAModel
    {
        public string tranNo { get; set; }
        public string actionID { get; set; }
        public string accountKey { get; set; }
        public string deviceID { get; set; }
        public string sessionID { get; set; }
    }

    public class Generated2FAResponseModel
    {
        public string statusCode { get; set; }
        public string message { get; set; }
    }

    public class LogoutPOSTResponse
    {
        public string statusCode { get; set; }
        public string message { get; set; }
    }

    public class SendLoadPOSTModel
	{
		public string user_reference_number { get; set; }
		public string customer_number { get; set; }
		public string product_code { get; set; }
		public string telco_name { get; set; }
		public double amount { get; set; }
		public string accountid { get; set; }
		public int client_id { get; set; }
	}


	

	public class StatusModel
    {
        public string? statusCode { get; set; }
        public string? message { get; set; }
    }
    public class CredentialModel
    {
        public string? username { get; set; }
        public string? password { get; set; }
    }


    public class BinaryPtsBal
    {
        public double leftPts { get; set; }
        public double rightPts { get; set; }
    }

    public class RqvPtsBal
    {
        public double leftPts { get; set; }
        public double rightPts { get; set; }
    }

    public class WalletPrimaryInfoModel
    {
        //public string refno { get; set; }
        public string accountID { get; set; }
        //public string primaryID { get; set; }
        public string sponsorID { get; set; }
        //public string uplineID { get; set; }
        //public int grp { get; set; }
        public DateTime? dateRegistered { get; set; }
        //public DateTime? mdate { get; set; }
        //public DateTime? paydate { get; set; }
        public DateTime? bdate { get; set; }
        public string? fname { get; set; }
        public string? mname { get; set; }
        public string? sname { get; set; }
        //public bool? isMover { get; set; }
        //public DateTime? moverDate { get; set; }
    }

    public class WalletRankInfoModel
    {
        public string? package { get; set; }
        public string? position { get; set; }
        public string? rank { get; set; }
        public string? title { get; set; }
    }

    public class WalletContactInfoModel
    {
        public string email { get; set; }
        public string contactNo { get; set; }
    }

    public class WalletAddressInfoModel
    {
        public string? country_Code { get; set; }
        public string? state_Code { get; set; }
        public string? city_Code { get; set; }
        //public string? brgy { get; set; }
        public string? homeAddress { get; set; }
    }


    // WALLET MODELSSSS --------------------------------

    // WALLET Models
    // balance and transaction list
    public class WalletBalanceModel
    {
        public string account_id { get; set; }
        public decimal account_balance { get; set; }
    }

    public class GeneratedReferenceModel
    {
        public string autoNum { get; set; }

    }

    /* public class CashoutWalletTransactionsModel
     * 
     * {
        "svrDate": "2024-10-12T12:07:00",
        "tranType": 2,
        "tranDesc": "Cash Out",
        "sourceID": 104,
        "sourceDesc": "BANKS",
        "tranNo": "TEST10122403",
        "amount": -102.00,
        "witholdingTax": 0.00,
        "processingFee": 0.00,
        "netAmt": -102.00,
        "accountId": "00000001"
    },
     * 
     * 
     * 
     */
    public class WalletTransactionsModel
    {
        public DateTime trandate { get; set; }
        public int trantype { get; set; }
        public string trandesc { get; set; }
        public int sourceid { get; set; }
        public string sourcedesc { get; set; }
        public string tranno { get; set; }
        public string accountid { get; set; }
        public string account_name { get; set; }
        public double amount { get; set; }
        public bool isposted { get; set; }
        public DateTime post_date { get; set; }

        List<WalletTransactionsModel> walletTransactionList;

        public IEnumerator<WalletTransactionsModel> GetEnumerator()
        {
            foreach (var transaction in walletTransactionList)
                yield return transaction;
        }
    }
    // cashout bank list - use bearer
    public class BankCashoutList
    {
        public string bank_name { get; set; }
        public string bank_code { get; set; }

        List<BankCashoutList> bankList;

        public IEnumerator<BankCashoutList> GetEnumerator()
        {
            foreach (var bank in bankList)
                yield return bank;
        }
    }
    // POST Cashout model
    public class POSTCashoutModel
    {
        public double amount { get; set; }
        public string account_first_name { get; set; }
        public string account_last_name { get; set; }
        public string account_number { get; set; }
        public string bank_code { get; set; }
        public string description { get; set; }
        public string client_reference_number { get; set; }
        public string accountid { get; set; }
        public string bank_name { get; set; }
    }

    public class POSTSendLoad
    {
        public string user_reference_number { get; set; }
        public string customer_number { get; set; }
        public string product_code { get; set; }
        public string telco_name { get; set; }
        public double amount { get; set; }
        public string accountid { get; set; }
        public int client_id { get; set; }
    }



    public class InstapayBalanceModel
    {
        public string account_name { get; set; }
        public decimal amount { get; set; }
    }


    public class MainCashoutResponseModel
    {
        public string version { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public CashoutResult result { get; set; }
    }

    public class CashoutResult
    {
        public string account_first_name { get; set; }
        public string account_last_name { get; set; }
        public string account_number { get; set; }
        public string bank_code { get; set; }
        public string description { get; set; }
        public string client_reference_number { get; set; }
        public double amount { get; set; }
        public double service_charge { get; set; }
        public int status_id { get; set; }
        public string partner_transaction_id { get; set; }
        public DateTime created_at { get; set; }
    }

    public class MainLoadResponse
    {
        public string version { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public LoadingResult result { get; set; }
    }

    public class LoadingResult
    {
        public string user_reference_number { get; set; }
        public string customer_number { get; set; }
        public string product_code { get; set; }
        public string system_reference_number { get; set; }
        public int status_id { get; set; }
        public DateTime created_at { get; set; }
    }


    public class GenOTPModelPost
	{
		public string tranNo { get; set; }
		public string accountID { get; set; }
		public string accountKey { get; set; }
		public string deviceID { get; set; }
		public string sessionID { get; set; }
		
	}

	public class GenOTPResponseModel
	{
		

	}


    public class GetCashoutFeeModel
    {
        public string tranDesc { get; set; }
		public double amount { get; set; }
	}

	public class GetCashinFeeModel
	{
		public string tranDesc { get; set; }
		public double amount { get; set; }
	}


    //


    public class MainContactInfo
    {
        public string accountKey { get; set; }
        public string pin { get; set; }
        public string sessionID { get; set; }
        public ContactInfo contactInfo { get; set; }
    }

    public class ContactInfo
    {
        public string? accountKey { get; set; }

        public string email { get; set; }
        public string mobileNo { get; set; }
    }




    public class MainUpdateContactResponseModel
    {
        public string statusCode { get; set; }
        public string message { get; set; }

    }

    public class IdTypeListModel
    {
        public int idCode { get; set; }
        public string idType { get; set; }
        public string idDesc { get; set; }
        public bool isActive { get; set; }

        List<IdTypeListModel> idList;

        public IEnumerator<IdTypeListModel> GetEnumerator()
        {
            foreach (var bank in idList)
                yield return bank;
        }
    }

    public class FileTypeListeModel
    {
        public int fileType { get; set; }
        public string fileDesc { get; set; }
        public bool isActive { get; set; }

        List<FileTypeListeModel> fileList;

        public IEnumerator<FileTypeListeModel> GetEnumerator()
        {
            foreach (var bank in fileList)
                yield return bank;
        }

    }

    public class KYCUploadModel
    {
        public int? option { get; set; }
        public DateTime SvrDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int KYCID { get; set; }
        public int IDCode { get; set; }
        public string AccountID { get; set; }
        public string CardNumber { get; set; }
        public string CardFname { get; set; }
        public string CardMname { get; set; }
        public string CardSname { get; set; }
        public int FileTypeID { get; set; }
        public string FilePath { get; set; }  // This will store the base64 string of the file
        public int Status { get; set; }
        public string VerifiedBy { get; set; }
    }


    public class KYCLevels
    {
        public int kycid { get; set; }
        public int level { get; set; }
        public string desc { get; set; }

        public int min_CashIn { get; set; }
        public int max_CashIn { get; set; }
        public int min_CashOut { get; set; }
        public int max_CashOut { get; set; }

        List<KYCLevels> levels;

        public IEnumerator<KYCLevels> GetEnumerator()
        {
            foreach (var lvl in levels)
                yield return lvl;
        }

    }

    public class KYStatusList
    {
        public int statusID { get; set; }
        public string statusDesc { get; set; }


        List<KYStatusList> stats;

        public IEnumerator<KYStatusList> GetEnumerator()
        {
            foreach (var st in stats)
                yield return st;
        }

    }

    public class KYCUploadResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
    }

    public class MainKYCDetailsResponse
    {
        public KYCStatusModel status { get; set; }
        public KYCDetailsModel details { get; set; }
    }


    public class KYCStatusModel
    {
        public int statusCode { get; set; }
        public string message { get; set; }
    }

    public class KYCDetailsModel
    {
        public DateTime dateUploaded { get; set; }
        public DateTime updatedDate { get; set; }
        public int kycid { get;set; }
        public int idCode { get; set; }
        public string accountID { get;set; }
        public string cardNumber { get; set; }
        public string cardFname { get; set; }
        public string cardMname { get; set; }
        public string cardSname { get; set; }
        public int fileTypeID { get; set; }
        public string filePath { get; set; }
        public int status { get; set; } = 0;
        public string verifiedBy { get; set; }  = "           ";
    }

    public class MainAccountKYCResponse
    {
        public KYCStatusModel status { get; set; }
        public KYCLevelModel level { get; set; }
    }

    public class KYCLevelModel
    {
        public int kycid { get; set; }
        public int level { get; set; }
        public string desc { get; set; }
        public double min_CashIn { get; set; }
        public double min_CashOut { get; set; }
        public double max_CashIn { get; set; }
        public double max_CashOut { get; set; }
    }
    public class BillsPaymentConfigModel
    {
        public string description { get; set; }
        public string base_url { get; set; }
        public string oauth_endpoint { get; set; }
        public string inqbal_endpoint { get; set; }
        public string inqtranx_endpoint { get; set; }
        public string billerlist_endpoint { get; set; }
        public string transact_endpoint { get; set; }
        public int client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string scope { get; set; }
        public bool stat { get; set; } = true;
    }

    public class BillerInfoModel
    {
        public string categoryName { get; set; }
        public string billerName { get; set; }
        public string description { get; set; }
        public int billerType { get; set; }
        public decimal serviceCharge { get; set; }
        public DateTime createdAt { get; set; }
        public bool stat { get; set; } = true;

        List<BillerInfoModel> infos;

        public IEnumerator<BillerInfoModel> GetEnumerator()
        {
            foreach (var info in infos)
                yield return info;
        }
    }

    public class GetBillsPaymentFeeModel
    {
        public string tranDesc { get; set; }
        public double amount { get; set; }
    }

    public class PostBillsPaymentTransactionModel
    {
        public decimal amount { get; set; }
        public decimal processing_fee { get; set; }
        public string biller_name { get; set; }
        public string biller_description { get; set; }
        public string client_reference_number { get; set; }
        public string biller_account_number { get; set; }
        public string biller_account_name { get; set; }
        public string mobile_number { get; set; }
        public string bill_date { get; set; }  // for MERALCO
        public string due_date { get; set; }   // for MERALCO
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string accountid { get; set; } // user ID
    }


    //public class PostBillsPaymentTransactionModel
    //{
    //    public decimal amount { get; set; }
    //    public string biller_name { get; set; }
    //    public string client_reference_number { get; set; }
    //    public Dictionary<string, string> accountid { get; set; } // <-- dynamic structure
    //}
    

    //public class PostBillsPaymentTransactionModel
    //{
    //    public decimal amount { get; set; }
    //    public decimal processing_fee { get; set; }
    //    public string biller_name { get; set; }
    //    public string biller_description { get; set; }
    //    public string client_reference_number { get; set; }
    //    public string biller_account_number { get; set; }
    //    public string biller_account_name { get; set; }
    //    public string mobile_number { get; set; }
    //    public string accountid { get; set; }
    //}




    public class BillsPaymentField
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class BillsPaymentResult
    {
        public int billerid { get; set; }
        public List<BillsPaymentField> fields { get; set; }
        public decimal? amount { get; set; }
        public decimal service_charge { get; set; }
        public int status_id { get; set; }
        public string transaction_reference_number { get; set; }
    }

    public class PostBillsTransactionResponseModel
    {
        public string version { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public BillsPaymentResult result { get; set; }
    }

    public class BillsPaymentTokenModel
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }

    // POST Fund Transfers

    // Commssiion balance
    public class CommissionBalanceResponse
    {
        public string account_id { get; set; }
        public decimal? account_balance { get; set; }
    }

    public class CommissionDetailsResponseList
    {
        public string accountid { get; set; }
        public DateTime trandate { get; set; }
        public int trantype { get; set; }
        public string trandesc { get; set; }
        public string refno { get; set; }
        public string tranno { get; set; }
        public decimal? amt { get; set; }
        public decimal? netamt { get; set; }

        List<CommissionDetailsResponseList> infos;

        public IEnumerator<CommissionDetailsResponseList> GetEnumerator()
        {
            foreach (var details in infos)
                yield return details;
        }
    }

    public class POSTFundTransferModel
    {
        public string idno { get; set; }
        public string refno { get; set; }
        public string tranno { get; set; }
        public decimal? amount { get; set; }

        public int? wtax { get; set; }
        public int? process_fee { get; set; }
        public decimal? netamt { get; set; }



        public string memname { get; set; }
        public string remarks { get; set; }
    }


    public class GetTransferProcessingFeeModel
    {
        public string tranDesc { get; set; }
        public decimal amount { get; set; }
    }

    // WTax Fee Model
    public class GetWtaxFeeModel
    {
        public string tranDesc { get; set; }
        public decimal amount { get; set; }
    }
    public class GetAppInfoModel
    {
        public int appID { get; set; }
        public string appName { get; set; } = string.Empty;
        public string appVersionCode { get; set; } = string.Empty;
        public int buildStatus { get; set; }
        public string releaseNotes { get; set; } = string.Empty;
        public DateTime dateReleased { get; set; }
    }

    public class ModuleStatus
    {
        public int sysID { get; set; }
        public string sysFunction { get; set; }
        public string sysMessage { get; set; }
        public bool isSysActive { get; set; }

        List<ModuleStatus> infos;

        public IEnumerator<ModuleStatus> GetEnumerator()
        {
            foreach (var details in infos)
                yield return details;
        }
    }



}
