using System.Text.Json.Serialization;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Models
{
    public class AppSettings
    {
        [JsonPropertyName("$schema")]
        public string Schema { get; set; }

        public string ResolvedPlaceholderFromEnvVariables { get; set; }

        public string UnresolvedPlaceholder { get; set; }

        public string ResolvedPlaceholderFromJson { get; set; }
    }
}
