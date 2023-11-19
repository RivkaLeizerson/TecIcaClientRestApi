#region INTEL_COPYRIGHT

/*
 * INTEL CONFIDENTIAL
 *
 * Copyright (C) 2023 Intel Corporation
 *
 * This software and the related documents are Intel copyrighted materials, and
 * your use of them is governed by the express license under which they were
 * provided to you (License). Unless the License provides otherwise, you may not
 * use, modify, copy, publish, distribute, disclose or transmit this software or
 * the related documents without Intel's prior written permission.
 *
 * This software and the related documents are provided as is, with no express or
 * implied warranties, other than those that are expressly stated in the License.
 *
 */

#endregion

using System.Text.Encodings.Web;
using System.Text.Json;
using Intel.Telemetry.Api.Client;
using Intel.Telemetry.Api.Commands.Data;
using Intel.Telemetry.Api.Context;
using Intel.Telemetry.Api.Events.EventsRegistrar;
using Intel.Telemetry.Api.Message.Version;

namespace TecIcaClient.WebApi.Logic;

public sealed class IcaClientActions : IIcaClientActions
{
	private const string THE_CLIENT_IS_NOT_CONNECTED_TO_ICA = "The client is not connected to ICA!";

	private static readonly Dictionary<int, string> TecEventsDictionary = new()
	{
		{ 9100, "USB Connect To Device" },
		{ 9101, "USB Disconnect From Device" },
		{ 9102, "USB Connect To Host" },
		{ 9103, "USB Disconnect From Host" },
		{ 9104, "TBT Connect To Host" },
		{ 9105, "TBT Disconnect From Host" },
		{ 9106, "TBT Connect To Device" },
		{ 9107, "TBT Disconnect From Device" },
		{ 9108, "Node Error PCI" },
		{ 9109, "Node Error USB" },
		{ 9110, "No DCH Driver" },
		{ 9111, "No Driver" },
		{ 9112, "TEC Presence" }
	};

	private static readonly Dictionary<int, string> TecCommandsDictionary = new()
	{
		{ 1, "Thunderbolt Rundown" }
	};

	private static readonly JsonSerializerOptions JsonSerializerOptions = new()
	{
		WriteIndented = true,
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
	};

	private ITelemetryClientContext? _context;

	public async Task<OperationStatus> SubscribeToEvent(int eventId)
	{
		if (!TecEventsDictionary.TryGetValue(eventId, out var eventName))
			return new OperationStatus(false, $"There is no support for event {eventId}");

		try
		{
			if (_context == null) return new OperationStatus(false, THE_CLIENT_IS_NOT_CONNECTED_TO_ICA);

			var eventDirectory = GetEventDirectory(eventId);

			var responseHeader = await _context.TelemetryEventsRegistrar.SubscribeToTelemetryEventAsync(
				new EventSubscriptionInfo(eventName!),
				json =>
				{
					File.WriteAllText(Path.Combine(eventDirectory, $"{eventId}_{DateTime.Now:dd_MM_yy_HH_mm_ss}.json"),
						json.ToJsonString(JsonSerializerOptions));
				});

			return responseHeader.HasFailed
				? new OperationStatus(false, responseHeader.FailureReason, responseHeader)
				: new OperationStatus(true, eventDirectory, responseHeader);
		}
		catch (Exception e)
		{
			return new OperationStatus(false, e.Message);
		}
	}

	public async Task<OperationStatus> UnsubscribeFromEvent(int eventId)
	{
		if (!TecEventsDictionary.TryGetValue(eventId, out var eventName))
			return new OperationStatus(false, $"There is no support for event {eventId}");

		try
		{
			if (_context == null) return new OperationStatus(false, THE_CLIENT_IS_NOT_CONNECTED_TO_ICA);

			var response =
				await _context.TelemetryEventsRegistrar.UnsubscribeFromTelemetryEventAsync(
					new EventSubscriptionInfo(eventName!));

			return response.HasFailed
				? new OperationStatus(false, response.FailureReason, response)
				: new OperationStatus(true, $"Successfully unsubscribed from event {eventId} - {eventName}", response);
		}
		catch (Exception e)
		{
			return new OperationStatus(false, e.Message);
		}
	}

	public async Task<OperationStatus> SendCommand(int commandId)
	{
		try
		{
			if (TecCommandsDictionary.TryGetValue(commandId, out var commandName))
				return new OperationStatus(false, $"There is no support for command {commandId}");

			if (_context == null) return new OperationStatus(false, THE_CLIENT_IS_NOT_CONNECTED_TO_ICA);
			var response = await _context.TelemetryCommandSender.SendTelemetryCommandAsync(new JsonCommand(commandName!,
				new MessageVersion(), 1, null));

			return response.Header.HasFailed
				? new OperationStatus(false, response.Header.FailureReason, response)
				: new OperationStatus(true, $"Successfully sent command {commandName} to ICA", response);
		}
		catch (Exception e)
		{
			return new OperationStatus(false, e.Message);
		}
	}

	public async Task<OperationStatus> ConnectToIca()
	{
		if (_context != null) return new OperationStatus(true, "Already connected");

		try
		{
			var telemetryClient = TelemetryClient.CreateNew(new FileInfo(@"Config\tec_client_signed.json"));

			Console.WriteLine("Connecting to ICA...");

			var initializeClientContextAsync = await
				telemetryClient.InitializeClientContextAsync(TimeSpan.FromSeconds(10), entries =>
				{
					Console.WriteLine("ICA API status:");
					foreach (var schemaDataEntry in entries) Console.WriteLine(schemaDataEntry);
				});

			Console.WriteLine("Connected to ICA");

			_context = initializeClientContextAsync;
		}
		catch (Exception e)
		{
			return new OperationStatus(false, e.Message);
		}

		return new OperationStatus(true, "Connected");
	}

	public OperationStatus GetSupportedApis() => _context == null
		? new OperationStatus(false, THE_CLIENT_IS_NOT_CONNECTED_TO_ICA)
		: new OperationStatus(true, "Get APIs", _context.SchemaDataEntries);

	private string GetEventDirectory(int eventId)
	{
		var executablePath = AppDomain.CurrentDomain.BaseDirectory;

		var dirName = $"{eventId}";

		var fullPath = Path.Combine(executablePath, dirName);

		Directory.CreateDirectory(fullPath);

		return fullPath;
	}
}