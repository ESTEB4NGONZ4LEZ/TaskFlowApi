using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;

namespace Test.Domain.Entities;

public class RolTests
{
    [Fact]
    public void Create_WithValidData_ReturnsRol()
    {
        var rol = Rol.Create("Admin", "Administrator role");

        rol.Name.Should().Be("Admin");
        rol.Description.Should().Be("Administrator role");
        rol.RolId.Should().Be(0);
        rol.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WithNullDescription_ReturnsRol()
    {
        var rol = Rol.Create("Admin", null);

        rol.Name.Should().Be("Admin");
        rol.Description.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_WithEmptyName_ThrowsValidationException(string name)
    {
        var act = () => Rol.Create(name, "Description");

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Create_WithNameExceedingMaxLength_ThrowsValidationException()
    {
        var name = new string('A', 51);

        var act = () => Rol.Create(name, "Description");

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name must not exceed 50 characters.");
    }

    [Fact]
    public void Create_WithDescriptionExceedingMaxLength_ThrowsValidationException()
    {
        var description = new string('A', 151);

        var act = () => Rol.Create("Admin", description);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Description must not exceed 150 characters.");
    }

    [Fact]
    public void Create_WithMultipleInvalidFields_ThrowsValidationExceptionWithAllErrors()
    {
        var name = new string('A', 51);
        var description = new string('A', 151);

        var act = () => Rol.Create(name, description);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().HaveCount(2);
    }

    [Fact]
    public void Create_WithNameAtMaxLength_ReturnsRol()
    {
        var name = new string('A', 50);

        var rol = Rol.Create(name, null);

        rol.Name.Should().Be(name);
    }
}
