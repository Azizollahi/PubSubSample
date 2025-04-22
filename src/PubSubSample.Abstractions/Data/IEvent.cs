// Copyright By Hossein Azizollahi All Right Reserved.

using System;

namespace PubSubSample.Abstractions.Data;

public interface IEvent
{
	public DateTimeOffset CreatedAt { get; }
	public string CorrelationId { get; }
}
