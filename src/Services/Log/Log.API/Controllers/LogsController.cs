using Log.Application.Features.Logs.Commands.CreateLog;
using Log.Application.Features.Logs.Queries.GetLogs;
using Microsoft.AspNetCore.Mvc;

namespace Log.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly CreateLogCommandHandler _createLogCommandHandler;
    private readonly GetLogsQueryHandler _getLogsQueryHandler;

    public LogsController(
        CreateLogCommandHandler createLogCommandHandler,
        GetLogsQueryHandler getLogsQueryHandler)
    {
        _createLogCommandHandler = createLogCommandHandler;
        _getLogsQueryHandler = getLogsQueryHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLogCommand command, CancellationToken cancellationToken)
    {
        var id = await _createLogCommandHandler.HandleAsync(command, cancellationToken);

        return Ok(new
        {
            Id = id,
            Message = "Log created successfully."
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var logs = await _getLogsQueryHandler.HandleAsync(cancellationToken);
        return Ok(logs);
    }
}