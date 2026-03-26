using Application.DTOs.Rol;
using Application.UseCases.Rol;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolController : ControllerBase
{
    private readonly CreateRolUseCase _createRolUseCase;

    public RolController(CreateRolUseCase createRolUseCase)
    {
        _createRolUseCase = createRolUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRolRequest request)
    {
        var response = await _createRolUseCase.ExecuteAsync(request);
        return CreatedAtAction(nameof(Create), new { id = response.RolId }, response);
    }
}
