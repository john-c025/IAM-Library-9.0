using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAM_Library._custom;


namespace IAM_Library.api
{
    public class ApiConfiguration
    {
        public string BaseUrl { get; }
        public Dictionary<string, string> Endpoints { get; }

        public ApiConfiguration(string baseUrl, Dictionary<string, string> endpoints)
        {
            BaseUrl = baseUrl;
            Endpoints = endpoints;
        }
    }

}
