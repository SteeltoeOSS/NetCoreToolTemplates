using System.Collections;
using System.Collections.Generic;

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
                    new object[] { "3.0.2", "net5.0", },
                    new object[] { "3.0.2", "netcoreapp3.1", },
                    new object[] { "2.5.3", "netcoreapp3.1", },
                    new object[] { "2.5.3", "netcoreapp2.1", },
                }.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
