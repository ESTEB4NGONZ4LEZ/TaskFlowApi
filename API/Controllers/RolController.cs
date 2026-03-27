using API.DTOs.Rol;
using Application.Features.Roles.Commands.CreateRol;
using Application.Features.Roles.Commands.UpdateRol;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RolController : BaseController
{
    public RolController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRolCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedResponse(nameof(Create), new { id = response.RolId }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRolRequest request)
    {
        var response = await Mediator.Send(new UpdateRolCommand(id, request.Name, request.Description));
        return OkResponse(response);
    }
}
