// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using PubSubSample.Services;

namespace PubSubSample.Core;

sealed class EventConsumerHostedService : BackgroundService
{
	private readonly IEventConsumer eventConsumer;

	public EventConsumerHostedService(IEventConsumer eventConsumer)
	{
		this.eventConsumer = eventConsumer;
	}
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				await eventConsumer.Execute();
			}
			catch (Exception exp)
			{
				Console.WriteLine(exp.ToString());
			}
		}
	}
}
