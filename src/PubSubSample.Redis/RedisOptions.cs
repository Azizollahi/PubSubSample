// Copyright By Hossein Azizollahi All Right Reserved.

using System.Collections.Generic;

namespace PubSubSample.Redis;

public class RedisOptions
{
	public List<RedisEndPoint> RedisEndPoints { get; init; } = new();
}
