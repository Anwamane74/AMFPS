using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Features.UserSecurity;

namespace Web.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserSecurityController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserSecurityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("RegisterUser", Name = "RegisterUser")]
    public async Task<ActionResult<UserRegistrationUseCase.RegistrationResponseDto>> RegisterUser(
        [FromBody] UserRegistrationUseCase.UserForRegistrationCommand userForRegistration)
    {
        var response = await _mediator.Send(userForRegistration);
        if (response.IsSuccessfulRegistration)
        {
            return Ok(201);
        }
        return BadRequest(response);
    }
}