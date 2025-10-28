using IAM_Library.models.auth;
using IAM_Library.models.dashboard;
using IAM_Library.models.general;
using IAM_Library.models.reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.api
{
    public class _AuthEndpoints_old
    {
        public string authSignatureGeneration { get; set; }
    }


    internal class _constants_old
    {
        public const string authHeader = "QmVhcmVy";
        public const string accountDetailsParams = "";
        //base url 
        public static string live = "aHR0cHM6Ly9hcGl2Mi5pYW0td29ybGR3aWRlY29ycC5jb20=";
        public static string beta = "aHR0cHM6Ly9iZXRhYXBpdjIuaWFtLXdvcmxkd2lkZWNvcnAuY29t";
        
        public static string authBaseUrl = live;
        
        //separator
        public const string endpointParameterAndSeparator = "Jg==";//&
        // endpoints
        public const string authSuccessDashEndpoint = "L2FjYy92MS9HZXRBY2NvdW50RGV0YWlscz9BY2NvdW50S2V5PQ=="; //get account details, basic profile
        public const string authSigGenEndpoint = "L2F1dGgvdjE="; // auth header
        //reports - coverage
        public const string getAccountRank = "L2FjYy92MS9HZXRBY2NvdW50UmFuaw==";
        public const string reportsCoverageEndpoint = "L2NvbW0vdjEvTG9hZEJJTkNvbW1Db3ZlcmFnZQ=="; //get coverage list of dates from and to
        // reports - commission summary
        public const string reportsCSummaryEndpointBase = "L2NvbW0vdjEvTG9hZEJpbmFyeUNvbW1pc3Npb25TdW1tYXJ5";
        //"L2NvbW0vdjEvTG9hZEJpbmFyeUNvbW1pc3Npb25TdW1tYXJ5Pw==";
        public const string reportsCSummaryOptionParam = "b3B0aW9uPQ==";
        public const string reportsCSummaryAccKey = "YWNjb3VudGtleT0=";
        public const string reportsCSummaryEndpointDfromParam = "ZGZyb209"; // dfrom =
        public const string reportsCSummaryEndpointDtoParam = "ZHRvPQ=="; // dto=
        //reports- history
        public const string reportsCHistoryEndpointBase = "L2NvbW0vdjEvTG9hZEJpbmFyeUNvbW1pc3Npb24=";
        //reports - referral
        public const string residualSalesMatched = "L2NvbW0vdjEvTG9hZFJTTUhpc3Rvcnk=";
        //reports - dsc 
        public const string reportsCReferralEndpointbase = "L2NvbW0vdjEvR2V0UmVmZXJyYWxDb21taXNzaW9u";
        // 1 - referral
        // 2 - DSC
        // 3 - SCC
        // 4 - J4U
        // 5 - GM5
        public const string reportsBinarySummary = "L2NvbW0vdjEvTG9hZEJpbmFyeUNvbW1pc3Npb24=";
        public const string reportsCheckMatchBase = "L2NvbW0vdjEvTG9hZENoZWNrTWF0Y2hIaXN0b3J5"; ///comm/v1/LoadCheckMatchHistory;
        public const string reportsRSBase = "L2NvbW0vdjEvTG9hZFJTTUhpc3Rvcnk=";
        public const string rqvHistoryBase = "L2NvbW0vdjEvTG9hZFJRVkhpc3Rvcnk=";
        public const string moverHistoryBase = "L2NvbW0vdjEvTG9hZE1vdmVyU3VtbWFyeQ==";
        public const string infinityHistoryBase = "L2NvbW0vdjEvTG9hZElORkNDb21taXNzaW9u";
        public const string checkDuplicateUserName = "L2FjYy92MS9DaGVja0R1cGxpY2F0ZVVzZXJuYW1l";
        public const string usernameParam = "dXNlcm5hbWU9";
        public const string getTotalUnicom = "L2NvbW0vdjEvR2V0VG90VW5pQ29tbQ==";
        //unilevel
        public const string reportsUnilevelSalesCommissionBase = "L2NvbW0vdjEvTG9hZFVuaWxldmVsQ29tbWlzc2lvbg==";
        //pairs
        public const string reportsPairsHistory = "L2NvbW0vdjEvTG9hZFBhaXJzSGlzdG9yeQ==";
        //geneology
        public const string geneologyBase = "L2dlbmVhL3YxL0xvYWRHZW5lYWxvZ3k=";
        public const string geneologySummary = "L2dlbmVhL3YxL0xvYWRHZW5lYWxvZ3lTdW1tYXJ5";
        //wallet
        public const string walletCommHistory = "L3dhbC92MS9Mb2FkV2FsbGV0Q29tbWlzc2lvbkhpc3Rvcnk=";
        public const string walletBalance = "L3dhbC92MS9HZXRXYWxsZXRCYWxhbmNl";
        public const string walletEncashmentDetails = "L3dhbC92MS9HZXRFbmNhc2htZW50RGV0YWlscw==";
        public const string walletJ4UBase = "L3dhbC92MS9Mb2FkV2FsbGV0SjRV";
        public const string postWalletWithdrawComm = "L3dhbC92MS9XaXRoZHJhd1dhbGxldENvbW0=";
        public const string walletJ4UUpdateSelected = "L3dhbC92MS9VcGRhdGVTZWxlY3RlZEo0VQ==";
        //utilities
        public const string generateAutoNum = "L3V0aWwvdjEvR2VuZXJhdGVBdXRvTnVtYmVy";
        // misc..
        public const string invalidIndicator = "aW52YWxpZF91c2Vy";
        //trantype
        //idnum
        //refno
        public const string verificationActivation = "L3V0aWwvdjEvVmVyaWZ5QWN0aXZhdGlvbg==";
        public const string getextremeupline = "L2FjYy92MS9HZXRFeHRyZW1lVXBsaW5l";
        //PARAMETERS
        public const string sponsorIdParam = "c3BvbnNvcmlkPQ==";
        public const string uplineIdParam = "dXBsaW5laWQ9";
        public const string activeNoParam = "YWN0aXZlbm89";
        public const string pinNoParam = "cGlubm89";
        public const string grpParam = "Z3JwPQ==";
        public const string latestParam = "bGF0ZXN0PQ==";
        //?sponsorid=00002063&activeno=7576735860&pinno=002006
        public const string RegistrationBase = "L2FjYy92MS9SZWdpc3Rlcg==";
        public const string checkCrosslineBase = "L2FjYy92MS9DaGVja0Nyb3NzbGluZQ==";
        //WALLET ---------------------------------------------------------------------------------------------
        public const string getEncashmentDetailsBase = "L3dhbC92MS9HZXRFbmNhc2htZW50RGV0YWlscw==";
        public const string getWalletBalanceBase = "L3dhbC92MS9HZXRXYWxsZXRCYWxhbmNl";
        public const string loadWalletCommissionBase = "L3dhbC92MS9Mb2FkV2FsbGV0Q29tbWlzc2lvbkhpc3Rvcnk=";
        public const string postEncashmentBase = "L3dhbC92MS9XaXRoZHJhd1dhbGxldENvbW0=";
        public const string loadEncashmntTypes = "L3dhbC92MS9Mb2FkRW5jYXNobWVudFR5cGU=";
        //Wallet Params
        public const string idNumberParam = "aWRubz0=";
        public const string idNumberParamQR = "aWRudW1iZXI9";
        public const string GETBankDetailsById = "L2FjYy92MS9HZXRCYW5rQWNjb3VudERldGFpbHM=";
        public const string tranTypeParam = "dHJhbnR5cGU9";
        public const string refNoParam = "cmVmbm89";
        public const string encashmentTypeParam = "ZW5jYXNobWVudHR5cGU9";
        public const string J4UWithdrawWalletBase = "L3dhbC92MS9XaXRoZHJhd0o0VUNvbW0=";
        // new api endpoints 
        public const string GETCheckAccountID = "L2FjYy92MS9DaGVja0FjY291bnQ=";
        public const string PUTUpdatePrimaryDetails = "L2FjYy92MS9VcGRhdGVQcmltYXJ5RGV0YWlscw==";
        public const string PUTUpdateBankDetails = "L2FjYy92MS9VcGRhdGVCYW5rRGV0YWlscw==";
        public const string POSTUploadID = "L2FjYy92MS9VcGxvYWRJRA==";
        public const string GETLoadMemo = "L21lbW8vdjEvTG9hZE1lbW8=";
        public const string GETLoadBankList = "L3V0aWwvdjEvTG9hZEJhbmtMaXN0";
        public const string GETLoadIDList = "L3V0aWwvdjEvTG9hZElETGlzdA==";
        public const string GETIDDetails = "";
        public const string GETCommCtr = "L2NvbW0vdjEvR2V0Q29tbWlzc2lvbkN0cg==";
        public const string GETAccountKey = "L3V0aWwvdjEvR2V0QWNjb3VudEtleQ==";
        public const string valueParameter = "dmFsdWU9";

        // pahabol reports
        public const string checkMatchedBase = "L2NvbW0vdjEvTG9hZENoZWNrTWF0Y2hIaXN0b3J5";
        public const string unilevelLeadership = "L2NvbW0vdjEvTG9hZFVMQw==";
        public const string rqvSummary = "L2NvbW0vdjEvTG9hZFJRVkhpc3Rvcnk=";
        public const string moverSummary = "L2NvbW0vdjEvTG9hZE1vdmVyU3VtbWFyeQ==";
        public const string walletJ4UPreviewBase = "L3dhbC92MS9HZXRKNFVBbW91bnRUb0VuY2FzaA==";


    }


    public class _defaults_old
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
