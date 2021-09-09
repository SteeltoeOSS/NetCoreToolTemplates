using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Utils
{
    public static class TemplateOptions
    {
        public class SteeltoeVersionsAndFrameworks : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                return new List<object[]>
                {
                    new object[] { "3.1.*", "net5.0", },
                    new object[] { "3.1.*", "netcoreapp3.1", },
                    new object[] { "3.0.*", "net5.0", },
                    new object[] { "3.0.*", "netcoreapp3.1", },
                    new object[] { "2.5.*", "netcoreapp3.1", },
                }.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class SteeltoeVersionsAndFrameworksAndOptions : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                var matrix = new List<object[]>();
                var options = new[]
                {
                    "circuit-breaker-hystrix",
                    "configuration-cloud-config",
                    "configuration-placeholder",
                    "configuration-random-value",
                    "connector-mongodb",
                    "connector-mysql",
                    "connector-mysql-efcore",
                    "connector-oauth",
                    "connector-postgresql",
                    "connector-postgresql-efcore",
                    "connector-rabbitmq",
                    "connector-redis",
                    "connector-sqlserver",
                    "discovery-eureka",
                    "distributed-tracing",
                    "dockerfile",
                    "hosting-azure-spring-cloud",
                    "hosting-cloud",
                    "hosting-cloud-foundry",
                    "logging-dynamic-logger",
                    "management-endpoints",
                    "messaging-rabbitmq",
                };
                foreach (var option in options)
                {
                    foreach (var steeltoeVersionsAndFramework in new SteeltoeVersionsAndFrameworks())
                    {
                        matrix.Add(steeltoeVersionsAndFramework.Append(option).ToArray());
                    }
                }

                return matrix.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
