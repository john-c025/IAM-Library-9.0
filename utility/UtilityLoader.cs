using IAM_Library._custom;
using IAM_Library.api;
using IAM_Library.models.auth;
using IAM_Library.models.dashboard;
using IAM_Library.models.general;
using IAM_Library.models.wallet;
using IAM_Library.wallet.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.utility
{
    public class UtilityLoader
    {
        private readonly UtilityAPIClient _utilityAPIClient;

        public UtilityLoader(AuthApiResponseData credentials, AccountDetailData loggedAccData, HttpClient httpClient)
        {
            string apiBaseUrl = Encryption.decodeString(_constants.authBaseUrl);
            _utilityAPIClient = new UtilityAPIClient(apiBaseUrl, credentials, loggedAccData, httpClient);
        }

        public async Task<ApiResponseModel<AccountDetailData>> VerifyUser(string idNumber)
        {
            try
            {
                var accountData = await _utilityAPIClient.CheckAccountData(idNumber);
                return accountData;
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<AccountDetailData>
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
