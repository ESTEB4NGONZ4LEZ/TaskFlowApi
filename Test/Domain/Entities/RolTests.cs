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

    [Fact]
    public void Update_WithValidData_UpdatesProperties()
    {
        var rol = Rol.Create("Admin", "Old description");

        rol.Update("SuperAdmin", "New description");

        rol.Name.Should().Be("SuperAdmin");
        rol.Description.Should().Be("New description");
    }

    [Fact]
    public void Update_WithNullDescription_SetsDescriptionToNull()
    {
        var rol = Rol.Create("Admin", "Old description");

        rol.Update("Admin", null);

        rol.Description.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Update_WithEmptyName_ThrowsValidationException(string name)
    {
        var rol = Rol.Create("Admin", null);

        var act = () => rol.Update(name, null);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name is required.");
    }

    [Fact]
    public void Update_WithNameExceedingMaxLength_ThrowsValidationException()
    {
        var rol = Rol.Create("Admin", null);

        var act = () => rol.Update(new string('A', 51), null);

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Name must not exceed 50 characters.");
    }

    [Fact]
    public void Update_WithDescriptionExceedingMaxLength_ThrowsValidationException()
    {
        var rol = Rol.Create("Admin", null);

        var act = () => rol.Update("Admin", new string('A', 151));

        act.Should().Throw<ValidationException>()
            .Which.Errors.Should().Contain("Description must not exceed 150 characters.");
    }

    [Fact]
    public void Update_DoesNotModifyCreatedAt()
    {
        var rol = Rol.Create("Admin", null);
        var createdAt = rol.CreatedAt;

        rol.Update("SuperAdmin", null);

        rol.CreatedAt.Should().Be(createdAt);
    }
}
