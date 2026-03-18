using Core.Domain;
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
    public string? Title { get; set; }

    [Column("fk_project_id")]
    public int FkProjectId { get; set; }

    [Column("fk_status_id")]
    public int? FkStatusId { get; set; }

    [Column("fk_priority_id")]
    public int? FkPriorityId { get; set; }

    [Column("created_date")]
    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    [ForeignKey(nameof(FkProjectId))] // Links the ID to this object
    public virtual ProjectsDTO? Project { get; set; }

    public TaskModel ToModel()
    {
        return new TaskModel
        {
            Id = this.Id,
            FkPriorityId = this.FkPriorityId ?? 0,
            FkProjectId = this.FkProjectId,
            FkStatusId = this.FkStatusId ?? 0,
            CreatedDate = this.CreatedDate ?? DateTime.Now,
            Title = this.Title ?? string.Empty
        };
    }

    public static TasksDTO FromModel(TaskModel taskModel)
    {
        ArgumentNullException.ThrowIfNull(taskModel, nameof(taskModel));
        return new()
        {
            Id = taskModel.Id,
            FkPriorityId = taskModel.FkPriorityId,
            FkProjectId = taskModel.FkProjectId,
            FkStatusId = taskModel.FkStatusId,
            CreatedDate = taskModel.CreatedDate ?? DateTime.Now,
            Title = taskModel.Title
        };
    }
}