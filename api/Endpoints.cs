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
using Microsoft.Extensions.Configuration;
using IAM_Library.Utility;
using System.Text;


namespace IAM_Library.api
{
    public class _AuthEndpoints
    {
        public string authSignatureGeneration { get; set; }
        public static void InitializeKeyAndIV(string key, string iv)
        {
            // Ensure key and IV are not null or empty
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
                throw new ArgumentException("Key and IV cannot be null or empty.");
        }

    }

    internal static class _constants
    {
        private static string key = "Dev@201912907125";
        private static string iv = "201912907@JohnTC";
        private static byte[] _key = Encoding.UTF8.GetBytes(key);
        private static byte[] _iv = Encoding.UTF8.GetBytes(iv);
        // API Settings
        public static string authHeader= "QmVhcmVy";
        // Base URLs
        public static string live = IAM_Maintenance.Decrypt("TFAiKzHUNpWnaGHPWITuJeaIXVXgbrEsAzb33S1xvO8vimA2HHj6wqWYAfOyLqw7yOgUM02qP6lZuqggSN1c5g==",_key,_iv);
        public static string beta= IAM_Maintenance.Decrypt("CZ+ow8okUFtdDoc3GZhGbxB5cDbd6wJ5ByjucNxuNU8H8A7pNPgI+X5csA/zoFjill3K/sCK2q7SN25chrTRXQ==",_key,_iv);

        public static string authBaseUrl = live;

        public static string systemAccessBase = IAM_Maintenance.Decrypt("1Zl37WroqrRn9GkN8G+aa3+flz4j39nAV+8ghQXfi8seRbQHRPm+kwX4XB/lPvgu",_key,_iv);
        public static string systemAppIdParam = IAM_Maintenance.Decrypt("y089JGpggg99WjxQTcrQyA==",_key,_iv);

        public static string accountDetailsParams= IAM_Maintenance.Decrypt("NGvVNYR/Vunwb0Ky3aMcCQ==",_key,_iv);
        public static string endpointParameterAndSeparator= IAM_Maintenance.Decrypt("8t5OGwJCRXzt/tpP1pIOlA==",_key,_iv);
        public static string authSuccessDashEndpoint= IAM_Maintenance.Decrypt("TXfXz8T5bZlJ6OhDBzZ71EhBtu481cjWmJ3Y9ChZMIfbAr3eAzNE62JR1HzUcPstLsqpyolB1Lo0vWSCCcxoFw==",_key,_iv);
        public static string authSigGenEndpoint= IAM_Maintenance.Decrypt("42PRvWclKzMrbk/8fpShTQ==",_key,_iv);
        public static string getAccountRank= IAM_Maintenance.Decrypt("TXfXz8T5bZlJ6OhDBzZ71Kh1Ab0woQPS6Ye2LM/RmTjCP2FikQDkX7XmjruREVLZ",_key,_iv);
        public static string reportsCoverageEndpoint= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+PEd6BO7hzvB7gpJTOcqogzRiWCCICMXF5FUSGDSV54d",_key,_iv);
        public static string reportsCSummaryEndpointBase= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+Aa4W2ISIjjtT66KIydKPq+uKRvVa3WiCQZKzHlsnKyWosLhaO1eXVpiy3fJV1SieQ==",_key,_iv);
        public static string reportsCSummaryOptionParam= IAM_Maintenance.Decrypt("8g4R/FgGkfccwa8P38FGXA==",_key,_iv);
        public static string reportsCSummaryAccKey= IAM_Maintenance.Decrypt("s37Ja/xt3ZMPU/Yg42kudL7zT7zeYZvbtCFNWJ84ULs=",_key,_iv);
        public static string reportsCSummaryEndpointDfromParam= IAM_Maintenance.Decrypt("OEWxlyupv56IHTfBZqaQFw==",_key,_iv);
        public static string reportsCSummaryEndpointDtoParam= IAM_Maintenance.Decrypt("oEnL33NstaHMPMn3L51cCA==",_key,_iv);
        public static string reportsCHistoryEndpointBase= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+Aa4W2ISIjjtT66KIydKPq8hNaF0uW846uU0apjejbYc",_key,_iv);
        public static string residualSalesMatched= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+GFLi8EN1KsRIIeNZT06wUzL3lUgF37I1fIe79E7pIit",_key,_iv);
        public static string reportsCReferralEndpointbase= IAM_Maintenance.Decrypt("z4XNUv9SETQYavee23HF/0C3rtYwnobyP1WrGu0LUBk00r57NBRVuu+fAodUIefc",_key,_iv);
        public static string reportsBinarySummary= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+Aa4W2ISIjjtT66KIydKPq8hNaF0uW846uU0apjejbYc",_key,_iv);
        public static string reportsCheckMatchBase= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+D8iI1ms70aBVYja7yx1eQhiwlURPm/zmfBAin+1g32v",_key,_iv);
        public static string reportsRSBase= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+GFLi8EN1KsRIIeNZT06wUzL3lUgF37I1fIe79E7pIit",_key,_iv);
        public static string rqvHistoryBase= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+HM4lVQmuZGAxuMPuBJYDkhfPUAVcbGmOc3Y3u6f6vke",_key,_iv);
        public static string moverHistoryBase= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+ACUXISGPI02UphmStEOPTgPw4awIosTJtVVOvBa/Hjm",_key,_iv);
        public static string infinityHistoryBase= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+Fq5UaQSTE/v09t02+esd1WmK+h3iG5X11uX/ZkdyE98",_key,_iv);
        public static string checkDuplicateUserName= IAM_Maintenance.Decrypt("Jwr/oXRyWqJ0yXrlxwr9G60l3lsjQX6mN/hyvFoQh1/mt+WI5MUy3vDEz4B9uUvc",_key,_iv);
        public static string usernameParam= IAM_Maintenance.Decrypt("wSNLActDH4qrRjWHs4TEcA==",_key,_iv);
        public static string getTotalUnicom= IAM_Maintenance.Decrypt("z4XNUv9SETQYavee23HF/22nvUEDK82o94/8uEiAj7kNtVbJ0B1QPdYp8Tvwmg/Q",_key,_iv);
        public static string reportsUnilevelSalesCommissionBase= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+HsjUHYXeaJ4r/mdYx3gUQVQvinX7phbmELxS+hjGvn1",_key,_iv);
        public static string reportsPairsHistory= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+AUg2G9y6MINqNLKBMj+jiTgy9XSqLd88voFCrrgtbQC",_key,_iv);
        public static string geneologyBase= IAM_Maintenance.Decrypt("hkClen2xiwqILfesrHh60vDVYhPIITTta3QfzFfplL+vqBl11hBFsr6AyiY4Xmtr",_key,_iv);
        public static string geneologySummary= IAM_Maintenance.Decrypt("hkClen2xiwqILfesrHh60kbdv4bH6fZGb4Epivc+1ul+dxFAXGZ9AD3NWEIybcfA",_key,_iv);
        public static string walletCommHistory= IAM_Maintenance.Decrypt("i3+rp5LK5tZrswGnVmiaorM6Wn+CISKnsKKVShJ3JJqHJOI6VihKIn2hPXezqbg9AAKIqiTPEtqTX4V6g6TJXQ==",_key,_iv);
        public static string walletBalance= IAM_Maintenance.Decrypt("YXokBg8+bKKqyJ/L4cqfMFnNP3m2fLkQwg+fazNfJqb8dkcguIYj9WUNWV4SYjZQ",_key,_iv);
        public static string walletEncashmentDetails= IAM_Maintenance.Decrypt("/DlV3SLa1Qi+sq34DrPK+w/7VvLWCWqR7nrarirZdV9RJeO1MEkO6V3AEp6J7Mn3",_key,_iv);
        public static string walletJ4UBase= IAM_Maintenance.Decrypt("i3+rp5LK5tZrswGnVmiaoqZ6mK7n7/wgX6J6pGL34Nk=",_key,_iv);
        public static string postWalletWithdrawComm= IAM_Maintenance.Decrypt("5+kM2KBHgIyjLDm3esRUvrmooYYgr6QlbabKf4Dn2GFs9t4jY383V4ODMaXNS/kw",_key,_iv);
        public static string walletJ4UUpdateSelected= IAM_Maintenance.Decrypt("Ar2fCcV1zu0ztr0gU+yV6a92+rDB34T6E+nR0ZsyLBqmn91dH6qz26cJMS8awd71",_key,_iv);
        public static string generateAutoNum= IAM_Maintenance.Decrypt("sDnnHCExuuKcrgQ1y1fS/k9uwojeDCT3oxkuaEL/RVgb/Uxzn1GQpw8V6Dn4Mjp/",_key,_iv);
        public static string invalidIndicator= IAM_Maintenance.Decrypt("+SjS5CG88/VUZq3Sk3KjGN+X7qkGQ58oXYXsYNuq+RU=",_key,_iv);
        public static string verificationActivation= IAM_Maintenance.Decrypt("4Q3ebNdCt6jAotb6ecjBszb+fUULOPRDo7A6i1mFVzy04f5dOVcRZWcVGb47ydnj",_key,_iv);
        public static string getextremeupline= IAM_Maintenance.Decrypt("3Tgv6NbaszBqrhUC+ruWMvD/2LRAWCGrlqUeHMBAkivo6gDtFHHKufvotiXhfh/D",_key,_iv);
        public static string sponsorIdParam= IAM_Maintenance.Decrypt("ixx8QSfgKE5vHbbN9WQEWhzu6hvdnenskpaeHP/2o7M=",_key,_iv);
        public static string uplineIdParam= IAM_Maintenance.Decrypt("AWGauQJ8Yv2GQgp8etm/Eg==",_key,_iv);
        public static string activeNoParam= IAM_Maintenance.Decrypt("jFDjmxq8O+ek2NJQpxlseA==",_key,_iv);
        public static string pinNoParam= IAM_Maintenance.Decrypt("igiUz1Cze+yb1dt82KEm+g==",_key,_iv);
        public static string grpParam= IAM_Maintenance.Decrypt("F4byR7pmJwrITz00kTRqgA==",_key,_iv);
        public static string latestParam= IAM_Maintenance.Decrypt("OSHgWjVFSFAlWdGkv5zLuw==",_key,_iv);
        public static string RegistrationBase= IAM_Maintenance.Decrypt("wHLi/d/rNnkWTCDxuk2uWLKnDa/VoSTMyxCddwQT3hM=",_key,_iv);
        public static string checkCrosslineBase= IAM_Maintenance.Decrypt("Jwr/oXRyWqJ0yXrlxwr9G7AafvSN7ADe3AH5sxDjZiJegoxJyNwcqNtPq7+rkPHV",_key,_iv);
        public static string getEncashmentDetailsBase= IAM_Maintenance.Decrypt("/DlV3SLa1Qi+sq34DrPK+w/7VvLWCWqR7nrarirZdV9RJeO1MEkO6V3AEp6J7Mn3",_key,_iv);
        public static string getWalletBalanceBase= IAM_Maintenance.Decrypt("YXokBg8+bKKqyJ/L4cqfMFnNP3m2fLkQwg+fazNfJqb8dkcguIYj9WUNWV4SYjZQ",_key,_iv);
        public static string loadWalletCommissionBase= IAM_Maintenance.Decrypt("i3+rp5LK5tZrswGnVmiaorM6Wn+CISKnsKKVShJ3JJqHJOI6VihKIn2hPXezqbg9AAKIqiTPEtqTX4V6g6TJXQ==",_key,_iv);
        public static string postEncashmentBase= IAM_Maintenance.Decrypt("5+kM2KBHgIyjLDm3esRUvrmooYYgr6QlbabKf4Dn2GFs9t4jY383V4ODMaXNS/kw",_key,_iv);
        public static string loadEncashmntTypes= IAM_Maintenance.Decrypt("i3+rp5LK5tZrswGnVmiaovKyFi0drA0YnTZFgX+gHjKnvZbvLu0OvPjufSD8A0mZ",_key,_iv);
        public static string idNumberParam= IAM_Maintenance.Decrypt("otxqTtyeIuRjovP2hmTlKg==",_key,_iv);
        public static string idNumberParamQR= IAM_Maintenance.Decrypt("6t+c2Z0E4lwCmNNzuwEbJA==",_key,_iv);
        public static string GETBankDetailsById= IAM_Maintenance.Decrypt("g7x3XXXvlkW6z0d0+kJkVzgmIZWTyS24Qg4V17itbgTfeUO989IPaqf5tyH66i8e",_key,_iv);
        public static string tranTypeParam= IAM_Maintenance.Decrypt("IV+5qlPyPc0orfIqws20mg==",_key,_iv);
        public static string refNoParam= IAM_Maintenance.Decrypt("7yzxIYfzoNzQmGUjS7PG9Q==",_key,_iv);
        public static string encashmentTypeParam= IAM_Maintenance.Decrypt("YP7GRpWYRbYVbeLqxJlHb9O2jIRV6AQ7tVNK762vOIo=",_key,_iv);
        public static string J4UWithdrawWalletBase= IAM_Maintenance.Decrypt("5+kM2KBHgIyjLDm3esRUvhuhnt7m/XFNG7lkfmzYPjANp3OO3vwqscMrAu8dXHrk",_key,_iv);
        public static string GETCheckAccountID= IAM_Maintenance.Decrypt("Jwr/oXRyWqJ0yXrlxwr9G25tLctn1VjJzPKGpfWJaq0=",_key,_iv);
        public static string PUTUpdatePrimaryDetails= IAM_Maintenance.Decrypt("qKeOwdQXjrvsVCWgIZzyEe0H/N8mrAOY0VDqcEaA44j7evxI/DM0Tka/Sqxy85UH",_key,_iv);
        public static string PUTUpdateBankDetails= IAM_Maintenance.Decrypt("qKeOwdQXjrvsVCWgIZzyEV0DipD/Wz5LKJfuXuPgRmhDbT6af24axgmr4N85ipn4",_key,_iv);
        public static string POSTUploadID= IAM_Maintenance.Decrypt("c2BF340aVyOEkHtU7fao1DFVAKanq0c8U706u0RtnZI=",_key,_iv);
        public static string GETLoadMemo= IAM_Maintenance.Decrypt("rg4NpFoWHg0oHcBHlVWto3NjImPkUTnBq1HmDiD8QGE=",_key,_iv);
        public static string GETLoadBankList= IAM_Maintenance.Decrypt("kVlJSdFlIl2h+j4BLjczmQ/aRIDsh33a1vzLoJaJ8dE=",_key,_iv);
        public static string GETLoadIDList= IAM_Maintenance.Decrypt("kVlJSdFlIl2h+j4BLjczmetQsFzrCvjO+g/oeo6v1vM=",_key,_iv);
        public static string GETIDDetails= IAM_Maintenance.Decrypt("NGvVNYR/Vunwb0Ky3aMcCQ==",_key,_iv);
        public static string GETCommCtr= IAM_Maintenance.Decrypt("z4XNUv9SETQYavee23HF/wvVOIBcbrimx/j1JCKk7KAsN3BV0W9nJnKjwGlDlNK9",_key,_iv);
        public static string GETAccountKey= IAM_Maintenance.Decrypt("oOuGSrOynh6R9X7XyopsbDsED0wfhMnkbj2sv9dx6ddwCYnW1y8UlwguZbvDqxrA",_key,_iv);
        public static string valueParameter= IAM_Maintenance.Decrypt("E7r3NzqfAIwP1YpPPJ2GmQ==",_key,_iv);
        public static string checkMatchedBase= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+D8iI1ms70aBVYja7yx1eQhiwlURPm/zmfBAin+1g32v",_key,_iv);
        public static string unilevelLeadership= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+AFrr3U2sFm/GT+bNeWyP6g=",_key,_iv);
        public static string rqvSummary= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+HM4lVQmuZGAxuMPuBJYDkhfPUAVcbGmOc3Y3u6f6vke",_key,_iv);
        public static string moverSummary= IAM_Maintenance.Decrypt("sjNxLmqEPRwoirg46g9C+ACUXISGPI02UphmStEOPTgPw4awIosTJtVVOvBa/Hjm",_key,_iv);
        public static string walletJ4UPreviewBase= IAM_Maintenance.Decrypt("uSrhriBuWJWikJbYLA0b0t7IqPd084uJf8KqTEO/o0EPueOqG787LhVNdtAE6uG4",_key,_iv);


        public static string purchasesListBase = IAM_Maintenance.Decrypt("pKayjX+JoDgrRbFMkJBEX+MG7/KGwBEcbzSqtbFQgc5xF3CyPnBRYsYmluGdaohx", _key, _iv);

   
    }




    public class _defaults
    {
        public static ApiResponseModel<object> SuccessApiResponse =
           new ApiResponseModel<object>
           {
               IsSuccess = true,
               StatusCode = 200,
               //ErrorMessage = "None",
               Description= "OK"
           };

        public static ReportsDataCommissionCoverage invalidDateCoverageList =
           new ReportsDataCommissionCoverage
           {
               dfrom= "invalid_request",
               dto= "invalid_request",

           };

        public static ApiResponseModel<object> defaultApiResponse =
           new ApiResponseModel<object>
           {
               IsSuccess = false,
               StatusCode = 500,
               //ErrorMessage = "Bad Request",
               Description= "Bad Request"
           };






        public static AccountDetailData invalidAccountDetailDataResponse =
        new AccountDetailData
        {
            primaryInfo = new PrimaryInfoModel
            {
                refno= "invalid_user",
                idNumber= "invalid_user",
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
