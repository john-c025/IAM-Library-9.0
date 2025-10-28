using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAM_Library.models.general;

namespace IAM_Library.models.auth
{
    
    public class AuthApiResponseData //Auth signature
    {
        public string accountKey { get; set; }
        public string signature { get; set; }
        

    }

    public class AuthWalletApiResponseData //Auth signature
    {
        public string accountKey { get; set; }
        public string signature { get; set; }
        public string sesssionID { get; set; }
        

    }

    public class AuthResponseModel
    {
        public AuthApiResponseData AuthData { get; set; }
        public ApiResponseModel<object> apiResponse { get; set; }
    }
    public class AuthResponseWalletModel
    {
        public AuthWalletApiResponseData WalletAuthData { get; set; }
        public ApiResponseModel<object> apiResponse { get; set; }
    }

    public class CredentialModel
    {
        private string usr;
        private string pw;

        public string usrname { get; set; }
        public string password { get; set; }

        public CredentialModel(string usr, string pw)
        {
            this.usr = usr;
            this.pw = pw;

            usrname = this.usr;
            password = this.pw;
        }

        //get expiry
        public class jwtToken
        {
            public long exp { get; set; }
        }


    }
     


}
