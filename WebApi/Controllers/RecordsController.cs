using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace GR.Records.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("records")]
    public class RecordsController : Controller
    {
        [HttpGet("gender")]
        public IEnumerable<string> GetSortedByGender()
        {
            return new string[] { "1", "2", "3" };
        }

        [HttpGet("birthdate")]
        public IEnumerable<string> GetSortedByBirthDate()
        {
            return new string[] { "2", "3", "1" };
        }

        [HttpGet("name")]
        public IEnumerable<string> GetSortedByName()
        {
            return new string[] { "3", "1", "2" };
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
    }
}
