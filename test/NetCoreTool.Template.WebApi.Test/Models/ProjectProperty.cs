using System.Diagnostics;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Models;

[DebuggerDisplay("{Name,nq} = {Value}")]
public class ProjectProperty
{
    public string Name { get; set; }
    public string Value { get; set; }
}
