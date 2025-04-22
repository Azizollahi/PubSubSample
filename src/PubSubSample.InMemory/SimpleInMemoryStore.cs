
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using PubSubSample.Abstractions.Data;
using PubSubSample.Services;

namespace PubSubSample.InMemory;

class SimpleInMemoryStore : IStore
{
	private readonly List<WrappedEvent> storage = [];

	public Task<List<T>> Load<T>(string type, int count = 1) where T : EventBase
	{
		var listOfNeededEvents = storage.Where(s => s.Type == type).Take(count).ToList();
		return Task.FromResult(listOfNeededEvents.Select(s=> JsonSerializer.Deserialize<T>(s.RawEvent)).ToList())!;
	}

	public Task Save<T>(T @event) where T : EventBase
	{
		var wrappedEvent = new WrappedEvent(JsonSerializer.Serialize(@event), @event.Type, @event.CorrelationId);
		var found = storage.FirstOrDefault(s => s.CorrelationId == @event.CorrelationId);
		if (found != null)
			storage.Remove(found);
		storage.Add(wrappedEvent);
		return Task.CompletedTask;
	}

	private class WrappedEvent : EventBase
	{
		public string RawEvent { get; }
		public WrappedEvent(string rawEvent, string type, string correlationId) : base(type, correlationId)
		{
			RawEvent = rawEvent;
		}
	}
}

