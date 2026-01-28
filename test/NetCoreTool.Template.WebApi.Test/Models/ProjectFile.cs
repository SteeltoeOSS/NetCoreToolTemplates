using System.Collections.Generic;

namespace Steeltoe.NetCoreTool.Template.WebApi.Test.Models;

public class ProjectFile
{
    public List<ProjectPropertyGroup> PropertyGroups { get; set; } = [];
    public List<ProjectItemGroup> ItemGroups { get; set; } = [];
}
