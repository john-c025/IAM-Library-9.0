using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAM_Library.models.general
{
    public class ApiResponseModel<T> //general
    {

        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        //public string ErrorMessage { get; set; }
        public string Description { get; set; }
        public T Data { get; set; }
        public T MiscData { get; set; }


    }

    public class ErrorModel
    {
        public string Message { get; set; }
        // Add other properties if needed
    }

    

    /*
     * 
     * "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "00-156e9a35c702b2560542bda93d8ad88e-949cf2aa160174fd-00",
    "errors": {
        "dto": [
            "The value '00/01/2024' is not valid."
        ]
    }
     * 
     * 
     * 
     * 
     */



}
