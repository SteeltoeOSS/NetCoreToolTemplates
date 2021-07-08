namespace Steeltoe.NetCoreTool.Template.Test.Utilities.Models
{
    public class AppSettings
    {
        public SpringSettings Spring { get; set; }

        public string ResolvedPlaceholderFromEnvVariables { get; set; }

        public string UnresolvedPlaceholder { get; set; }

        public string ResolvedPlaceholderFromJson { get; set; }
    }

    public class SpringSettings
    {
        public CloudSettings Cloud { get; set; }
    }

    public class CloudSettings
    {
        public StreamSettings Stream { get; set; }
    }

    public class StreamSettings
    {
        public string Binder { get; set; }
    }
}
