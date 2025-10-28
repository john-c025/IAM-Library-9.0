using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library._custom
{
    using System;

    public class ReportsApiClientException : Exception
    {
        public ReportsApiClientException(string message) : base(message)
        {
        }
    }

    public class ApiRequestException : ReportsApiClientException
    {
        public ApiRequestException(string message) : base(message)
        {
        }
    }

    public class ApiResponseException : ReportsApiClientException
    {
        public ApiResponseException(string message) : base(message)
        {
        }
    }

}

