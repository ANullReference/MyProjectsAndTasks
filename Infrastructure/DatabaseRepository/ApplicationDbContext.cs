using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseRepository;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    public virtual DbSet<DTO.TasksDTO> Tasks { get; set; }

    public virtual DbSet<DTO.ProjectsDTO> Projects { get; set; }
}
