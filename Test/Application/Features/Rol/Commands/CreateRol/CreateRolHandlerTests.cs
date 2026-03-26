using Application.DTOs.Rol;
using Application.Features.Rol.Commands.CreateRol;
using RolEntity = Domain.Entities.Rol;
using Domain.Ports;
using Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Application.Features.Rol.Commands.CreateRol;

public class CreateRolHandlerTests
{
    private readonly Mock<IRolRepository> _rolRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateRolHandler _handler;

    public CreateRolHandlerTests()
    {
        _rolRepositoryMock = new Mock<IRolRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateRolHandler(_rolRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsRolResponse()
    {
        var command = new CreateRolCommand { Name = "Admin", Description = "Administrator role" };
        var createdRol = RolEntity.Reconstitute(1, "Admin", "Administrator role", DateTime.UtcNow);

        _rolRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<RolEntity>()))
            .ReturnsAsync((Func<RolEntity>)(() => createdRol));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<RolResponse>();
        result.RolId.Should().Be(1);
        result.Name.Should().Be("Admin");
        result.Description.Should().Be("Administrator role");
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCreateAsync()
    {
        var command = new CreateRolCommand { Name = "Admin", Description = "Description" };
        var createdRol = RolEntity.Reconstitute(1, "Admin", "Description", DateTime.UtcNow);

        _rolRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<RolEntity>()))
            .ReturnsAsync((Func<RolEntity>)(() => createdRol));

        await _handler.Handle(command, CancellationToken.None);

        _rolRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<RolEntity>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCommitAsync()
    {
        var command = new CreateRolCommand { Name = "Admin", Description = "Description" };
        var createdRol = RolEntity.Reconstitute(1, "Admin", "Description", DateTime.UtcNow);

        _rolRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<RolEntity>()))
            .ReturnsAsync((Func<RolEntity>)(() => createdRol));

        await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CommitsAfterCreate()
    {
        var command = new CreateRolCommand { Name = "Admin", Description = "Description" };
        var createdRol = RolEntity.Reconstitute(1, "Admin", "Description", DateTime.UtcNow);
        var callOrder = new List<string>();

        _rolRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<RolEntity>()))
            .ReturnsAsync((Func<RolEntity>)(() => createdRol))
            .Callback(() => callOrder.Add("CreateAsync"));

        _unitOfWorkMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("CommitAsync"));

        await _handler.Handle(command, CancellationToken.None);

        callOrder.Should().ContainInOrder("CreateAsync", "CommitAsync");
    }
}
