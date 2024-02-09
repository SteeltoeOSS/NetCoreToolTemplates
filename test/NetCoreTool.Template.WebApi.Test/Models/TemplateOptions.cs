using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Models
{
    public static class TemplateOptions
    {
        public class BaseOptions : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                return new List<object[]>
                {
                    new object[] { "3.2.6", "net6.0", "C#", },
                    new object[] { "3.2.6", "net6.0", "F#", },
                    new object[] { "3.2.6", "net8.0", "C#", },
                    new object[] { "3.2.6", "net8.0", "F#", },
                }.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public class DependencyOptions : IEnumerable<object[]>
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
                    foreach (var steeltoeVersionsAndFramework in new BaseOptions())
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
