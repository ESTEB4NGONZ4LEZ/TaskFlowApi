using Application.DTOs.StateTask;
using Application.Features.StateTasks.Commands.CreateStateTask;
using Domain.Entities;
using Domain.Ports;
using Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Application.Features.StateTasks.Commands.CreateStateTask;

public class CreateStateTaskHandlerTests
{
    private readonly Mock<IStateTaskRepository> _stateTaskRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateStateTaskHandler _handler;

    public CreateStateTaskHandlerTests()
    {
        _stateTaskRepositoryMock = new Mock<IStateTaskRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateStateTaskHandler(_stateTaskRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsStateTaskResponse()
    {
        var command = new CreateStateTaskCommand { Name = "In Progress", Description = "Being worked on" };
        var created = StateTask.Reconstitute(1, "In Progress", "Being worked on");

        _stateTaskRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<StateTask>()))
            .ReturnsAsync((Func<StateTask>)(() => created));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<StateTaskResponse>();
        result.StateTaskId.Should().Be(1);
        result.Name.Should().Be("In Progress");
        result.Description.Should().Be("Being worked on");
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCreateAsync()
    {
        var command = new CreateStateTaskCommand { Name = "In Progress", Description = null };
        var created = StateTask.Reconstitute(1, "In Progress", null);

        _stateTaskRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<StateTask>()))
            .ReturnsAsync((Func<StateTask>)(() => created));

        await _handler.Handle(command, CancellationToken.None);

        _stateTaskRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<StateTask>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCommitAsync()
    {
        var command = new CreateStateTaskCommand { Name = "In Progress", Description = null };
        var created = StateTask.Reconstitute(1, "In Progress", null);

        _stateTaskRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<StateTask>()))
            .ReturnsAsync((Func<StateTask>)(() => created));

        await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CommitsAfterCreate()
    {
        var command = new CreateStateTaskCommand { Name = "In Progress", Description = null };
        var created = StateTask.Reconstitute(1, "In Progress", null);
        var callOrder = new List<string>();

        _stateTaskRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<StateTask>()))
            .ReturnsAsync((Func<StateTask>)(() => created))
            .Callback(() => callOrder.Add("CreateAsync"));

        _unitOfWorkMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("CommitAsync"));

        await _handler.Handle(command, CancellationToken.None);

        callOrder.Should().ContainInOrder("CreateAsync", "CommitAsync");
    }
}
