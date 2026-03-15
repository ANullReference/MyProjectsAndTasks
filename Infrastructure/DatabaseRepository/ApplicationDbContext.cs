using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DatabaseRepository;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    public DbSet<DTO.TasksDTO> Tasks { get; set; }

    public DbSet<DTO.ProjectsDTO> Projects { get; set; }
}
