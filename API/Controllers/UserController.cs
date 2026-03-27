using API.DTOs.User;
using Application.Features.Users.Commands.AssignRol;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Manages users and their role assignments.
/// </summary>
public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="command">User data including username, password, and email.</param>
    /// <returns>The created user.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedResponse(nameof(Create), new { id = response.UserId }, response);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="request">Updated user data. Password is optional — omit to keep the current one.</param>
    /// <returns>The updated user.</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var response = await Mediator.Send(new UpdateUserCommand(id, request.UserName, request.Email, request.IsActive, request.Password));
        return OkResponse(response);
    }

    /// <summary>
    /// Assigns a role to a user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <param name="request">The role to assign.</param>
    /// <returns>The role assignment details including the assigned date.</returns>
    [HttpPut("{id:int}/roles")]
    public async Task<IActionResult> AssignRol(int id, [FromBody] AssignRolRequest request)
    {
        var response = await Mediator.Send(new AssignRolCommand(id, request.RolId));
        return OkResponse(response);
    }
}
