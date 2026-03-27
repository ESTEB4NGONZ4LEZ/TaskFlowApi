using Application.DTOs.User;
using Application.Features.Users.Commands.UpdateUser;
using Application.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly UpdateUserHandler _handler;

    public UpdateUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _handler = new UpdateUserHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsUpdatedUserResponse()
    {
        var existing = User.Reconstitute(1, "john_doe", "OldHash", "john@example.com", true, DateTime.UtcNow, null);
        var command = new UpdateUserCommand(1, "jane_doe", "jane@example.com", false, null);

        _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<UserResponse>();
        result.UserId.Should().Be(1);
        result.UserName.Should().Be("jane_doe");
        result.Email.Should().Be("jane@example.com");
        result.IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WithPasswordProvided_HashesPassword()
    {
        var existing = User.Reconstitute(1, "john_doe", "OldHash", "john@example.com", true, DateTime.UtcNow, null);
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, "NewSecret1@");

        _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _passwordHasherMock.Setup(h => h.Hash("NewSecret1@")).Returns("NewHash");

        await _handler.Handle(command, CancellationToken.None);

        _passwordHasherMock.Verify(h => h.Hash("NewSecret1@"), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNullPassword_DoesNotHashPassword()
    {
        var existing = User.Reconstitute(1, "john_doe", "OldHash", "john@example.com", true, DateTime.UtcNow, null);
        var command = new UpdateUserCommand(1, "john_doe", "john@example.com", true, null);

        _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        await _handler.Handle(command, CancellationToken.None);

        _passwordHasherMock.Verify(h => h.Hash(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsUpdateAsync()
    {
        var existing = User.Reconstitute(1, "john_doe", "OldHash", "john@example.com", true, DateTime.UtcNow, null);
        var command = new UpdateUserCommand(1, "jane_doe", "jane@example.com", true, null);

        _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCommitAsync()
    {
        var existing = User.Reconstitute(1, "john_doe", "OldHash", "john@example.com", true, DateTime.UtcNow, null);
        var command = new UpdateUserCommand(1, "jane_doe", "jane@example.com", true, null);

        _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);

        await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ThrowsNotFoundException()
    {
        _userRepositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((User?)null);

        var command = new UpdateUserCommand(99, "john_doe", "john@example.com", true, null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WithValidCommand_CommitsAfterUpdate()
    {
        var existing = User.Reconstitute(1, "john_doe", "OldHash", "john@example.com", true, DateTime.UtcNow, null);
        var command = new UpdateUserCommand(1, "jane_doe", "jane@example.com", true, null);
        var callOrder = new List<string>();

        _userRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
        _userRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<User>()))
            .Callback(() => callOrder.Add("UpdateAsync"));
        _unitOfWorkMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("CommitAsync"));

        await _handler.Handle(command, CancellationToken.None);

        callOrder.Should().ContainInOrder("UpdateAsync", "CommitAsync");
    }
}
