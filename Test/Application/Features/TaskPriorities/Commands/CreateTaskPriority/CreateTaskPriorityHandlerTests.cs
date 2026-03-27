using Application.DTOs.TaskPriority;
using Application.Features.TaskPriorities.Commands.CreateTaskPriority;
using Domain.Entities;
using Domain.Ports;
using Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Application.Features.TaskPriorities.Commands.CreateTaskPriority;

public class CreateTaskPriorityHandlerTests
{
    private readonly Mock<ITaskPriorityRepository> _taskPriorityRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateTaskPriorityHandler _handler;

    public CreateTaskPriorityHandlerTests()
    {
        _taskPriorityRepositoryMock = new Mock<ITaskPriorityRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateTaskPriorityHandler(_taskPriorityRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsTaskPriorityResponse()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = "Highest priority", Level = 1 };
        var created = TaskPriority.Reconstitute(1, "Critical", "Highest priority", 1);

        _taskPriorityRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<TaskPriority>()))
            .ReturnsAsync((Func<TaskPriority>)(() => created));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<TaskPriorityResponse>();
        result.TaskPriorityId.Should().Be(1);
        result.Name.Should().Be("Critical");
        result.Description.Should().Be("Highest priority");
        result.Level.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WithNullDescription_ReturnsResponseWithNullDescription()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };
        var created = TaskPriority.Reconstitute(1, "Critical", null, 1);

        _taskPriorityRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<TaskPriority>()))
            .ReturnsAsync((Func<TaskPriority>)(() => created));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Description.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCreateAsync()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };
        var created = TaskPriority.Reconstitute(1, "Critical", null, 1);

        _taskPriorityRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<TaskPriority>()))
            .ReturnsAsync((Func<TaskPriority>)(() => created));

        await _handler.Handle(command, CancellationToken.None);

        _taskPriorityRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<TaskPriority>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCommitAsync()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };
        var created = TaskPriority.Reconstitute(1, "Critical", null, 1);

        _taskPriorityRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<TaskPriority>()))
            .ReturnsAsync((Func<TaskPriority>)(() => created));

        await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CommitsAfterCreate()
    {
        var command = new CreateTaskPriorityCommand { Name = "Critical", Description = null, Level = 1 };
        var created = TaskPriority.Reconstitute(1, "Critical", null, 1);
        var callOrder = new List<string>();

        _taskPriorityRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<TaskPriority>()))
            .ReturnsAsync((Func<TaskPriority>)(() => created))
            .Callback(() => callOrder.Add("CreateAsync"));
        _unitOfWorkMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("CommitAsync"));

        await _handler.Handle(command, CancellationToken.None);

        callOrder.Should().ContainInOrder("CreateAsync", "CommitAsync");
    }
}
