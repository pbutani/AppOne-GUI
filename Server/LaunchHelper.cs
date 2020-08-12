using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using BlazorApp1.Server.Models;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlazorApp1.Server
{
    public class LaunchHelper
    {
        public static LaunchHelper launchHelper = null;
        private static string launchUrl;

        //private readonly IConnectionParameters connectionParameters = null;
        //public static string launchUri { set; get; }
        //private static readonly HttpClient fhirHttpClient;

        private LaunchHelper() //IConnectionParameters cp)
        {
            //fhirHttpClient = new HttpClient();
            //   connectionParameters = cp;
        }

        public static void SetUrl(string url)
        {
            if (launchHelper == null)
            {
                launchHelper = new LaunchHelper();
            }

            launchUrl = url;
        }

        public void ProcessLaunch()
        {
            //ConnectionParameters cp = new ConnectionParameters();
            ConnectionParameters.SeLaunchUrlString(launchUrl);
            GetMetadata();
            BuildAuthRequest();
        }

        private bool GetMetadata()
        {
            bool result = false;

            FormUrlEncodedContent tokenRequestForm = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("name", "SMART on FHIR Testing Server"),
                    new KeyValuePair<string, string>("description", "Dev server for SMART on FHIR"),
                    new KeyValuePair<string, string>("url", ConnectionParameters.ServiceUri)
                }
            );

            Dictionary<string, string> myDictionary = new Dictionary<string, string>();

            using (HttpClient httpClient = new HttpClient())
            {
                string requestString = tokenRequestForm.ReadAsStringAsync().Result;
                StringContent requestContent = new StringContent(requestString);
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                httpClient.DefaultRequestHeaders.Add("Authorization",
                    string.Format("Basic {0}", ConnectionParameters.LaunchContextId));

                string medatadataUrl = ConnectionParameters.BaseURL + "/" + ConnectionParameters.ServiceIdentifier +
                                       "/metadata";

                var response = httpClient.GetAsync(medatadataUrl + "?_format=json").Result;
                JObject jsonResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                //JsonSerializer jsonSerializer = new JsonSerializer();

                myDictionary = GetAuthenticationDictionaryFromJson(jsonResponse);
            }

            if (myDictionary.ContainsKey("token"))
            {
                ConnectionParameters.TokenUri = new Uri(myDictionary["token"]).ToString();
                result = true;
            }
            else
            {
                //Should be throwing or logging an error here.
                ConnectionParameters.TokenUri = "Sorry, couldn't find Token URI";
                result = false;
            }

            if (myDictionary.ContainsKey("authorize"))
            {
                ConnectionParameters.AuthUri = new Uri(myDictionary["authorize"]).ToString();
                result = true;
            }
            else
            {
                //Should be throwing or logging an error here.
                ConnectionParameters.AuthUri = "Sorry, couldn't find Authorize URI";
                result = false;
            }

            return result;
        }

        Dictionary<string, string> GetAuthenticationDictionaryFromJson(JObject rss)
        {
            JArray rssValue = (JArray) rss["rest"];

            //Get the json token that contains the list of urls we care about.
            JToken myJToken = rssValue[0]["security"]["extension"][0]["extension"];

            Dictionary<string, string> myDictionary = new Dictionary<string, string>();

            //Shove that list into a dictionary object.
            //Truth is, this should be done directly from the JToken but I can't figure that out.
            foreach (JObject content in myJToken.Children<JObject>())
            {
                myDictionary.Add(content["url"].ToString(), content["valueUri"].ToString());
            }

            return myDictionary;
        }

        private bool BuildAuthRequest()
        {
            bool bOk = true;
            ConnectionParameters.State = Guid.NewGuid();

            ConnectionParameters.AuthCodeRequest =
                ConnectionParameters.AuthUri + "?" +
                "response_type=code&" +
                "client_id=" + HttpUtility.UrlEncode(ConnectionParameters.ClientId) + "&" +
                "scope=" + HttpUtility.UrlEncode(ConnectionParameters.Scopes) + "&" +
                "redirect_uri=" + HttpUtility.UrlEncode(ConnectionParameters.RedirectUri) + "&" +
                "aud=" + HttpUtility.UrlEncode(ConnectionParameters.ServiceUri) + "&" +
                "launch=" + ConnectionParameters.LaunchContextId + "&" +
                "state=" + ConnectionParameters.State.ToString();

            Debug.WriteLine(
                $"In SendAuthRequest ConnectionParameters.AuthCodeRequest = {ConnectionParameters.AuthCodeRequest}");
            return bOk;
        }

        public void SendTokenRequest()
        {
            FormUrlEncodedContent tokenRequestForm = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string,string>("grant_type", "authorization_code"),
                    new KeyValuePair<string,string>("code", ConnectionParameters.AuthCode),
                    new KeyValuePair<string,string>("redirect_uri", ConnectionParameters.RedirectUri),
                    new KeyValuePair<string,string>("client_id", ConnectionParameters.ClientId)

                }
            );
            
            using (HttpClient httpClient = new HttpClient())
            {
                string requestString = tokenRequestForm.ReadAsStringAsync().Result;
                StringContent requestContent = new StringContent(requestString);
                requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ConnectionParameters.ClientId + ":" + ConnectionParameters.Secret);
                // Set up the HTTP POST request
                HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Post, ConnectionParameters.TokenUri);
                tokenRequest.Content = requestContent;
                tokenRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue(ConnectionParameters.AppName, ConnectionParameters.AppVersion));
                tokenRequest.Headers.Add("client-request-id", Guid.NewGuid().ToString());
                tokenRequest.Headers.Add("return-client-request-id", "true");
                tokenRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(plainTextBytes));

                ConnectionParameters.AuthTokenRequestString = tokenRequest.ToString();
                //Debug.WriteLine("In GetTokensFromAuthority");
                //Debug.WriteLine($"tokenRequest = {tokenRequest}");


                // Send the request and read the JSON body of the response
                HttpResponseMessage response = httpClient.SendAsync(tokenRequest).Result;
                JObject jsonResponse = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                //Debug.WriteLine($"jsonResponse = {jsonResponse}");
                ConnectionParameters.AuthTokenResponseString = response.ToString();

                JsonSerializer jsonSerializer = new JsonSerializer();

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        ConnectionParameters.TokenRequestSuccess = (TokenRequestSuccessResponse)jsonSerializer.Deserialize(
                            new JTokenReader(jsonResponse), typeof(TokenRequestSuccessResponse));

                        ConnectionParameters.FhirLaunchParams = (FhirLaunchInfo)jsonSerializer.Deserialize(
                            new JTokenReader(jsonResponse), typeof(FhirLaunchInfo));
                        ConnectionParameters.FhirLaunchParams.retrivaltime = DateTime.Now;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }

                    //cernerAuthenticationInformation = (CernerAuth)jsonSerializer.Deserialize(
                    //  new JTokenReader(jsonResponse), typeof(CernerAuth));

                    // Save the tokens
                    //SaveUserTokens(session, s);
                    //return null;
                }
                else
                {
                    try
                    {

                        ConnectionParameters.TokenRequestError = (TokenRequestErrorResponse) jsonSerializer.Deserialize(
                            new JTokenReader(jsonResponse), typeof(TokenRequestErrorResponse));

                        /*cernerAuthenticationInformation.CorrelationId = e.CorrelationId;
                        cernerAuthenticationInformation.ErrorDescription = e.Description;
    
                        // Throw the error description
                        throw new Exception(e.Description);*/
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
        }

        public void GetPatient()
        {
            var client = new FhirClient(ConnectionParameters.ServiceUri)
            {
                PreferredFormat = ResourceFormat.Json
            }; ;

            client.OnBeforeRequest += (object sender, BeforeRequestEventArgs e) =>
            {
                e.RawRequest.Headers.Add("Authorization", "Bearer " + ConnectionParameters.TokenRequestSuccess.AccessToken);
            };


            var identity = ResourceIdentity.Build("Patient", ConnectionParameters.FhirLaunchParams.patient);

            Hl7.Fhir.Model.Patient resPatient = client.Read<Hl7.Fhir.Model.Patient>(identity);
            var serializer = new FhirXmlSerializer();
            ConnectionParameters.PatientResourceString = serializer.SerializeToString(resPatient);
        } 

    }
}
