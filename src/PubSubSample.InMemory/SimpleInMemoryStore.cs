
using System;
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
		var listOfNeededEvents = storage.Where(s => s.Type == type && s.Status != EventStatus.Dead).Take(count).ToList();
		return Task.FromResult(listOfNeededEvents.Select(s=> JsonSerializer.Deserialize<T>(s.RawEvent)).ToList())!;
	}

	public Task Save<T>(T @event) where T : EventBase
	{
		var wrappedEvent = new WrappedEvent(JsonSerializer.Serialize(@event), @event.Id, @event.Type,
			@event.CorrelationId, @event.CreatedAt, @event.Status, @event.FailCount, @event.ModifiedAt);
		var found = storage.FirstOrDefault(s => s.Type == @event.Type &&
		                                        s.CorrelationId == @event.CorrelationId);
		if (found != null)
			storage.Remove(found);
		storage.Add(wrappedEvent);
		return Task.CompletedTask;
	}

	private class WrappedEvent : EventBase
	{
		public string RawEvent { get; }
		public WrappedEvent(string rawEvent, string id, string type, string correlationId, DateTimeOffset createdAt,
			EventStatus status, int failCount, DateTimeOffset modifiedAt) : base(id, type, correlationId, createdAt,
			status, failCount, modifiedAt)
		{
			RawEvent = rawEvent;
		}
	}
}

