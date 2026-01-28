using System.Collections.Generic;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Models;

public class ProjectItemGroup
{
    public List<PackageReference> PackageReferences { get; set; } = [];
}
