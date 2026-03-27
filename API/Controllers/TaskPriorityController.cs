using API.DTOs.TaskPriority;
using Application.Features.TaskPriorities.Commands.CreateTaskPriority;
using Application.Features.TaskPriorities.Commands.UpdateTaskPriority;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Manages task priorities used to classify tasks by urgency.
/// </summary>
public class TaskPriorityController : BaseController
{
    public TaskPriorityController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Creates a new task priority.
    /// </summary>
    /// <param name="command">Priority data including name, optional description, and level (1 = highest).</param>
    /// <returns>The created task priority.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskPriorityCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedResponse(nameof(Create), new { id = response.TaskPriorityId }, response);
    }

    /// <summary>
    /// Updates an existing task priority.
    /// </summary>
    /// <param name="id">The ID of the task priority to update.</param>
    /// <param name="request">Updated priority data.</param>
    /// <returns>The updated task priority.</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskPriorityRequest request)
    {
        var response = await Mediator.Send(new UpdateTaskPriorityCommand(id, request.Name, request.Description, request.Level));
        return OkResponse(response);
    }
}
