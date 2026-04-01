using Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DatabaseRepository.DTO;

public class TasksDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [MaxLength(256)]
    [Column("title")]
    public string Title { get; set; }

    [Column("fk_project_id")]
    public int FkProjectId { get; set; }

    [Column("fk_status_id")]
    public int FkStatusId { get; set; }

    [Column("fk_priority_id")]
    public int FkPriorityId { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [ForeignKey(nameof(FkProjectId))] // Links the ID to this object
    public virtual ProjectsDTO Project { get; set; }

    public ProjectTask ToModel()
    {
        return new ProjectTask
        (
            id: this.Id,
            title: this.Title,
            fkProjectId: this.FkPriorityId,
            fkStatusId: this.FkStatusId,
            fkPriorityId: this.FkPriorityId,
            createdDate: this.CreatedDate
        );
    }

    public static TasksDTO FromModel(ProjectTask taskModel)
    {
        ArgumentNullException.ThrowIfNull(taskModel, nameof(taskModel));
        return new()
        {
            Id = taskModel.Id,
            FkPriorityId = taskModel.FkPriorityId,
            FkProjectId = taskModel.FkProjectId,
            FkStatusId = taskModel.FkStatusId,
            CreatedDate = taskModel.CreatedDate,
            Title = taskModel.Title
        };
    }
}