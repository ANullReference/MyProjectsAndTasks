using Core;
using Core.Abstractions;
using Domain;

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

    [Test]
    [Arguments(10)]
    [Arguments(20)]
    public async System.Threading.Tasks.Task Verify_TenMaxTasks_ForProjectId_ShouldReturnInvalid(int projectId)
    {
        _projectRepository.Get(projectId, Any())
            .Returns(new Project(projectId, "name", "description", DateTime.Now));

        _taskRepository.DidIReachMaxTasksForAProjectId(projectId, Any())
            .Returns(true);

        ProjectTask taskModel = new ProjectTask(0, "New Task", projectId, 0, 0, DateTime.Now);

        ResponseObject<ProjectTask> taskModelResponse = await _systemUnderTest_ServiceManager.CreateTask(taskModel, CancellationToken.None);
        await Assert.That(taskModelResponse.IsInvalid).IsTrue();
    }

    [Test]
    [Arguments("", 10)]
    [Arguments(null, 15)]
    public async System.Threading.Tasks.Task Verify_NullOrEmptyTitle_ShouldReturnInvalid(string title, int projectId)
    {
        ProjectTask taskModel = new ProjectTask(0, "New Task", 10, 0, 0, DateTime.Now);

        //setup the mock to return a project and indicate that the max tasks for the project has been reached
        _projectRepository.Get(projectId, Any())
            .Returns(new Project(projectId, "name", "description", DateTime.Now));


        _taskRepository.DidIReachMaxTasksForAProjectId(projectId, Any())
            .Returns(false);

        _taskRepository.Create(Any(), Any())
            .Returns(taskModel);

        ResponseObject<ProjectTask> taskModelResponse = await _systemUnderTest_ServiceManager.CreateTask(taskModel, CancellationToken.None);

        await Assert.That(taskModelResponse.IsInvalid).IsTrue();
    }

    [Test]
    [Arguments(10)]
    [Arguments(20)]
    public async System.Threading.Tasks.Task Verify_LessThanTenTasks_ForProjectId_ShouldReturnSucccess(int projectId)
    {
        ProjectTask taskModel = new ProjectTask(0, "New Task", projectId, 0, 0, DateTime.Now);

        //setup the mock to return a project and indicate that the max tasks for the project has been reached
        _projectRepository.Get(projectId, Any())
            .Returns(new Project(projectId, "name", "description", DateTime.Now));

        _taskRepository.DidIReachMaxTasksForAProjectId(projectId, Any())
            .Returns(false);

        _taskRepository.Create(Any(), Any())
            .Returns(taskModel);

        ResponseObject<ProjectTask> taskModelResponse = await _systemUnderTest_ServiceManager.CreateTask(taskModel, CancellationToken.None);

        await Assert.That(taskModelResponse.IsSuccess).IsTrue();
    }
}