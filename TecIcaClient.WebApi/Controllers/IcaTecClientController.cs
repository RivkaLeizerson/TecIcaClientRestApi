using Microsoft.AspNetCore.Mvc;
using TecIcaClient.WebApi.Logic;

namespace TecIcaClient.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IcaTecClientController : ControllerBase
{
    private readonly IIcaClientActions _actions;

    public IcaTecClientController(IIcaClientActions actions) => _actions = actions;

    [HttpPost("Subscribe/{eventName}")]
    public async Task<ActionResult<string>> SubscribeToEvent(string eventName)
    {
        // Your subscribe logic here
        var operationStatus = await _actions.SubscribeToEvent(eventName);

        if (!operationStatus.Success)
            return BadRequest($"Failed to subscribe to event {eventName}: {operationStatus.Reason}");

        return Ok(operationStatus);
    }

    [HttpPost("Unsubscribe/{eventName}")]
    public async Task<ActionResult> UnsubscribeFromEvent(string eventName)
    {
        // Your unsubscribe logic here
        var operationStatus = await _actions.UnsubscribeFromEvent(eventName);

        if (!operationStatus.Success)
            return BadRequest($"Failed to unsubscribe from event {eventName}: {operationStatus.Reason}");

        return Ok(operationStatus);
    }

    [HttpPost("SendCommand/{commandName}")]
    public async Task<ActionResult> SendCommand(string commandName)
    {
        // Your send command logic here
        var operationStatus = await _actions.SendCommand(commandName);

        if (!operationStatus.Success)
            return BadRequest($"Failed to send command {commandName}: {operationStatus.Reason}");

        return Ok(operationStatus);
    }

    [HttpPost("Connect")]
    public async Task<ActionResult> ConnectToIca()
    {
        var operationStatus = await _actions.ConnectToIca();
        if (!operationStatus.Success)
            return BadRequest($"Failed to connect to ICA: {operationStatus.Reason}");

        return Ok(operationStatus);
    }

    [HttpGet("GetApis")]
    public ActionResult GetSupportedApis()
    {
        var operationStatus = _actions.GetSupportedApis();
        if (!operationStatus.Success)
            return BadRequest($"Failed to retrieve supported APIs: {operationStatus.Reason}");

        return Ok(operationStatus);
    }
}