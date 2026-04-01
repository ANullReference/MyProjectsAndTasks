namespace Domain;

public class ProjectTask
{
    public int Id { get; }
    public string Title { get; } = string.Empty;
    public int FkProjectId { get; }
    public int FkStatusId { get; }
    public int FkPriorityId { get; }
    public DateTime CreatedDate { get; }

    public ProjectTask(int id, string title, int fkProjectId, int fkStatusId, int fkPriorityId, DateTime createdDate)
    {
        if (Id <= 0)
        {
            throw new InvalidOperationException($"Id {Id} is not valid");
        }

        if (string.IsNullOrEmpty(Title) || string.IsNullOrWhiteSpace(Title))
        {
            throw new InvalidOperationException($"Title cannot be empty or blank spaces");
        }

        if (title.Length > 256)
        {
            throw new InvalidOperationException($"Title cannot be longer than 256 characters");
        }

        Id = id;
        Title = title;
        FkProjectId = fkProjectId;
        FkStatusId = fkStatusId;
        FkPriorityId = fkPriorityId;
        CreatedDate = createdDate;
    }
}