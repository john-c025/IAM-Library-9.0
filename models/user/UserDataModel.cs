using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.models.user
{
    public class UserDataModel
    {
        public string username {  get; set; }
        public string userAccountKey { get; set; }
        public string userToken { get; set; }
        public DateTime userTokenExpiry { get; set; }
    }
}
