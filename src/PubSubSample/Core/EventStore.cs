// Copyright By Hossein Azizollahi All Right Reserved.

using System.Threading.Tasks;
using PubSubSample.Abstractions.Data;
using PubSubSample.Abstractions.Services;
using PubSubSample.Services;

namespace PubSubSample.Core;

class EventStore : IEventStore
{
	private readonly IStore store;

	public EventStore(IStore store)
	{
		this.store = store;
	}
	public async Task Publish<T>(T @event) where T : EventBase
	{
		await store.Save(@event);
	}
}
