using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp1.Server.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var vals = new ConnectionParameters();
            return new string[]
            {
                ConnectionParameters.LaunchUri, 
                ConnectionParameters.ServiceUri,
                ConnectionParameters.LaunchContextId,
                ConnectionParameters.AuthCodeRequest,
                ConnectionParameters.AuthCode,
                ConnectionParameters.AuthTokenRequestString,
                ConnectionParameters.AuthTokenResponseString,
                ConnectionParameters.TokenRequestSuccess.AccessToken,
                ConnectionParameters.PatientResourceString
            };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            ConnectionParameters.SeAuthResponseUrl(value);
            LaunchHelper.launchHelper.SendTokenRequest();
            LaunchHelper.launchHelper.GetPatient();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
