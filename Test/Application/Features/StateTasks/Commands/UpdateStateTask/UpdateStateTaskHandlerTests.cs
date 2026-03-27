using Application.DTOs.StateTask;
using Application.Features.StateTasks.Commands.UpdateStateTask;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Application.Features.StateTasks.Commands.UpdateStateTask;

public class UpdateStateTaskHandlerTests
{
    private readonly Mock<IStateTaskRepository> _stateTaskRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateStateTaskHandler _handler;

    public UpdateStateTaskHandlerTests()
    {
        _stateTaskRepositoryMock = new Mock<IStateTaskRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateStateTaskHandler(_stateTaskRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsUpdatedStateTaskResponse()
    {
        var existing = StateTask.Reconstitute(1, "In Progress", "Old description");
        var command = new UpdateStateTaskCommand(1, "Done", "New description");

        _stateTaskRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existing);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<StateTaskResponse>();
        result.StateTaskId.Should().Be(1);
        result.Name.Should().Be("Done");
        result.Description.Should().Be("New description");
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsUpdateAsync()
    {
        var existing = StateTask.Reconstitute(1, "In Progress", null);
        var command = new UpdateStateTaskCommand(1, "Done", null);

        _stateTaskRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existing);

        await _handler.Handle(command, CancellationToken.None);

        _stateTaskRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<StateTask>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCommitAsync()
    {
        var existing = StateTask.Reconstitute(1, "In Progress", null);
        var command = new UpdateStateTaskCommand(1, "Done", null);

        _stateTaskRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existing);

        await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenStateTaskNotFound_ThrowsNotFoundException()
    {
        _stateTaskRepositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((StateTask?)null);

        var command = new UpdateStateTaskCommand(99, "Done", null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WithValidCommand_CommitsAfterUpdate()
    {
        var existing = StateTask.Reconstitute(1, "In Progress", null);
        var command = new UpdateStateTaskCommand(1, "Done", null);
        var callOrder = new List<string>();

        _stateTaskRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existing);

        _stateTaskRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<StateTask>()))
            .Callback(() => callOrder.Add("UpdateAsync"));

        _unitOfWorkMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("CommitAsync"));

        await _handler.Handle(command, CancellationToken.None);

        callOrder.Should().ContainInOrder("UpdateAsync", "CommitAsync");
    }
}
