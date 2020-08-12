using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp1.Server.Models
{
    public class FhirLaunchInfo
    {
        /*public FhirLaunchInfo()
        {
            this.retrivaltime = DateTime.Now;
        }*/

        /// <summary>
        /// Boolean to indicate if the patient banner is needed by the UI.
        /// </summary>
        public bool need_patient_banner { get; set; }
        /// <summary>
        /// The authentication token needed to access the patient data.
        /// </summary>
        public string id_token { get; set; }
        /// <summary>
        /// Where the smart style is. For smart dressers
        /// </summary>
        public string smart_style_url { get; set; }
        /// <summary>
        /// ID for the current encounter.
        /// </summary>
        public string encounter { get; set; }
        /// <summary>
        /// ID for the current patient.
        /// </summary>
        public string patient { get; set; }
        /// <summary>
        /// Permissions scope granted by the system for the current activity.
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// ID of the current user.
        /// </summary>
        public string user { get; set; }
        /// <summary>
        /// I think this is the unique ID of the system currently being accessed
        /// </summary>
        public string tenant { get; set; }
        /// <summary>
        /// Login name of the current user.
        /// </summary>
        public string username { get; set; }

        //}

        //public partial class CernerToken
        //{

        /// <summary>
        /// Type of token represented by this object. 'Bearer' or '' are the known options. 
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// The system generated unique identifier used to access the system.
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// Token used to get a new access_token after the one currently being used has expired.
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// Time, in seconds, until the token expires. Based on the retrivaltime
        /// </summary>
        public int expires_in { get; set; }


        //public partial class CernerAuth

        public DateTime retrivaltime { get; set; }

        public DateTime expiretime
        {
            get
            {
                return retrivaltime.AddSeconds(expires_in);
            }
        }


        public string CorrelationId { get; set; }

        public string ErrorDescription { get; set; }
    }
}
