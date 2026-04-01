namespace Domain;

public class Project
{
    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime CreatedDate { get; }

    public Project(int id, string name, string description, DateTime createdDate)
    {
        if (Id <= 0)
        {
            throw new InvalidOperationException($"Id {Id} is not valid");
        }

        if (string.IsNullOrEmpty(Name) || string.IsNullOrWhiteSpace(Name))
        {
            throw new InvalidOperationException($"Name cannot be empty or blank spaces");
        }

        Id = id;
        Name = name;
        Description = description;
        CreatedDate = createdDate;
    }
}