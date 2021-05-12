#if (PlaceholderConfiguration || RandomValueConfiguration)
using System.Collections.Generic;
#endif
using Microsoft.AspNetCore.Mvc;
#if (PlaceholderConfiguration || RandomValueConfiguration)
using Microsoft.Extensions.Configuration;
#endif

namespace Company.WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
#if (PlaceholderConfiguration || RandomValueConfiguration)
        private readonly IConfiguration _configuration;

        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

#endif
#if PlaceholderConfiguration
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var val1 = _configuration["ResolvedPlaceholderFromEnvVariables"];
            var val2 = _configuration["UnresolvedPlaceholder"];
            var val3 = _configuration["ResolvedPlaceholderFromJson"];

            return new[] { val1, val2, val3 };
        }
#elif RandomValueConfiguration
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var val1 = _configuration["random:int"];
            var val2 = _configuration["random:uuid"];
            var val3 = _configuration["random:string"];

            return new[] { val1, val2, val3 };
        }
#else
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "value";
        }
#endif

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
