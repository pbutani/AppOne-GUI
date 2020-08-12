using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace BlazorApp1.Server.Models
{
    public class ConnectionParameters
    {
        public static void SeLaunchUrlString(string url)
        {
            LaunchUri = url;
            ServiceUri = getUrlParameter(LaunchUri,"iss=");
            LaunchContextId = getUrlParameter(LaunchUri, "launch=");
            if (TokenRequestSuccess == null)
            {
                TokenRequestSuccess = new TokenRequestSuccessResponse();
                TokenRequestError = new TokenRequestErrorResponse();
                FhirLaunchParams = new FhirLaunchInfo();
            }
        }

        public static void SeAuthResponseUrl(string url)
        {
            AuthCodeResponse = url;
            AuthCode = getUrlParameter(AuthCodeResponse, "code=");
            string st = getUrlParameter(AuthCodeResponse, "state=");
            /*if (State.CompareTo(st) == 0)
            {
                Debug.WriteLine("state == state");
            }*/
            
        }

        /*public string GetUrlString()
        {
            return LaunchUri;
        }*/

        
        public static string ServiceUri { get; set; }

        public static string LaunchContextId { get; set; }


        /// <summary>
        /// An identifier sent via the URL used to access passed information.
        /// It's a Guid/string generated at run time and sent as a parameter in the LaunchURL.
        /// </summary>
        public static Guid State { get; set; }
        /// <summary>
        /// URL for this Launch page. It's set on the applications portal page.
        /// </summary>
        public static string LaunchUri { get; set; }
        /// <summary>
        /// Where the calling site is supposed to go after the launch page.
        /// It needs to match the one set in the applications portal page.
        /// </summary>
        /// <summary>
        /// Where information about which aspects of the standard this site complies to.
        /// </summary>
        public string ConformanceUri { get; set; }
        /// <summary>
        /// URL to where authentication happens.
        /// </summary>
        public static string AuthUri { get; set; }
        /// <summary>
        /// Where to go to get the token.
        /// </summary>
        public static string TokenUri { get; set; }
        public string SmartExtension { get; set; }

        //public string RedirectHref { get; set; }
        public static string AuthCodeRequest { get; set; }
        public static string AuthCodeResponse { get; set; }
        public static string AuthCode { get; set; }
        public static string AuthTokenRequestString { get; set; }
        public static string AuthTokenResponseString { get; set; }

        public static TokenRequestSuccessResponse TokenRequestSuccess { get; set; }
        public static TokenRequestErrorResponse TokenRequestError { get; set; }
        public static FhirLaunchInfo FhirLaunchParams { get; set; }

        public static string PatientResourceString { get; set; }

        private static string getUrlParameter(string Source, string sParam)
        {
            string retStr = "";
            int ndx1 = Source.IndexOf(sParam);
            if (ndx1 > 0)
            {
                ndx1 += sParam.Length;
                int ndx2 = Source.IndexOf('&', ndx1);
                if (ndx2 < 0)
                    ndx2 = Source.Length;
                
                string tempStr = Source.Substring(ndx1, ndx2 - ndx1);
                retStr = tempStr.Replace("%2F", "/");
            }
            return retStr;
        }

        // parameters of our app's registration with Cerner:
        public static readonly string ClientId = "782d91ee-bc38-4540-92ac-26accfc54393";
        public static readonly string Secret = "4a783dbf-57b9-45bf-95c4-e23b01fbe7e2";
        public static readonly string Scopes = "patient/Patient.read patient/Observation.read launch online_access openid profile";
        public static readonly string RedirectUri = "https://blazorapp3server303.azurewebsites.net";
        public static readonly string AppName = "MizuhoSmartApp";
        public static readonly string AppVersion = "1.0";

        // Cerner's FHIR server
        public static readonly string BaseURL = "https://fhir-ehr.sandboxcerner.com/r4";//"dstu2";
        public static readonly string ServiceIdentifier="0b8a0111-e8e6-4c26-a91c-5069cbc6b1ca";

    }

    public class TokenRequestSuccessResponse
    {
        [JsonProperty("access_token")] public string AccessToken = "";
        [JsonProperty("expires_in")] public string ExpiresIn = "";
        [JsonProperty("expires_on")] public string ExpiresOn = "";
        [JsonProperty("id_token")] public string IdToken = "";
        [JsonProperty("refresh_token")] public string RefreshToken = "";
        [JsonProperty("resource")] public string Resource = "";
        [JsonProperty("scope")] public string Scope = "";
        [JsonProperty("token_type")] public string TokenType = "";

        public string not_before = "";
        public string pwd_exp = "";
        public string pwd_url = "";
    }

    public class TokenRequestErrorResponse
    {
        [JsonProperty("error")]
        public string Error;
        [JsonProperty("error_description")]
        public string Description;
        [JsonProperty("error_codes")]
        public string[] ErrorCodes;
        [JsonProperty("timestamp")]
        public string Timestamp;
        [JsonProperty("trace_id")]
        public string TraceId;
        [JsonProperty("correlation_id")]
        public string CorrelationId;
        [JsonProperty("submit_url")]
        public string SubmitUrl;
        [JsonProperty("context")]
        public string Context;
    }

}
