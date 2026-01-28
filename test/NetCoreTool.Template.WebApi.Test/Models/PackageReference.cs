using System.Diagnostics;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Models;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class PackageReference
{
    public string Include { get; set; }
    public string Condition { get; set; }
    public string Version { get; set; }

    private string DebuggerDisplay => Condition == null
        ? $"Include=\"{Include}\" Version=\"{Version}\""
        : $"Include=\"{Include}\" Condition=\"{Condition}\" Version=\"{Version}\"";
}
