using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class ProjectModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
}
