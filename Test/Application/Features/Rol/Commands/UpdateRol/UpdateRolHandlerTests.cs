using Application.DTOs.Rol;
using Application.Features.Roles.Commands.UpdateRol;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Ports;
using Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace Test.Application.Features.Roles.Commands.UpdateRol;

public class UpdateRolHandlerTests
{
    private readonly Mock<IRolRepository> _rolRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateRolHandler _handler;

    public UpdateRolHandlerTests()
    {
        _rolRepositoryMock = new Mock<IRolRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new UpdateRolHandler(_rolRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ReturnsUpdatedRolResponse()
    {
        var existingRol = Rol.Reconstitute(1, "Admin", "Old description", DateTime.UtcNow);
        var command = new UpdateRolCommand(1, "SuperAdmin", "New description");

        _rolRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existingRol);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeOfType<RolResponse>();
        result.RolId.Should().Be(1);
        result.Name.Should().Be("SuperAdmin");
        result.Description.Should().Be("New description");
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsUpdateAsync()
    {
        var existingRol = Rol.Reconstitute(1, "Admin", null, DateTime.UtcNow);
        var command = new UpdateRolCommand(1, "SuperAdmin", null);

        _rolRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existingRol);

        await _handler.Handle(command, CancellationToken.None);

        _rolRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Rol>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_CallsCommitAsync()
    {
        var existingRol = Rol.Reconstitute(1, "Admin", null, DateTime.UtcNow);
        var command = new UpdateRolCommand(1, "SuperAdmin", null);

        _rolRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existingRol);

        await _handler.Handle(command, CancellationToken.None);

        _unitOfWorkMock.Verify(u => u.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRolNotFound_ThrowsNotFoundException()
    {
        _rolRepositoryMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Rol?)null);

        var command = new UpdateRolCommand(99, "SuperAdmin", null);

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_WithValidCommand_CommitsAfterUpdate()
    {
        var existingRol = Rol.Reconstitute(1, "Admin", null, DateTime.UtcNow);
        var command = new UpdateRolCommand(1, "SuperAdmin", null);
        var callOrder = new List<string>();

        _rolRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(existingRol);

        _rolRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<Rol>()))
            .Callback(() => callOrder.Add("UpdateAsync"));

        _unitOfWorkMock
            .Setup(u => u.CommitAsync(It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("CommitAsync"));

        await _handler.Handle(command, CancellationToken.None);

        callOrder.Should().ContainInOrder("UpdateAsync", "CommitAsync");
    }
}
