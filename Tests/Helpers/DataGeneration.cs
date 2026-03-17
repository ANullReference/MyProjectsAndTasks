using Infrastructure.DatabaseRepository;
using Infrastructure.DatabaseRepository.DTO;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Helpers;

public class DataGeneration
{
    public static Mock<DbSet<TasksDTO>> MockedDbSetTask(int entrySize, Mock<ApplicationDbContext> dBContext, ProjectsDTO projectDTO)
    {
        if (projectDTO.Id == null)
        {
            throw new ArgumentException("ProjectDTO must have a valid Id.");
        }

        List<TasksDTO> taskDTO = new List<TasksDTO>();

        for (int i = 0; i < entrySize; i++)
        {
            taskDTO.Add(new TasksDTO() { Id = i, Title = $"Title {i}", FkProjectId = projectDTO.Id ?? 0 });
        }

        var data = taskDTO.AsQueryable();

        //var a = data.GetEnumerator();

        Mock<DbSet<TasksDTO>> mockSet = new Mock<DbSet<TasksDTO>>();

        // Link the IQueryable properties to the List
        mockSet.As<IQueryable<TasksDTO>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<TasksDTO>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<TasksDTO>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<TasksDTO>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        dBContext.Setup(s => s.Tasks).Returns(mockSet.Object);

        return mockSet;
    }


    public static Mock<DbSet<ProjectsDTO>> MockedDbSetProject(int entrySize, Mock<ApplicationDbContext> dBContext)
    {
        List<ProjectsDTO> projectsDTOs = new List<ProjectsDTO>();

        for (int i = 0; i < entrySize; i++)
        {
            projectsDTOs.Add(new ProjectsDTO() { Id = i, Name = $"Test{i}" });
        }

        var data = projectsDTOs.AsQueryable();

        //var a = data.GetEnumerator();

        Mock<DbSet<ProjectsDTO>> mockSet = new Mock<DbSet<ProjectsDTO>>();

        // Link the IQueryable properties to the List
        mockSet.As<IQueryable<ProjectsDTO>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<ProjectsDTO>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<ProjectsDTO>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<ProjectsDTO>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

        dBContext.Setup(s => s.Projects).Returns(mockSet.Object);

        return mockSet;
    }
}
