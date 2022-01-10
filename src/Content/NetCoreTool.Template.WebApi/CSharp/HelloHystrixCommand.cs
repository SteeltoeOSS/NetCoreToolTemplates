#if (CircuitBreakerHystrixOption)
using System.Threading.Tasks;
using Steeltoe.CircuitBreaker.Hystrix;

namespace Company.WebApplication.CS
{
    public sealed class HelloHystrixCommand : HystrixCommand<string>
    {
        private readonly string _name;

        public HelloHystrixCommand(string name) : base(HystrixCommandGroupKeyDefault.AsKey("MyCircuitBreakers"))
        {
            _name = name;
            IsFallbackUserDefined = true;
        }

        protected override async Task<string> RunAsync()
        {
            return await Task.FromResult("Hello " + _name);
        }

        protected override async Task<string> RunFallbackAsync()
        {
            return await Task.FromResult("Hello " + _name + " via fallback");
        }
    }
}
#endif
