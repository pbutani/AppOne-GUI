
namespace BlazorApp1.Server.Models
{
    public interface ILaunchParameters
    {
        void SetUrlString(string url);
        string GetUrlString();
    }

    public class LaunchParameters : ILaunchParameters
    {
        private string patientId { get; set; }
        private string launchUrl { get; set; }

        public LaunchParameters()
        {
            patientId = "";
            launchUrl = "";
        }

        public void SetUrlString(string url)
        {
            launchUrl = url;
        }

        public string GetUrlString()
        {
            return launchUrl;
        }
    }
}
