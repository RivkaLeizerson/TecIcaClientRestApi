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

using Microsoft.AspNetCore.Hosting;
using TecIcaClient.WebApi.Logic;

namespace TecIcaClient.WebApi;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.UseUrls("http://*:5000");
		

        // Add services to the container.
        builder.Services.AddRazorPages();

		builder.Services.AddSingleton<IIcaClientActions, IcaClientActions>();

		var app = builder.Build();
		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthorization();
		app.MapControllers();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapRazorPages();
			endpoints.MapGet("/", context =>
			{
				context.Response.Redirect("/Readme");
				return Task.CompletedTask;
			});
		});

		app.MapRazorPages();

		app.Run();
	}

  
}