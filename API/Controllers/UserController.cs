using API.DTOs.User;
using Application.Features.Users.Commands.CreateUser;
using Application.Features.Users.Commands.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class UserController : BaseController
{
    public UserController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedResponse(nameof(Create), new { id = response.UserId }, response);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var response = await Mediator.Send(new UpdateUserCommand(id, request.UserName, request.Email, request.IsActive, request.Password));
        return OkResponse(response);
    }
}
