using Domain.Entities;
using FluentAssertions;

namespace Test.Domain.Entities;

public class UserRolTests
{
    [Fact]
    public void Create_WithValidData_ReturnsUserRol()
    {
        var userRol = UserRol.Create(1, 2);

        userRol.UserId.Should().Be(1);
        userRol.RolId.Should().Be(2);
    }

    [Fact]
    public void Create_SetsAssignedAtToCurrentTime()
    {
        var before = DateTime.UtcNow;
        var userRol = UserRol.Create(1, 2);
        var after = DateTime.UtcNow;

        userRol.AssignedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Fact]
    public void Reconstitute_SetsAllProperties()
    {
        var assignedAt = new DateTime(2025, 1, 15, 10, 0, 0, DateTimeKind.Utc);

        var userRol = UserRol.Reconstitute(3, 5, assignedAt);

        userRol.UserId.Should().Be(3);
        userRol.RolId.Should().Be(5);
        userRol.AssignedAt.Should().Be(assignedAt);
    }
}
