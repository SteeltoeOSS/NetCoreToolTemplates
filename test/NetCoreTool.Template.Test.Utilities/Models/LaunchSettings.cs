using System.Collections.Generic;

namespace Steeltoe.NetCoreTool.Template.Test.Utilities.Models
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
