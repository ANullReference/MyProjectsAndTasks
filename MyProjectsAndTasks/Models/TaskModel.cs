namespace MyProjectsAndTasks.Models;

public class TaskModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int FkProjectId { get; set; }
    public int FkStatusId { get; set; }
    public int FkPriorityId { get; set; }
    public DateTime CreatedDate { get; set; }
}