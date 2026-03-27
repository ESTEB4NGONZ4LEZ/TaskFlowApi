using Application.DTOs.TaskPriority;
using Application.Features.TaskPriorities.Commands.UpdateTaskPriority;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Application.Features.TaskPriorities.Commands.UpdateTaskPriority;

public class UpdateTaskPriorityHandlerTests
{
    private readonly Mock<ITaskPriorityRepository> _taskPriorityRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateTaskPriorityHandler _handler;

    public UpdateTaskPriorityHandlerTests()
    {
        _taskPriorityRepositoryMock = new Mock<ITaskPriorityRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateTaskPriorityHandler(_taskPriorityRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsUpdatedTaskPriorityResponse()
    {
        var existing = TaskPriority.Reconstitute(1, "Critical", "Old description", 1);
        var command = new UpdateTaskPriorityCommand(1, "High", "New description", 2);

        _taskPriorityRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<TaskPriorityResponse>();
        result.TaskPriorityId.Should().Be(1);
        result.Name.Should().Be("High");
        result.Description.Should().Be("New description");
        result.Level.Should().Be(2);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsUpdateAsync()
    {
        var existing = TaskPriority.Reconstitute(1, "Critical", null, 1);
        var command = new UpdateTaskPriorityCommand(1, "High", null, 2);

        _taskPriorityRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        await _handler.Handle(command, CancellationToken.None);

        _taskPriorityRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TaskPriority>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCommitAsync()
    {
        var existing = TaskPriority.Reconstitute(1, "Critical", null, 1);
        var command = new UpdateTaskPriorityCommand(1, "High", null, 2);

        _taskPriorityRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTaskPriorityNotFound_ThrowsNotFoundException()
    {
        _taskPriorityRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TaskPriority?)null);

        var command = new UpdateTaskPriorityCommand(99, "High", null, 2);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WithValidCommand_CommitsAfterUpdate()
    {
        var existing = TaskPriority.Reconstitute(1, "Critical", null, 1);
        var command = new UpdateTaskPriorityCommand(1, "High", null, 2);
        var callOrder = new List<string>();

        _taskPriorityRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _taskPriorityRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<TaskPriority>()))
            .Callback(() => callOrder.Add("UpdateAsync"));
        _unitOfWorkMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("CommitAsync"));

        await _handler.Handle(command, CancellationToken.None);

        callOrder.Should().ContainInOrder("UpdateAsync", "CommitAsync");
    }
}
