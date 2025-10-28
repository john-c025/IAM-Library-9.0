using IAM_Library.models.general;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.appWallet.models
{
    public class WalletAuthResponseData //Auth signature
    {
            public string accountKey { get; set; }
            public string signature { get; set; }
            public string sessionID { get; set; }
        
    }

    public class WalletAuthModel
    {
        public WalletAuthResponseData AuthData { get; set; }
        public ApiResponseModel<object> apiResponse { get; set; }
    }

    public class WalletAuthData
    {
        public int option { get; set; }
        public string usrname { get; set; }
        public string cCodedusrname { get; set; }
        public string cCodedpword { get; set; }
        
    }

    public class DeviceDetails
    {
        public string deviceID { get; set; }
        public string deviceName { get; set; }
        public string ipAddress { get; set; }
    }
}
