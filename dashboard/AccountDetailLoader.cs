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


namespace IAM_Library.dashboard
{


    public class AccountDetailLoader //test WIP
    {
        AccountDetailData getAccountData;

        public async Task<AccountDetailData> LoadAccountData(AuthApiResponseData credentials, HttpClient _httpClient)
        {
            var apiBaseUrl = $"{Encryption.decodeString(_constants.authBaseUrl)}";

            AuthApiResponseData myCreds = credentials;
            var accountAPiClient = new AccountsApiClient(apiBaseUrl, myCreds.signature, _httpClient);

            try
            {
                getAccountData = await accountAPiClient.GetAccountDataAsync(credentials.accountKey); //test inject encoded creds
            }
            catch (Exception e)
            { 
                getAccountData = _defaults.invalidAccountDetailDataResponse;
            }
            return getAccountData;


        }
    }

}

