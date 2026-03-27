using Application.Features.Users.Commands.AssignRol;
using Domain.Ports.Repositories;
using FluentValidation.TestHelper;
using Moq;

namespace Test.Application.Features.Users.Commands.AssignRol;

public class AssignRolCommandValidatorTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IRolRepository> _rolRepositoryMock;
    private readonly Mock<IUserRolRepository> _userRolRepositoryMock;
    private readonly AssignRolCommandValidator _validator;

    public AssignRolCommandValidatorTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _rolRepositoryMock = new Mock<IRolRepository>();
        _userRolRepositoryMock = new Mock<IUserRolRepository>();

        _userRepositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        _rolRepositoryMock
            .Setup(r => r.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        _userRolRepositoryMock
            .Setup(r => r.IsAssignedAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _validator = new AssignRolCommandValidator(
            _userRepositoryMock.Object,
            _rolRepositoryMock.Object,
            _userRolRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidCommand_PassesValidation()
    {
        var command = new AssignRolCommand(1, 2);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    // UserId

    [Fact]
    public async Task Validate_WithNonExistentUserId_FailsWithNotFoundError()
    {
        _userRepositoryMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        var command = new AssignRolCommand(99, 2);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.UserId)
            .WithErrorMessage("User not found.");
    }

    [Fact]
    public async Task Validate_WithExistingUserId_PassesIdValidation()
    {
        var command = new AssignRolCommand(1, 2);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.UserId);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesUserExistsAsyncCall()
    {
        var command = new AssignRolCommand(1, 2);

        await _validator.TestValidateAsync(command);

        _userRepositoryMock.Verify(r => r.ExistsAsync(1), Times.Once);
    }

    // RolId — existence

    [Fact]
    public async Task Validate_WithNonExistentRolId_FailsWithNotFoundError()
    {
        _rolRepositoryMock.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        var command = new AssignRolCommand(1, 99);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.RolId)
            .WithErrorMessage("Rol not found.");
    }

    [Fact]
    public async Task Validate_WithExistingRolId_PassesRolValidation()
    {
        var command = new AssignRolCommand(1, 2);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.RolId);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesRolExistsAsyncCall()
    {
        var command = new AssignRolCommand(1, 2);

        await _validator.TestValidateAsync(command);

        _rolRepositoryMock.Verify(r => r.ExistsAsync(2), Times.Once);
    }

    // RolId — duplicate

    [Fact]
    public async Task Validate_WithAlreadyAssignedRol_FailsWithDuplicateError()
    {
        _userRolRepositoryMock.Setup(r => r.IsAssignedAsync(1, 2)).ReturnsAsync(true);

        var command = new AssignRolCommand(1, 2);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.RolId)
            .WithErrorMessage("This role is already assigned to the user.");
    }

    [Fact]
    public async Task Validate_WithUnassignedRol_PassesDuplicateCheck()
    {
        var command = new AssignRolCommand(1, 2);

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.RolId);
    }

    [Fact]
    public async Task Validate_WithValidCommand_VerifiesIsAssignedAsyncCall()
    {
        var command = new AssignRolCommand(1, 2);

        await _validator.TestValidateAsync(command);

        _userRolRepositoryMock.Verify(r => r.IsAssignedAsync(1, 2), Times.Once);
    }
}
