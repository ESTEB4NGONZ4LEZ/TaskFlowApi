using API.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly IMediator Mediator;

    protected BaseController(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected IActionResult OkResponse<T>(T data, string message = "Request successful") =>
        Ok(ApiResponse<T>.Ok(data, message));

    protected IActionResult CreatedResponse<T>(string actionName, object routeValues, T data, string message = "Resource created successfully") =>
        CreatedAtAction(actionName, routeValues, ApiResponse<T>.Ok(data, message));

    protected IActionResult FailResponse(string message, IEnumerable<string>? errors = null) =>
        BadRequest(ApiResponse<object>.Fail(message, errors));
}
