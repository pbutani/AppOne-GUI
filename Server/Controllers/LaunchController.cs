using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LaunchController : ControllerBase
    {
        private readonly ILogger<LaunchController> _logger;
        //private string urlString;

        public LaunchController(ILogger<LaunchController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public void Post([FromBody] string value)
        {
            LaunchHelper.SetUrl(value);
            LaunchHelper.launchHelper.ProcessLaunch();
        }
    }
}
