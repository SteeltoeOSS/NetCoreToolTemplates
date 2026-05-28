using System.Collections;
using System.Collections.Generic;

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
                    new object[] { "3.4.*", "net8.0", "C#" },
                    new object[] { "4.2.*", "net8.0", "C#" },
                    new object[] { "4.2.*", "net9.0", "C#" },
                    new object[] { "4.2.*", "net10.0", "C#" },
                    new object[] { "4.*-main-*", "net8.0", "C#" },
                    new object[] { "4.*-main-*", "net9.0", "C#" },
                    new object[] { "4.*-main-*", "net10.0", "C#" }
                }.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
