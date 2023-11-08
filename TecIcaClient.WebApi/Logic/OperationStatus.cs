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

namespace TecIcaClient.WebApi.Logic;

public class OperationStatus
{
	public OperationStatus(bool success, string reason, object? data = null)
	{
		Success = success;
		Reason = reason;
		Data = data;
	}

	public bool Success { get; }

	public string Reason { get; }

	public object? Data { get; set; }
}