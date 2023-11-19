using Microsoft.AspNetCore.Mvc;
using TecIcaClient.WebApi.Logic;

namespace TecIcaClient.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class IcaTecClientController : ControllerBase
{
	private readonly IIcaClientActions _actions;

	public IcaTecClientController(IIcaClientActions actions) => _actions = actions;

	[HttpPost("Subscribe/{eventId:int}")]
	public async Task<ActionResult<string>> SubscribeToEvent(int eventId)
	{
		// Your subscribe logic here
		var operationStatus = await _actions.SubscribeToEvent(eventId);

		if (!operationStatus.Success)
			return BadRequest($"Failed to subscribe to event {eventId}: {operationStatus.Reason}");

		return Ok(operationStatus);
	}

	[HttpPost("Unsubscribe/{eventId:int}")]
	public async Task<ActionResult> UnsubscribeFromEvent(int eventId)
	{
		// Your unsubscribe logic here
		var operationStatus = await _actions.UnsubscribeFromEvent(eventId);

		if (!operationStatus.Success)
			return BadRequest($"Failed to unsubscribe from event {eventId}: {operationStatus.Reason}");

		return Ok(operationStatus);
	}

	[HttpPost("SendCommand/{commandId:int}")]
	public async Task<ActionResult> SendCommand(int commandId)
	{
		// Your send command logic here
		var operationStatus = await _actions.SendCommand(commandId);

		if (!operationStatus.Success)
			return BadRequest($"Failed to send command {commandId}: {operationStatus.Reason}");

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