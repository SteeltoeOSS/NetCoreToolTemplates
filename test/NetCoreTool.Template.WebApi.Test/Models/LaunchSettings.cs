using System.Collections.Generic;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Models
{
    public class LaunchSettings
    {
        public Dictionary<string, Profile> Profiles { get; set; }

        public class Profile
        {
            public string LaunchUrl { get; set; }
        }
    }
}
