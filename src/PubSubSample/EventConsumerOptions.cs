// Copyright By Hossein Azizollahi All Right Reserved.

using System;

namespace PubSubSample;

public class EventConsumerOptions
{
	public required string Type { get; init; }
	public TimeSpan DelayTimeSpan { get; init; } = TimeSpan.FromSeconds(10);
	public TimeSpan LockExpiry { get; init; } = TimeSpan.FromSeconds(30);
	public int FetchCount { get; init; } = 1;
	public int MaxFailRetry { get; init; } = 1;
}
