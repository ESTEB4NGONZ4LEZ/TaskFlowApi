using Application.DTOs.User;
using Application.Features.Users.Commands.AssignRol;
using Domain.Entities;
using Domain.Ports;
using Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Application.Features.Users.Commands.AssignRol;

public class AssignRolHandlerTests
{
    private readonly Mock<IUserRolRepository> _userRolRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AssignRolHandler _handler;

    public AssignRolHandlerTests()
    {
        _userRolRepositoryMock = new Mock<IUserRolRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new AssignRolHandler(_userRolRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsUserRolResponse()
    {
        var command = new AssignRolCommand(1, 2);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<UserRolResponse>();
        result.UserId.Should().Be(1);
        result.RolId.Should().Be(2);
        result.AssignedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsAssignAsync()
    {
        var command = new AssignRolCommand(1, 2);

        await _handler.Handle(command, CancellationToken.None);

        _userRolRepositoryMock.Verify(r => r.AssignAsync(It.IsAny<UserRol>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCommitAsync()
    {
        var command = new AssignRolCommand(1, 2);

        await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CommitsAfterAssign()
    {
        var command = new AssignRolCommand(1, 2);
        var callOrder = new List<string>();

        _userRolRepositoryMock
            .Setup(r => r.AssignAsync(It.IsAny<UserRol>()))
            .Callback(() => callOrder.Add("AssignAsync"));
        _unitOfWorkMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("CommitAsync"));

        await _handler.Handle(command, CancellationToken.None);

        callOrder.Should().ContainInOrder("AssignAsync", "CommitAsync");
    }
}
