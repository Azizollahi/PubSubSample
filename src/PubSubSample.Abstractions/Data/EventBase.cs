// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Collections.Generic;

namespace PubSubSample.Abstractions.Data;

public class EventBase : IEvent
{
	public EventBase(string type, string correlationId)
	{
		Id = Guid.NewGuid().ToString("N");
		StatusHistory = new List<EventStatusChange>();
		CreatedAt = DateTimeOffset.UtcNow;
		Type = type;
		CorrelationId = correlationId;
		Status = EventStatus.Idle;
		FailCount = 0;
		ModifiedAt = CreatedAt;
	}
	public EventBase(string id, string type, string correlationId, DateTimeOffset createdAt, EventStatus status,
		int failCount, DateTimeOffset modifiedAt)
	{
		Id = id;
		CorrelationId = correlationId;
		Type = type;
		CreatedAt = createdAt;
		Status = status;
		FailCount = failCount;
		ModifiedAt = modifiedAt;
	}
	public string Id { get; init; }
	public DateTimeOffset CreatedAt { get; init; }
	public string CorrelationId { get; init; }
	public string Type { get; init; }
	public EventStatus Status { get; private set; }
	public int FailCount { get; private set; }
	public List<EventStatusChange> StatusHistory { get; private set; }
	public DateTimeOffset ModifiedAt { get; private set; }

	internal void ChangeStatus(EventStatus status, string message="")
	{
		var occurrence = DateTime.UtcNow;
		ModifiedAt = occurrence;
		Status = status;
		if(status == EventStatus.Fail)
			FailCount++;
		StatusHistory.Add(new EventStatusChange(occurrence, status, message));
	}
}
