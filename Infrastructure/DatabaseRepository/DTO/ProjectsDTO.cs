using Core.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DatabaseRepository.DTO;

public class ProjectsDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int? Id { get; set; }

    [MaxLength(64)]
    [Column("name")]
    public string? Name { get; set; }

    [MaxLength(256)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("created_date")]
    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public ICollection<TasksDTO>? Tasks { get; set; }

    public ProjectModel ToModel()
    {
        return new ProjectModel
        {
            Id = this.Id,
            Name = this.Name,
            Description = this.Description,
            CreatedDate = this.CreatedDate ?? DateTime.Now
        };
    }

    public static ProjectsDTO ToEmpty => new ProjectsDTO { Id = 0 };
    public static ProjectsDTO FromModel(ProjectModel projectModel)
    {
        ArgumentNullException.ThrowIfNull(projectModel, nameof(projectModel));
        return new ProjectsDTO
        {
            Id = projectModel.Id,
            Name = projectModel.Name,
            Description = projectModel.Description,
            CreatedDate = projectModel.CreatedDate ?? DateTime.Now
        };
    }
}