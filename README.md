# IcaTecClient API

## Overview

The IcaTecClient API provides a set of endpoints for managing subscriptions to events, sending commands, and connecting to the ICA system. This API is structured around REST, which is designed to have predictable, resource-oriented URLs and to use HTTP response codes to indicate API errors.

## API Endpoints

### Subscribe to Event

- **POST** `/IcaTecClient/Subscribe/{eventId}`
  
  Subscribes the client to a specified event.

  - `eventId` (int): The ID of the event to subscribe to.

### Unsubscribe from Event

- **POST** `/IcaTecClient/Unsubscribe/{eventId}`
  
  Unsubscribes the client from a specified event.

  - `eventId` (int): The ID of the event to unsubscribe from.

### Send Command

- **POST** `/IcaTecClient/SendCommand/{commandId}`
  
  Sends a command to the ICA system.

  - `commandId` (int): The ID of the command to send.

### Connect to ICA

- **POST** `/IcaTecClient/Connect`
  
  Initiates a connection to the ICA system.

### Get Supported APIs

- **GET** `/IcaTecClient/GetApis`
  
  Retrieves a list of supported APIs available in the ICA system.

## Response Structure

Responses will be in JSON format. Successful calls to the API endpoints will return a 200 OK status code along with any relevant data. Errors will be signaled with standard HTTP error response codes, accompanied by error messages indicating the nature of the error.

## Error Handling

The API uses the following error codes:

- 400 Bad Request: The request was invalid or cannot be served. The exact error is explained in the error message.
- 404 Not Found: The requested resource could not be found.
- 500 Internal Server Error: Something went wrong on the API's end. If the issue persists, please contact the support team.

## Usage

To use these endpoints, make HTTP requests with the appropriate method (GET or POST) to the given URL, substituting path parameters (e.g., `{eventId}`) with the actual values you wish to use.

## Examples

Here are a few examples of how you might call the API endpoints:

```bash
# Subscribe to an event with ID 9100 ("USB Connect To Device")
curl -X POST https://yourdomain.com/IcaTecClient/Subscribe/9100

# Unsubscribe from an event with ID 9112 ("TEC Presence")
curl -X POST https://yourdomain.com/IcaTecClient/Unsubscribe/9112

# Send a command with ID 1 ("Thunderbolt Rundown")
curl -X POST https://yourdomain.com/IcaTecClient/SendCommand/1

# Connect to ICA
curl -X POST https://yourdomain.com/IcaTecClient/Connect

# Get a list of supported APIs
curl -X GET https://yourdomain.com/IcaTecClient/GetApis
```

## Contact

For further assistance or to report issues, please contact the API support team at [avi.zadok@intel.com](mailto:avi.zadok@intel.com).

---

Note: Replace `yourdomain.com` with your actual domain where the API is hosted. This README assumes a certain level of familiarity with making HTTP requests and using APIs. Adjust the content according to the target audience's expertise level.
