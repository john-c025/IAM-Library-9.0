using IAM_Library.api;
using IAM_Library.appWallet.models.dashboard;
using IAM_Library.models.auth;
using IAM_Library.models.dashboard;
using IAM_Library.models.general;
using IAM_Library.models.reports;
using IAM_Library.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.appWallet.api
{
    public class _AuthEndpoints
    {
        public string authSignatureGeneration { get; set; }

    }


    internal class _wallet_endpoints
    {
        public void SetKeyAndIV(string key, string iv)
        {
            _key = Encoding.UTF8.GetBytes(key);
            _iv = Encoding.UTF8.GetBytes(iv);
        }
        //store keys here for now for testing, will add to environment for security upon production!
        private static byte[] _key = Encoding.UTF8.GetBytes("Dev@201912907125");
        private static byte[] _iv = Encoding.UTF8.GetBytes("201912907@JohnTC");

        public const string authHeader = "QmVhcmVy";
        public const string accountDetailsParams = "";
        //base url 
        public static string live = "aHR0cHM6Ly9hcGl2Mi5pYW0td29ybGR3aWRlY29ycC5jb20="; //CZ+ow8okUFtdDoc3GZhGbxh4rUNL8xWt+lIfxeqj8iJHt0aay4xoEEPA0htcW6ba
        public static string beta = "aHR0cHM6Ly9iZXRhYXBpLmlhbS13YWxsZXQuY29t"; //betawalapi = CZ+ow8okUFtdDoc3GZhGb62rCZ+EBZpGa8Og9F3n19fYYZWuIdiwfWGVcepim9lz5Ub1kHuJD7xAboOhuqG4Sg==
        public static string live_v2 = "aHR0cHM6Ly9hcGkuaWFtLXdvcmxkd2lkZWNvcnAuY29t";
        public static string live_wallet = "aHR0cHM6Ly9hcGkuaWFtLXdhbGxldC5jb20=";
        public static string beta_wallet = "aHR0cHM6Ly9iZXRhYXBpLmlhbS13YWxsZXQuY29t";
        public static string beta_api_util = IAM_Maintenance.Decrypt("CZ+ow8okUFtdDoc3GZhGb+9+UWZwkg6FFTkRUDFkjY4PiDLPk6tQIgW1QWGfMchy14YY5UubXztuCTRaZLrd1Q==", _key, _iv);
        public static string live_api_util = "aHR0cHM6Ly9hcGl1dGlsLmlhbS13b3JsZHdpZGVjb3JwLmNvbQ==";


        public static string baseUrlWalletKYC = live_api_util;
        public static string baseUrlWallet = live_wallet;
        
        
        public static string walletAuthtenticate = IAM_Maintenance.Decrypt("WsycQEl703GLHZxOKXXKz+ki6bocLT/7eCUAaWYdOtc=", _key, _iv);
        public static string walletGetAccountDetails = IAM_Maintenance.Decrypt("MVm0p9volf1zxQIVroW2SSaB17j/t0xR93P5NANppBNk81U9U7B06KCYa044teLO", _key, _iv);
        public static string walletGetAccountDetailsFull = IAM_Maintenance.Decrypt("MVm0p9volf1zxQIVroW2SSaB17j/t0xR93P5NANppBOqj+bU4wetsH62p4atb4Hvl2jMSWujHavn7bIJFQn9GQ==", _key, _iv);
        public static string api_key = "NjNkNDYwMGY2YmM4YjM0MTQwZmNhMGJkM2E4M2I1Nzg=";
        //f24b51dfd6fda3a6fb20882c1554790e
        
        // WALLETS


        // GET CASHOUT BANK LIST
        public static string getCashoutBankListEndpoint = IAM_Maintenance.Decrypt("lcZJ62lIvE9jg2xRGgru2btNTzrkWlrEYccBMUoiy1Beg6seVaOxv1nP7VK4KD3Q", _key, _iv);
        // GET CASHOUT Transaction details - /client_ref_number
        public static string getCashoutTransactionDetailsEndpoint = IAM_Maintenance.Decrypt("lcZJ62lIvE9jg2xRGgru2aimf9JEPYHBSePYgk13lo3fD7UcIKaODE3AOYmyN7bNQiEuutcHABof364srd0VLw==", _key, _iv);
        // GET Wallet Transactons list 
        public static string getWalletTransactions = IAM_Maintenance.Decrypt("lcZJ62lIvE9jg2xRGgru2YEdnRWIkxnmhg4y1tzX6x6Z+J/f8sql/ateryxzA4MkEi/Oyk+dSbfTQUFgNqj4+Q==", _key, _iv);
        // POST Cashout
        public static string postCashoutEndpoint = IAM_Maintenance.Decrypt("lcZJ62lIvE9jg2xRGgru2fl/uChLdckAIIIttSxF44QUWwEdJ3Y7kHplKYtM3+ti", _key, _iv);

        //Wallet API - /accountid
        public static string getWalletAccountBalanceEndpoint = IAM_Maintenance.Decrypt("4EGFPqqQGdZWr0Pjewoyn+pZv+8EYU8XL1KTG2Re8ltz83eYmtP/srbwwvQSReqwkHTEFKXwms4XiUtFj9mulQ==", _key, _iv);
        //Wallet Get Transaction List - /accountid
        public static string getCashouWalletTransactionListEndpoint = IAM_Maintenance.Decrypt("lcZJ62lIvE9jg2xRGgru2YEdnRWIkxnmhg4y1tzX6x6Z+J/f8sql/ateryxzA4MkEi/Oyk+dSbfTQUFgNqj4+Q==", _key, _iv);
        //v1/Wallet
        public static string getWalletTransactionListEndpoint = IAM_Maintenance.Decrypt("4EGFPqqQGdZWr0PjewoynxtVRL8Ea0zMwvDylAdBhxZ3d4Y5T36fIrD8vdr+mcXj", _key, _iv);

        //load
        public static string getIamLoadBalanceEP = IAM_Maintenance.Decrypt("DjBzwPJoSFoyOlQicwtShDib6hrWZvNGdYBHE95lcJhBXuCUJJhmMTyGoZUIfT41", _key, _iv);
        ///v1/Eload/PostSendLoad
        public static string postSendload = IAM_Maintenance.Decrypt("O8yO1W7Jm1aEEeg4dkILc4l6peeLXrYuOgFsF6gxq/Kxtw9KaFJg8EM8QRm6r0VO", _key, _iv);
        ////v1/Eload/GetEloadTransactionStatus
        public static string getELoadTransactionStatus = IAM_Maintenance.Decrypt("DjBzwPJoSFoyOlQicwtShI9oo64dmeuctyASQsG2Jq5w694GhykqGq/9UXcM79vw+2mBOuo8sjU3enuaKvo2nQ==", _key, _iv);
        // IAM Side
        // Get IAM Balance /
        // /v1/Iam/GetIamWalletTransactions
        public static string getIamWalletTransactions = IAM_Maintenance.Decrypt("usYIKCZ6jykzln4jB9F4twFkqw3nlZNBzcWUKXCeXtNSkNolq4chV8dKqWk1au2N", _key, _iv);
        // Get Instapa balance
        // /v1/Iam/GetIamInstapayBalance
        public static string getIamInstapayBalance = IAM_Maintenance.Decrypt("usYIKCZ6jykzln4jB9F4t6UDssqurpjxiWfq5HTt10iLChixRW3IN9rh6n8jMaw2", _key, _iv);
        // POST update cred
        public static string postUpdateUserCredentialWallet = IAM_Maintenance.Decrypt("MVm0p9volf1zxQIVroW2SWcFZsyF5yHvB9MmrLGtIJvIR1Lq07daHHndji9pKkkL", _key, _iv);
        // Util



        public static string getGeneratedReferenceNumber = IAM_Maintenance.Decrypt("CZ+ow8okUFtdDoc3GZhGb+9+UWZwkg6FFTkRUDFkjY4PiDLPk6tQIgW1QWGfMchyKrFCSRlKB56m9z0SPs+J0Xq4uhPZp3xTOHF21VGw1QtCAQx0RoMxe5APPYM3f9PIYb9Day9dlMk4e86s2dNKHA==", _key, _iv);

        public static string getEloadTelcoList = IAM_Maintenance.Decrypt("CZ+ow8okUFtdDoc3GZhGbxh4rUNL8xWt+lIfxeqj8iLP0NukuwCdSODh4Npnbrz+zKcVPAIwOgoKW9kA1uwTakBU8zrfOjhJ+NU5n52Ka34=", _key, _iv);

        public static string getEloadPromosByTelcoName = IAM_Maintenance.Decrypt("CZ+ow8okUFtdDoc3GZhGbxh4rUNL8xWt+lIfxeqj8iLP0NukuwCdSODh4Npnbrz+zKcVPAIwOgoKW9kA1uwTauaFt+N6Qf2agdNqC4uZvTjvbx361AaKvUNqex/RuEzf", _key, _iv);

        public static string generate2FA = IAM_Maintenance.Decrypt("wK3bZs7NHwaRZPLMwYXQsC8c0c83JoPkf2/qyRMsZJjhCow+2C0lCM1WtkdLYexQ", _key, _iv);

        public static string verify2FA = IAM_Maintenance.Decrypt("CZ+ow8okUFtdDoc3GZhGbxh4rUNL8xWt+lIfxeqj8iLODHhOLPhbpCxcGL5LsFXk3x/jWWJA4uAXglzNd5vMRWDCeL01H/KqZleQNArnQ9M=", _key, _iv);

        public static string loadCashinEndpoint = IAM_Maintenance.Decrypt("CZ+ow8okUFtdDoc3GZhGbxh4rUNL8xWt+lIfxeqj8iJ1lYhgmWfbpFNUYWGFu+TFtO6e7nw18Ex+oYeKjp3SiXg2crV73he+xlTMhd5iFHAj2yJGNKD81dZtptS0b/pZ", _key, _iv);
        public static string loadCashParam = IAM_Maintenance.Decrypt("fsuMKYcWYYDsBMYP3TKLtAEjJbdMh5I1jPjam521F88=", _key, _iv);
        public static string logoutAPI = IAM_Maintenance.Decrypt("CZ+ow8okUFtdDoc3GZhGbxh4rUNL8xWt+lIfxeqj8iLODHhOLPhbpCxcGL5LsFXk73wZgxsXLgC8RaeP/VD3eA==",_key,_iv);

        public static string COProcessingFee = "aHR0cHM6Ly9hcGkuaWFtLXdhbGxldC5jb20vdjEvQ2FzaG91dC9HZXRDYXNob3V0UHJvY2Vzc2luZ0ZlZQ==";

        public static string CIProcessingFee = "aHR0cHM6Ly9hcGkuaWFtLXdhbGxldC5jb20vdjEvQ2FzaGluL0dldENhc2hpblByb2Nlc3NpbmdGZWU=";

        /*
         *  /v1/Account/UpdateCredential
         *  
         *  
         *  
         *  {
  "accountKey": "string",
  "username": "string",
  "password": "string"
}
         * 
         */

        //new fixed wallet ep
        public static string getGeneratedReferenceNew = IAM_Maintenance.Decrypt("EJEylJL++DA9n9FuJokqyMkGp2DTn4HxBwpgrM5IlXjp03Bm1A7U32Wo+QdIFew4s9+D0xmiPqKDyeYav/wyFA==", _key, _iv);
        public static string getTelcoListNew = IAM_Maintenance.Decrypt("DjBzwPJoSFoyOlQicwtShKNUAvUg/rO/gCCFS7usqoNl9yU1qqi0ab8IGl58KH7d", _key, _iv);
        public static string getLoadPromosByNameNew = IAM_Maintenance.Decrypt("DjBzwPJoSFoyOlQicwtShEyTyCIDqGk0egpd+nUVV5ewfm/sPB28Hg/iwJkknw6k", _key,_iv);
        public static string verify2FANew = IAM_Maintenance.Decrypt("wK3bZs7NHwaRZPLMwYXQsHRV6P3JHzE75shf5/KRZbU=", _key, _iv);
        public static string loadCashinListNew = IAM_Maintenance.Decrypt("D/byrB3+AHpeyIQh85cwlCJTkdezC1IsbO8+v3SOjp2jDeRp9kz68G3LeFEoWTDn",_key, _iv);
        public static string logoutNew = IAM_Maintenance.Decrypt("WsycQEl703GLHZxOKXXKzwCMK3j2RzoRPv1iZC2zewY=", _key, _iv);
        public static string COProcessingFeeNew = IAM_Maintenance.Decrypt("lcZJ62lIvE9jg2xRGgru2fY9mlxwPgP3sYi1tTqQ4xP0ZKhkyZKzJbE5bjRrlOXqotqCtp09rV91/UD6gNTcEA==", _key, _iv);
        public static string CIProcessingFeeNew = IAM_Maintenance.Decrypt("D/byrB3+AHpeyIQh85cwlCJTkdezC1IsbO8+v3SOjp0ORIGt+uibaVLersqzhKWX", _key, _iv);
        public static string updateContactDetails = IAM_Maintenance.Decrypt("MVm0p9volf1zxQIVroW2SblL/mXr8OQSN04JvKN9JH22+/2qHLDnE5LX6hTc8lbX", _key, _iv);


        // KYC Endpoint
        public static string LoadKYCIDListEndpoint = IAM_Maintenance.Decrypt("1qey5sGcaPdFiei+2xTA00yJwRaYUUvMCVmuxjPQ8twvWSF0TzVy/lksFudYTkNZ", _key,_iv);
        public static string LoadKYCFileTypesEndpoint = IAM_Maintenance.Decrypt("1qey5sGcaPdFiei+2xTA0/hiBCaXastRoAlge8mcFaPwNOrynCCoE76FvLEQ3a+bMe8kbxAbpia3kgFWJWsYiQ==", _key, _iv);
        public static string UploadKYC = IAM_Maintenance.Decrypt("Y82oIMWLFmLYhP6JhsreVctKHwNokLx1oxrTc0ybUL2+Q6CQd3zNIPZhrq8Vc8VE", _key, _iv);
        // Additional....
        public static string LoadKYCLevels = IAM_Maintenance.Decrypt("1qey5sGcaPdFiei+2xTA04HjYB+4j1P99VRwUl7IqVUk68U2/y2myscoFYDUC0h1", _key, _iv);
        public static string LoadKYCStatusList = IAM_Maintenance.Decrypt("1qey5sGcaPdFiei+2xTA0xUTi8vubK+0F0/X/igYtS+XKWHW3krrcYmwEsAMWE7U", _key, _iv);
        public static string LoadMemberKYC = IAM_Maintenance.Decrypt("1qey5sGcaPdFiei+2xTA08Efbho4dQF6kmek3SFXozaDW83UPjoA/NcRsWZh9BkKV7cgA4mGj4t7rtxU08PkLw==", _key, _iv);
        
        public static string LoadKYCDetailsBase = IAM_Maintenance.Decrypt("j7YbMaOGq5cYdkVY0thzajOUDuh1YOlXWwjllbfxQ1v+XWK3sIuyp4/b+YKX7SnO", _key, _iv);
        public static string LoadKYCDetailsKYCParam = "a3ljaWQ9";
        public static string LoadKYCDetailsFileTypParam = "ZmlsZXR5cGU9";
        public static string LoadKYCDetailsAccountIDParam = "YWNjb3VudGlkPQ==";
        public static string LoadMainKYCLevel = "L3YxL0tZQy9HZXRBY2NvdW50S1lDTGV2ZWw/b3B0aW9uPTEmYWNjb3VudGlkPQ==";

        /// <summary>GET /v1/KYC/GetAccountKYCHistory?accountid= — plain path (apiutil host via baseUrlWalletKYC).</summary>
        public static string GetAccountKYCHistory = "/v1/KYC/GetAccountKYCHistory";

        // BillsPayment

        
            
            //v1/BillsPayment/GetBillsPaymentSettings/{id}

            public static string GetBillPaymentSettings = "L3YxL0JpbGxzUGF5bWVudC9HZXRCaWxsc1BheW1lbnRTZXR0aW5ncw==";
            

            //v1/BillsPayment/GetBillsPaymentToken/{id}

            public static string GetBillsPaymentToken = "L3YxL0JpbGxzUGF5bWVudC9HZXRCaWxsc1BheW1lbnRUb2tlbg==";

            

            //v1/BillsPayment/GetBillsPaymentBillerList

             public static string GetBillsPaymentBillerList = "L3YxL0JpbGxzUGF5bWVudC9HZXRCaWxsc1BheW1lbnRCaWxsZXJMaXN0";
            
         

            //v1/BillsPayment/GetBillsPaymentBillerDetails/{biller_name}

             public static string GetBillsPaymentBillerDetails = "L3YxL0JpbGxzUGF5bWVudC9HZXRCaWxsc1BheW1lbnRCaWxsZXJEZXRhaWxz";

           
            //v1/BillsPayment/GetBillsPaymentProcessingFee

            public static string GetBillsPaymentProcessingFee = "L3YxL0JpbGxzUGF5bWVudC9HZXRCaWxsc1BheW1lbnRQcm9jZXNzaW5nRmVl";

            
            //v1/BillsPayment/PostBillsPayment
            public static string PostBillsPayment = IAM_Maintenance.Decrypt("gW+YCUwm9giW+EEKkTpdjVA8O8k7HhXZ4J+2mSSFUphnqry0xlpiKYIZdi5k7swP",_key,_iv);




        // FUND TRANSFER

        // POST FUND TRANSFER
        ///v1/Wallet/PostFundTransfer
        public static string PostFundTransfer = "L3YxL1dhbGxldC9Qb3N0RnVuZFRyYW5zZmVy";

        // GET COMMISSION BALANCE ;
        public static string GetCommissionBalance = "L3YxL1dhbGxldC9HZXRDb21taXNzaW9uQmFsYW5jZS8=";

        // GET COMMISSION TRANSACTION LIST
        //v1/Wallet/GetCommissionDetails/

        public static string GetCommissionDetailsList = "L3YxL1dhbGxldC9HZXRDb21taXNzaW9uRGV0YWlscy8=";

        public static string GetTransferProcessingFee = "L3YxL1dhbGxldC9HZXRGdW5kVHJhbnNmZXJQcm9jZXNzaW5nRmVl";

        public static string GetWtaxFee = "L3YxL1dhbGxldC9HZXRGdW5kVHJhbnNmZXJXVGF4";

        public static string UpdateContactNoApi = "L3YxL0FjY291bnQvVXBkYXRlTm9Db250YWN0RGV0YWlscw==";

        public static string GetModuleStatus = "L3YxL1N5c3RlbS9HZXRTeXN0ZW1GdW5jdGlvblN0YXR1cw==";

        // RAFFLE (GET /util/v1/...)
        public static string LoadRaffle = "L3V0aWwvdjEvTG9hZFJhZmZsZQ==";
        public static string LoadMemberRaffleTickets = "L3V0aWwvdjEvTG9hZE1lbWJlclJhZmZsZVRpY2tldHM=";
        public static string GetRaffleTicketCtr = "L3V0aWwvdjEvR2V0UmFmZmxlVGlja2V0Q3Ry";

        // ANNOUNCEMENTS (GET only for user-facing apps)
        // NOTE: No base64/encryption required per integration request.
        public static string GetActiveAnnouncements = "/v1/Announcements/GetActiveAnnouncements";
        public static string GetAllAnnouncements = "/v1/Announcements/GetAllAnnouncements";
        public static string GetAnnouncementById = "/v1/Announcements/GetAnnouncementById/";
     
        /* < November 11 2024 >
         *  - endpoints fix remaining ep
         *  - getCashoutBankList
         *  - getCashoutTransactionDetails/tRefNo=[]
         *  - getCashhoutWaelletTransactions/accountId=[]
         *  - postCashout
         * 
         * 
         * 
         */
    }


    public class _defaults_wal
    {
        public static ApiResponseModel<object> SuccessApiResponse =
           new ApiResponseModel<object>
           {
               IsSuccess = true,
               StatusCode = 200,
               //ErrorMessage = "None",
               Description = "OK"
           };

        public static ReportsDataCommissionCoverage invalidDateCoverageList =
           new ReportsDataCommissionCoverage
           {
               dfrom = "invalid_request",
               dto = "invalid_request",

           };

        public static ApiResponseModel<object> defaultApiResponse =
           new ApiResponseModel<object>
           {
               IsSuccess = false,
               StatusCode = 500,
               //ErrorMessage = "Bad Request",
               Description = "Bad Request"
           };

        public static GetTransferProcessingFeeModel invalidTransferProcessingFee = new GetTransferProcessingFeeModel
        {
            tranDesc = "INVALID",
            amount = 0
        };

        public static GetWtaxFeeModel invalidWtaxFee = new GetWtaxFeeModel
        {
            tranDesc = "INVALID",
            amount = 0
        };

        public static WalletAccountDetailData invalidWalletAccount =
        new WalletAccountDetailData
        {
            primaryInfo = null,
            contactInfo = null,
            addressInfo = null,
            credential = null,
        };


        public static InstapayBalanceModel invalidInstapay =
        new InstapayBalanceModel
        {
            account_name = null,
            amount=0
        };

        public static WalletBalanceModel invalidBalance =
        new WalletBalanceModel
        {
            account_id = null,
            account_balance = 0
        };
        public static GetAppInfoModel invalidAppInfo => new GetAppInfoModel
        {
            appID = -1,
            appName = "Unknown",
            appVersionCode = "0",
            buildStatus = -1,
            releaseNotes = "Error retrieving app info",
            dateReleased = DateTime.MinValue
        };
        public static CommissionBalanceResponse invalidCommissionBalance =
       new CommissionBalanceResponse
       {
           account_id = null,
           account_balance = 0
       };

        public static AccountDetailData invalidAccountDetailDataResponse =
        new AccountDetailData
        {
            primaryInfo = new PrimaryInfoModel
            {
                refno = "invalid_user",
                idNumber = "invalid_user",
                primaryID = "invalid_user",
                sponsorID = "invalid_user", // You may need to provide appropriate values
                uplineID = "invalid_user",  // You may need to provide appropriate values
                grp = 0,  // You may need to provide appropriate values
                dateRegistered = DateTime.Parse("2016-09-09T07:57:00"),
                mdate = DateTime.Parse("2016-09-09T07:57:00"), // You may need to provide appropriate values
                paydate = DateTime.Parse("2016-09-09T07:57:00"), // You may need to provide appropriate values
                bdate = DateTime.Parse("1900-01-01T00:00:00"), // You may need to provide appropriate values
                fname = "invalid_user",
                mname = "invalid_user",
                sname = "invalid_user"
            },
            rankInfo = new RankInfoModel
            {
                package = "invalid_user",
                position = "invalid_user", // You may need to provide appropriate values
                rank = "invalid_user", // You may need to provide appropriate values
                title = "invalid_user" // You may need to provide appropriate values
            },
            contactInfo = new ContactInfoModel
            {
                email = "invalid_user",
                contactNo = "invalid_user"
            },
            addressInfo = new AddressInfoModel
            {
                country = "invalid_user",
                province = "invalid_user",
                city = "invalid_user",
                brgy = "invalid_user",
                homeAddress = "invalid_user"
            }
        };

        /*
        public static ReportsDataCommissionSummary invalidReportsData = new ReportsDataCommissionSummary()
        {
            dsc
            infsc
            giveMe5
            just4u
            checkMatch
            gsc
            rspc
            public double lsc { get; set; }
            public double proj001 { get; set; }
            public double grossComm { get; set; }
            public double wtax { get; set; }
            public double netComm { get; set; }
        };
        */


    }




}
