using API.DTOs.StateTask;
using Application.Features.StateTasks.Commands.CreateStateTask;
using Application.Features.StateTasks.Commands.UpdateStateTask;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Manages task states used in the task workflow.
/// </summary>
public class StateTaskController : BaseController
{
    public StateTaskController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Creates a new task state.
    /// </summary>
    /// <param name="command">Task state data including name and optional description.</param>
    /// <returns>The created task state.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStateTaskCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedResponse(nameof(Create), new { id = response.StateTaskId }, response);
    }

    /// <summary>
    /// Updates an existing task state.
    /// </summary>
    /// <param name="id">The ID of the task state to update.</param>
    /// <param name="request">Updated task state data.</param>
    /// <returns>The updated task state.</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStateTaskRequest request)
    {
        var response = await Mediator.Send(new UpdateStateTaskCommand(id, request.Name, request.Description));
        return OkResponse(response);
    }
}
