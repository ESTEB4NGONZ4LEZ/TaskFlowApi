using API.DTOs.TaskPriority;
using Application.Features.TaskPriorities.Commands.CreateTaskPriority;
using Application.Features.TaskPriorities.Commands.UpdateTaskPriority;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class TaskPriorityController : BaseController
{
    public TaskPriorityController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskPriorityCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedResponse(nameof(Create), new { id = response.TaskPriorityId }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskPriorityRequest request)
    {
        var response = await Mediator.Send(new UpdateTaskPriorityCommand(id, request.Name, request.Description, request.Level));
        return OkResponse(response);
    }
}
