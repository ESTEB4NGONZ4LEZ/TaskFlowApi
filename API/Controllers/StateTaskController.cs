using API.DTOs.StateTask;
using Application.Features.StateTasks.Commands.CreateStateTask;
using Application.Features.StateTasks.Commands.UpdateStateTask;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class StateTaskController : BaseController
{
    public StateTaskController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStateTaskCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedResponse(nameof(Create), new { id = response.StateTaskId }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStateTaskRequest request)
    {
        var response = await Mediator.Send(new UpdateStateTaskCommand(id, request.Name, request.Description));
        return OkResponse(response);
    }
}
