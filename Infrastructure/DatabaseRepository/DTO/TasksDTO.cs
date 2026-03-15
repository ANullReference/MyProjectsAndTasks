using Core.Domain;
using System;
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
    public int? FkProjectId { get; set; }
    
    [Column("fk_status_id")]
    public int? FkStatusId { get; set; }
    
    [Column("fk_priority_id")]
    public int? FkPriorityId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public TaskModel ToModel()
    {
        return new TaskModel
        {
            Id = this.Id,
            FkPriorityId = this.FkPriorityId ?? 0,
            FkProjectId = this.FkProjectId ?? 0,    
            FkStatusId = this.FkStatusId ?? 0,
            CreatedDate = this.CreatedDate,
            Title = this.Title ?? string.Empty
        };
    }

    public static TasksDTO FromModel(TaskModel taskModel)
    {
        ArgumentNullException.ThrowIfNull(taskModel, nameof(taskModel));
        return new TasksDTO
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