using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get() {

            //Contract.Ensures(Contract.Result<ActionResult>() != null);
            //ServicePointManager.ServerCertificateValidationCallback =
            //delegate (object s, X509Certificate certificate,
            //X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            var claims = User.Claims;

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id) {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
