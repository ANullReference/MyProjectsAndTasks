using Core;
using Core.Abstractions;
using Core.Domain;
//using Moq;

namespace Tests;


public class ServiceManagerTests
{
    private readonly ServiceManager _systemUnderTest_ServiceManager;

    private readonly Mock<IProjectRepository> _projectRepository;
    private readonly Mock<ITaskRepository> _taskRepository;

    public ServiceManagerTests()
    {
        _projectRepository = Mock.Of<IProjectRepository>();
        _taskRepository = Mock.Of<ITaskRepository>();

        _systemUnderTest_ServiceManager = new ServiceManager(_projectRepository.Object, _taskRepository.Object);
    }

    private IEnumerable<TaskModel> CreateTaskModels(int size, int projectId)
    {
        if (size < 0)
        {
            throw new ArgumentException("Size must be non-negative", nameof(size));
        }

        for (int i = 0; i < size; i++)
        {
            yield return new TaskModel
            {
                Id = i + 1,
                FkProjectId = projectId,
                Title = $"Task {i + 1}"
            };
        }
    }

    [Test]
    [Arguments(10)]
    [Arguments(20)]
    public async Task Verify_TenMaxTasks_ForProjectId_ShouldReturnInvalid(int projectId)
    {
        _projectRepository.Get(projectId, Any())
            .Returns(new ProjectModel { Id = projectId });

        _taskRepository.DidIReachMaxTasksForAProjectId(projectId, Any())
            .Returns(true);

        TaskModel taskModel = new()
        {
            FkProjectId = projectId,
            Title = "New Task"
        };

        ResponseObject<TaskModel> taskModelResponse = await _systemUnderTest_ServiceManager.CreateTask(taskModel, CancellationToken.None);
        await Assert.That(taskModelResponse.IsInvalid).IsTrue();
    }

    [Test]
    [Arguments("", 10)]
    [Arguments(null, 15)]
    public async Task Verify_NullOrEmptyTitle_ShouldReturnInvalid(string? title, int projectId)
    {
        TaskModel taskModel = new()
        {
            FkProjectId = 10,
            Title = title
        };

        //setup the mock to return a project and indicate that the max tasks for the project has been reached
        _projectRepository.Get(projectId, Any())
            .Returns(new ProjectModel { Id = projectId });

        _taskRepository.DidIReachMaxTasksForAProjectId(projectId, Any())
            .Returns(false);

        _taskRepository.Create(Any(), Any())
            .Returns(taskModel);

        ResponseObject<TaskModel> taskModelResponse = await _systemUnderTest_ServiceManager.CreateTask(taskModel, CancellationToken.None);

        await Assert.That(taskModelResponse.IsInvalid).IsTrue();
    }

    [Test]
    [Arguments(10)]
    [Arguments(20)]
    public async Task Verify_LessThanTenTasks_ForProjectId_ShouldReturnSucccess(int projectId)
    {
        TaskModel taskModel = new()
        {
            FkProjectId = projectId,
            Title = "New Task"
        };

        //setup the mock to return a project and indicate that the max tasks for the project has been reached
        _projectRepository.Get(projectId, Any())
            .Returns(new ProjectModel { Id = projectId });

        _taskRepository.DidIReachMaxTasksForAProjectId(projectId, Any())
            .Returns(false);

        _taskRepository.Create(Any(), Any())
            .Returns(taskModel);

        ResponseObject<TaskModel> taskModelResponse = await _systemUnderTest_ServiceManager.CreateTask(taskModel, CancellationToken.None);

        await Assert.That(taskModelResponse.IsSuccess).IsTrue();
    }
}