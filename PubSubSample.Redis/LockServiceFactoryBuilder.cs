// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using PubSubSample.Services;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace PubSubSample.Redis;

public static class LockServiceFactoryBuilder
{
	public static ILockServiceFactory CreateLockServiceFactory(IServiceProvider _, IConfiguration configuration)
	{
		var redisOptions = configuration.GetValue<RedisOptions>("RedisOptions") ?? new RedisOptions
		{
			RedisEndPoints = [new("localhost", 6379)]
		};
		var addresses = redisOptions.RedisEndPoints.Select(s => $"{s.DnsAddress}:{s.Port}").ToList();
		var multiplexers = new List<RedLockMultiplexer>();
		foreach (string address in addresses)
		{
			multiplexers.Add(ConnectionMultiplexer.ConnectAsync(address)
				.ConfigureAwait(false)
				.GetAwaiter()
				.GetResult());
		}
		IDistributedLockFactory distributedLockFactory = RedLockFactory.Create(multiplexers);
		return new CustomRedisLockFactory(distributedLockFactory);
	}
}
