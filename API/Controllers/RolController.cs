using Application.Features.Rol.Commands.CreateRol;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRolCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = response.RolId }, response);
    }
}
