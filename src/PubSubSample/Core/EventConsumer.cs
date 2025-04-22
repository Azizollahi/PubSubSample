// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PubSubSample.Abstractions.Data;
using PubSubSample.Abstractions.Services;
using PubSubSample.Services;

namespace PubSubSample.Core;

sealed class EventConsumer<T> : IEventConsumer where T: EventBase
{
	private readonly IStore store;
	private readonly IEventSubscriber<T> subscriber;
	private readonly ILockServiceFactory lockServiceFactory;
	private readonly IOptions<EventConsumerOptions> options;
	public EventConsumer(IStore store, IEventSubscriber<T> subscriber, ILockServiceFactory lockServiceFactory, IOptions<EventConsumerOptions> options)
	{
		this.store = store;
		this.subscriber = subscriber;
		this.lockServiceFactory = lockServiceFactory;
		this.options = options;
	}
	public async Task<bool> Execute()
	{
		await using var lockService = await lockServiceFactory.Create(options.Value.Type, options.Value.LockExpiry);
		if (await lockService.Acquire())
		{
			bool result = await RunEventDispatching();
			if(!result)
				await Task.Delay(options.Value.DelayTimeSpan);
			return result;
		}
		await Task.Delay(options.Value.DelayTimeSpan);
		return true;
	}

	private async Task<bool> RunEventDispatching()
	{
		var events = await store.Load<T>(options.Value.Type, options.Value.FetchCount);
		if (events.Count <= 0)
			return false;
		foreach (T @event in events)
		{
			@event.ChangeStatus(EventStatus.Pending);
			try
			{
				var result = await subscriber.Subscribe(@event);
				if (result)
				{
					if (@event.FailCount >= options.Value.MaxFailRetry)
						@event.ChangeStatus(EventStatus.Dead, result);
					else
						@event.ChangeStatus(EventStatus.Fail, result);
				}
				else
					@event.ChangeStatus(EventStatus.Success, result);
			}
			catch (AgException exp)
			{
				@event.ChangeStatus(EventStatus.Fail, exp.Message);
			}
			catch (Exception)
			{
				@event.ChangeStatus(EventStatus.Fail);
				await store.Save(@event);
				return false;
			}
			await store.Save(@event);
		}
		return true;
	}
}
