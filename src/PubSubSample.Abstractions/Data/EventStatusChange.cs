// Copyright By Hossein Azizollahi All Right Reserved.

using System;

namespace PubSubSample.Abstractions.Data;

public class EventStatusChange
{
	public EventStatusChange(DateTime occurrenceDateTime, EventStatus status, string message)
	{
		Id = Guid.NewGuid().ToString("N");
		OccurrenceDateTime = occurrenceDateTime;
		Status = status;
		Message = message;
	}

	public string Id { get; init; }
	public DateTime OccurrenceDateTime { get; init; }
	public EventStatus Status { get; init; }
	public string Message { get; init; }
}
