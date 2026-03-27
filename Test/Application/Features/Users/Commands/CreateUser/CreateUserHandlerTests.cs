using Application.DTOs.User;
using Application.Features.Users.Commands.CreateUser;
using Application.Services;
using Domain.Entities;
using Domain.Ports;
using Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Application.Features.Users.Commands.CreateUser;

public class CreateUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly CreateUserHandler _handler;

    public CreateUserHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _handler = new CreateUserHandler(_userRepositoryMock.Object, _unitOfWorkMock.Object, _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsUserResponse()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@", Email = "john@example.com" };
        var created = User.Reconstitute(1, "john_doe", "hashed", "john@example.com", true, DateTime.UtcNow, null);

        _passwordHasherMock.Setup(h => h.Hash("Secret1@")).Returns("hashed");
        _userRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((Func<User>)(() => created));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<UserResponse>();
        result.UserId.Should().Be(1);
        result.UserName.Should().Be("john_doe");
        result.Email.Should().Be("john@example.com");
        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithValidCommand_HashesPassword()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@", Email = "john@example.com" };
        var created = User.Reconstitute(1, "john_doe", "hashed", "john@example.com", true, DateTime.UtcNow, null);

        _passwordHasherMock.Setup(h => h.Hash("Secret1@")).Returns("hashed");
        _userRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((Func<User>)(() => created));

        await _handler.Handle(command, CancellationToken.None);

        _passwordHasherMock.Verify(h => h.Hash("Secret1@"), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCreateAsync()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@", Email = "john@example.com" };
        var created = User.Reconstitute(1, "john_doe", "hashed", "john@example.com", true, DateTime.UtcNow, null);

        _passwordHasherMock.Setup(h => h.Hash(It.IsAny<string>())).Returns("hashed");
        _userRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((Func<User>)(() => created));

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCommitAsync()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@", Email = "john@example.com" };
        var created = User.Reconstitute(1, "john_doe", "hashed", "john@example.com", true, DateTime.UtcNow, null);

        _passwordHasherMock.Setup(h => h.Hash(It.IsAny<string>())).Returns("hashed");
        _userRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((Func<User>)(() => created));

        await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CommitsAfterCreate()
    {
        var command = new CreateUserCommand { UserName = "john_doe", Password = "Secret1@", Email = "john@example.com" };
        var created = User.Reconstitute(1, "john_doe", "hashed", "john@example.com", true, DateTime.UtcNow, null);
        var callOrder = new List<string>();

        _passwordHasherMock.Setup(h => h.Hash(It.IsAny<string>())).Returns("hashed");
        _userRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<User>()))
            .ReturnsAsync((Func<User>)(() => created))
            .Callback(() => callOrder.Add("CreateAsync"));
        _unitOfWorkMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("CommitAsync"));

        await _handler.Handle(command, CancellationToken.None);

        callOrder.Should().ContainInOrder("CreateAsync", "CommitAsync");
    }
}
