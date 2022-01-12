using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Company.WebApplication.CS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RandomValueController : ControllerBase
    {
        private IConfiguration _config;

        public RandomValueController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public dynamic Index()
        {
            return new
            {
                intVal = _config["random:int"],
                longVal = _config["random:long"],
                int10 = _config["random:int(10)"],
                long10 = _config["random:long(100)"],
                int10_20 = _config["random:int(10,20)"],
                long100_200 = _config["random:long(100,200)"],
                uuid = _config["random:uuid"],
                stringVal = _config["random:string"]
            };
        }
    }
}
