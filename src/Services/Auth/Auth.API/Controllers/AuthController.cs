using Auth.Application.Features.Auth.Commands.Login;
using Auth.Application.Features.Auth.Commands.RefreshToken;
using Auth.Application.Features.Auth.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterCommandHandler _registerCommandHandler;
    private readonly LoginCommandHandler _loginCommandHandler;
    private readonly RefreshTokenCommandHandler _refreshTokenCommandHandler;

    public AuthController(
        RegisterCommandHandler registerCommandHandler,
        LoginCommandHandler loginCommandHandler,
        RefreshTokenCommandHandler refreshTokenCommandHandler)
    {
        _registerCommandHandler = registerCommandHandler;
        _loginCommandHandler = loginCommandHandler;
        _refreshTokenCommandHandler = refreshTokenCommandHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _registerCommandHandler.HandleAsync(command, cancellationToken);

        if (!result)
        {
            return BadRequest(new { Message = "User already exists." });
        }

        return Ok(new { Message = "User registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _loginCommandHandler.HandleAsync(command, cancellationToken);

        if (result is null)
        {
            return Unauthorized(new { Message = "Invalid email or password." });
        }

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await _refreshTokenCommandHandler.HandleAsync(command, cancellationToken);

        if (result is null)
        {
            return BadRequest(new { Message = "Invalid refresh token." });
        }

        return Ok(result);
    }
}