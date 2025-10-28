using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using static IAM_Library.models.auth.CredentialModel;
using System.Net.Http.Headers;
using IAM_Library.api;
using System.Net.Http;
using IAM_Library.models.auth;
using System.Security.Claims;
using System.Security.Cryptography;


namespace IAM_Library._custom
{
    public class Encryption
    {
        public static string decodeString(string base64EncodedString)
        {
            try
            {
                byte[] data = Convert.FromBase64String(base64EncodedString);
                return Encoding.UTF8.GetString(data);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error decoding Base64 string: {ex.Message} : string is {base64EncodedString}");
                throw; // Re-throw the exception to handle it at a higher level
            }
        }

        public static string encodeString(string x) //Base64 Decode
        {
            var stringBytes = Encoding.UTF8.GetBytes(x);
            var encodedString = Convert.ToBase64String(stringBytes, Base64FormattingOptions.InsertLineBreaks);
            //Base64FormattingOptions.InsertLineBreaks

            return encodedString;
        }

        //md5
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            Console.WriteLine($"Encoded string is {strBuilder.ToString()}");
            return strBuilder.ToString();

        }


        // get expiry of token -AUTH
        public DateTime GetExpiryTimestamp(string accessToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accessToken))
                    return DateTime.MinValue;
                if (!accessToken.Contains("."))
                    return DateTime.MinValue;

                string[] parts = accessToken.Split('.');
                jwtToken payload = JsonSerializer.Deserialize<jwtToken>(Base64UrlEncoder.Decode(parts[1]));
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(payload.exp);
                return dateTimeOffset.LocalDateTime;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        // GET l


        /// implement encrypt and decrypt with static tokens/keys

    }

    public class CustomScript
    {
        public String UrlEndpointBuilder(List<String> urls)
        {
            List<String> returnList = new List<String>();
            foreach (string url in urls)
            { returnList.Add(url); }
            return string.Join("", returnList.ToArray());
        }

        //testing custom func

        public string CustomUrlBuilder(string _baseUrl, string baseEndpointPath, ApiConfiguration _apiConfig, List<String> options)
        {
            UriBuilder uriBuilder = new UriBuilder(_baseUrl);
            uriBuilder.Path = baseEndpointPath;

            //uriBuilder.Query = $"option={_apiConfig.EndpointCSummaryOptionParam}&{_apiConfig.EndpointCSummaryAccKey}={_accountKey}&{_apiConfig.EndpointCSummaryDfromParam}=01/01/2024&{_apiConfig.EndpointCSummaryDtoParam}=01/01/2024";

            string url = uriBuilder.ToString();
            return url;
        }

        // Build Url & Function for Sending Get & Post Request

        /*
        public async Task<string> SendGetRequestAsync(string fullAPIurl, HttpClient _httpClient)
        {
            var response = await _httpClient.GetAsync(fullAPIurl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        */

        public string BuildUrl(string _apiBaseUrl, string endpointName, ApiConfiguration _apiConfig, params (string key, string value)[] queryParams)
        {
            Console.WriteLine($"Api Base url to build on is {_apiBaseUrl}");
            var endpoint = _apiConfig.Endpoints[endpointName];
            var uriBuilder = new UriBuilder(_apiBaseUrl) { Path = endpoint };

            var query = string.Join("&", queryParams.Select(p => $"{p.key}{p.value}"));
            uriBuilder.Query = query;

            return uriBuilder.ToString();
        }

        public string BuildUrlV2(string _apiBaseUrl, string endpointName, ApiConfiguration _apiConfig, params (string key, string value)[] queryParams)
        {
            Console.WriteLine($"Api Base url to build on is {_apiBaseUrl}");
            var endpoint = _apiConfig.Endpoints[endpointName];
            var uriBuilder = new UriBuilder(new Uri(_apiBaseUrl)) { Path = endpoint };

            var query = string.Join("&", queryParams.Select(p => $"{Uri.EscapeDataString(p.key)}{Uri.EscapeDataString(p.value)}"));
            uriBuilder.Query = query;
            Console.WriteLine($"BUILDER URL {uriBuilder.ToString()}");
            return uriBuilder.ToString();
        }

        //Temporary
        public string temporaryUserGenerator()
        {
            Random random = new Random();
            int randomNumber = random.Next(100, 1000);

            // Get today's date in the format YYYYMMDD
            string todayDate = DateTime.Now.ToString("yyyyMMdd");

            // Combine the username, random number, and today's date
            string userIdentifier = $"user{randomNumber}{todayDate}";

            return userIdentifier;
        }

        //generalized async method for sending http requests:

        private async Task<string> SendRequestAsync(HttpClient _httpClient,string full_url, HttpMethod method, HttpContent content = null)
        {
            try
            {
                var request = new HttpRequestMessage(method, full_url);

                if (content != null)
                    request.Content = content;

                var response = await _httpClient.SendAsync(request);

                // Ensure that the response indicates success
                response.EnsureSuccessStatusCode();

                // Fetch the status code if needed
                var statusCode = response.StatusCode;

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors (e.g., non-success status codes)
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                throw; // Propagate the exception to the caller
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                Console.WriteLine($"Error during HTTP request: {ex.Message}");
                throw; // Propagate the exception to the caller
            }
        }

        private async Task<string> SendGetRequestAsync(string fullAPIurl, HttpClient _httpClient)
        {
            try
            {
                var response = await _httpClient.GetAsync(fullAPIurl);

                // Ensure that the response indicates success
                response.EnsureSuccessStatusCode();

                // Fetch the status code if needed
                var statusCode = response.StatusCode;

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP request errors (e.g., non-success status codes)
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                throw; // Propagate the exception to the caller
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions
                Console.WriteLine($"Error during HTTP request: {ex.Message}");
                throw; // Propagate the exception to the caller
            }
        }


    }

    public class JWTUtils
    {
        public string JWTGetExpiryAsMS(AuthApiResponseData authData)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(authData.signature) as JwtSecurityToken;

            string exp = jsonToken.Claims.FirstOrDefault(f=>f.Type == JwtRegisteredClaimNames.Exp).Value;

            return exp;
        }

        public static DateTime GetExpiryJWT(AuthApiResponseData authData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(authData.signature))
                    return DateTime.MinValue;
                if (!authData.signature.Contains("."))
                    return DateTime.MinValue;

                string[] parts = authData.signature.Split('.');
                jwtToken payload = JsonSerializer.Deserialize<jwtToken>(Base64UrlEncoder.Decode(parts[1]));
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(payload.exp);
                return dateTimeOffset.LocalDateTime;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }
    }

   

}

