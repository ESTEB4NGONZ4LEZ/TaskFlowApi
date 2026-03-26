using Application.Features.Rol.Commands.CreateRol;
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
        return CreatedAtAction(nameof(Create), new { id = response.RolId }, response);
    }
}
