// Copyright By Hossein Azizollahi All Right Reserved.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PubSubSample.Abstractions.Data;
using PubSubSample.Abstractions.Services;
using PubSubSample.InMemory;
using PubSubSample.Services;
using Xunit;

namespace PubSubSample.Test;

public class DummyEventSampleTest
{
	[Fact]
	public async Task Run()
	{
		IServiceCollection services = new ServiceCollection();
		var configBuilder = new ConfigurationBuilder();

		var configuration = configBuilder.Build();
		var dummyEventSpy = new DummyEventSpy();
		services.AddSingleton(dummyEventSpy);

		// We have the ability to use Redis here too as the 3rd parameter.
		services.AddEventStore(configuration, InMemoryStoreFactory.CreateInMemoryStore);
		services.AddEventConsumer<DummyEvent, DummyEventSubscriber>(new EventConsumerOptions()
		{
			Type = "Hello"
		});

		var sp = services.BuildServiceProvider();

		var eventStore = sp.GetRequiredService<IEventStore>();
		await eventStore.Publish(new DummyEvent("1212", "Good day"));

		// This part will be run in background as background hosted service, Now for the sake of test we run it here.
		// Also, IEventConsumer is not accessible from the outside of PubSubSample project. (only accessible for the internal usage of the library)
		var eventConsumer = sp.GetRequiredKeyedService<IEventConsumer>("Hello");
		await eventConsumer.Execute();

		if(dummyEventSpy.Events.Any())
			Assert.Fail("Events Failed");
	}
}

class DummyEventSpy
{
	public List<DummyEvent> Events = new();
}

class DummyEventSubscriber : IEventSubscriber<DummyEvent>
{
	private readonly DummyEventSpy eventSpy;
	public DummyEventSubscriber(DummyEventSpy eventSpy)
	{
		this.eventSpy = eventSpy;
	}
	public Task<Error> Subscribe(DummyEvent @event)
	{
		Console.WriteLine($"Subscribed to the event: {@event.Type}");
		if(@event.GoodObject != "Good day")
			eventSpy.Events.Add(@event);
		return Task.FromResult(Error.Success());
	}
}

class DummyEvent : EventBase
{
	public DummyEvent(string correlationId, string goodObject) : base("Hello", correlationId)
	{
		GoodObject = goodObject;
	}
	public string GoodObject { get; }
}
