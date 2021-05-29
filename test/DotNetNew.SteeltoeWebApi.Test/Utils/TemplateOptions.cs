using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Steeltoe.DotNetNew.SteeltoeWebApi.Test.Utils
{
    public static class TemplateOptions
    {
        public class SteeltoeVersionsAndFrameworks : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                return new List<object[]>
                {
                    new object[] { "3.0.*", "net5.0", },
                    new object[] { "3.0.*", "netcoreapp3.1", },
                    new object[] { "2.5.*", "netcoreapp3.1", },
                    new object[] { "2.5.*", "netcoreapp2.1", },
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
                    "azure-spring-cloud",
                    "cloud-config",
                    "cloud-foundry",
                    "docker",
                    "dynamic-logger",
                    "eureka",
                    "hystrix",
                    "management-endpoints",
                    "mongodb",
                    "mysql",
                    "mysql-efcore",
                    "oauth",
                    "placeholder",
                    "postgresql",
                    "postgresql-efcore",
                    "rabbitmq",
                    "random-value",
                    "redis",
                    "sqlserver",
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
