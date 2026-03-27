using API.DTOs.Rol;
using Application.Features.Roles.Commands.CreateRol;
using Application.Features.Roles.Commands.UpdateRol;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Manages roles within the system.
/// </summary>
public class RolController : BaseController
{
    public RolController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Creates a new role.
    /// </summary>
    /// <param name="command">Role data including name and optional description.</param>
    /// <returns>The created role.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRolCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedResponse(nameof(Create), new { id = response.RolId }, response);
    }

    /// <summary>
    /// Updates an existing role.
    /// </summary>
    /// <param name="id">The ID of the role to update.</param>
    /// <param name="request">Updated role data.</param>
    /// <returns>The updated role.</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRolRequest request)
    {
        var response = await Mediator.Send(new UpdateRolCommand(id, request.Name, request.Description));
        return OkResponse(response);
    }
}
