using System.Collections.Generic;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Models
{
    public class ProjectOptions
    {
        public SteeltoeVersion SteeltoeVersion { get; set; }

        public Framework Framework { get; set; }

        public Language Language { get; set; }

        public List<string> Dependencies { get; set; } = new List<string>();
    }
}
