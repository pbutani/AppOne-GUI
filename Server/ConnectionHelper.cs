using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Newtonsoft.Json.Linq;

namespace BlazorApp1.Server
{
    public class ConnectionHelper
    {
        string ServiceRootUrl = "https://r3.smarthealthit.org";

        private FhirClient fhirClient = null;

        public ConnectionHelper()
        {
            fhirClient = new Hl7.Fhir.Rest.FhirClient(ServiceRootUrl) {Timeout = (120 * 1000)};
        }

        public /*async Task*/ void ConnectToEhr()//string webApiUrl, string accessToken, Action<JObject> processResult)
        {
            try
            {
                // find patients with last name = Bernhard
                Bundle returnedSearchBundle = fhirClient.Search<Patient>(new string[] { "family=Bernhard" });
                string resId = String.Empty;
                if (returnedSearchBundle != null)
                {
                    Debug.WriteLine($"Found: {returnedSearchBundle.Total.ToString()} Bernhard patients.");
                    foreach (var entry in returnedSearchBundle.Entry)
                    {
                        Debug.WriteLine($"{entry.Resource.TypeName}/{entry.Resource.Id}");
                        resId = entry.Resource.Id; // remember any of them
                    }
                }

                var locA = new Uri(ServiceRootUrl + "/Patient/" + resId);
                var patA = fhirClient.Read<Patient>(locA);
                if (patA != null)
                {
                    Debug.WriteLine($"{patA.TypeName}/{patA.Id}");
                }

                

                //Attempt to send the resource to the server endpoint                
                //UriBuilder uriBuilderx = new UriBuilder(ServiceRootUrl);
                //uriBuilderx.Path = "Patient/" + resId; //"Patient";//
                //Hl7.Fhir.Model.Resource returnedResource = fhirClient.InstanceOperation(uriBuilderx.Uri, "everything"); 
                /*if (returnedResource is Hl7.Fhir.Model.Bundle)
                {
                    Hl7.Fhir.Model.Bundle returnedBundle = returnedResource as Hl7.Fhir.Model.Bundle;
                    //Console.WriteLine("Received: " + returnedBundle.Total + " results, the resources are: ");          
                    //foreach (var entry in returnedBundle.Entry)
                    //{
                    //    Console.WriteLine(string.Format("{0}/{1}",entry.Resource.TypeName, entry.Resource.Id));
                    //}
                }
                else
                {
                    throw new Exception("Operation call must return a bundle resource");
                }*/

            }
            catch (Hl7.Fhir.Rest.FhirOperationException FhirOpExec)
            {
                   //Process any Fhir Errors returned as OperationOutcome resource
                   Debug.WriteLine("");
                   Debug.WriteLine("An error message: " + FhirOpExec.Message);
                   Debug.WriteLine("");
                   string xml = Hl7.Fhir.Serialization.FhirSerializer.SerializeResourceToXml(FhirOpExec.Outcome);
                   XDocument xDoc = XDocument.Parse(xml);
                   Debug.WriteLine(xDoc.ToString());
            }
            catch (Exception GeneralException)
            {
                   Debug.WriteLine("");
                   Debug.WriteLine("An error message: " + GeneralException.Message);
                   Debug.WriteLine("");
            }
        }
    }
}
