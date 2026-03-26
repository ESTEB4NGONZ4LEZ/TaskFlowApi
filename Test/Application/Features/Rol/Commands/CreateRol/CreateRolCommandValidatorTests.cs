using Application.Features.Rol.Commands.CreateRol;
using Domain.Ports.Repositories;
using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;

namespace Test.Application.Features.Rol.Commands.CreateRol;

public class CreateRolCommandValidatorTests
{
    private readonly Mock<IRolRepository> _rolRepositoryMock;
    private readonly CreateRolCommandValidator _validator;

    public CreateRolCommandValidatorTests()
    {
        _rolRepositoryMock = new Mock<IRolRepository>();
        _rolRepositoryMock
            .Setup(r => r.ExistsWithNameAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(false);

        _validator = new CreateRolCommandValidator(_rolRepositoryMock.Object);
    }

    [Fact]
    public async Task Validate_WithValidCommand_PassesValidation()
    {
        var command = new CreateRolCommand { Name = "Admin", Description = "Description" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithNullDescription_PassesValidation()
    {
        var command = new CreateRolCommand { Name = "Admin", Description = null };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Validate_WithEmptyName_FailsWithRequiredError(string name)
    {
        var command = new CreateRolCommand { Name = name, Description = "Description" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public async Task Validate_WithNameExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new CreateRolCommand { Name = new string('A', 51), Description = "Description" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name must not exceed 50 characters.");
    }

    [Fact]
    public async Task Validate_WithNameAtMaxLength_PassesValidation()
    {
        var command = new CreateRolCommand { Name = new string('A', 50), Description = null };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public async Task Validate_WithDescriptionExceedingMaxLength_FailsWithMaxLengthError()
    {
        var command = new CreateRolCommand { Name = "Admin", Description = new string('A', 151) };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage("Description must not exceed 150 characters.");
    }

    [Fact]
    public async Task Validate_WithDescriptionAtMaxLength_PassesValidation()
    {
        var command = new CreateRolCommand { Name = "Admin", Description = new string('A', 150) };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public async Task Validate_WithDuplicateName_FailsWithUniqueError()
    {
        _rolRepositoryMock
            .Setup(r => r.ExistsWithNameAsync("Admin", 0))
            .ReturnsAsync(true);

        var command = new CreateRolCommand { Name = "Admin", Description = "Description" };

        var result = await _validator.TestValidateAsync(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("A role with this name already exists.");
    }

    [Fact]
    public async Task Validate_WithDuplicateName_VerifiesRepositoryCall()
    {
        var command = new CreateRolCommand { Name = "Admin", Description = "Description" };

        await _validator.TestValidateAsync(command);

        _rolRepositoryMock.Verify(r => r.ExistsWithNameAsync("Admin", 0), Times.Once);
    }
}
