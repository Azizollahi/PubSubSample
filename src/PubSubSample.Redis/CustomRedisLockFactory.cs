// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Threading.Tasks;
using PubSubSample.Services;
using RedLockNet;

namespace PubSubSample.Redis;

sealed class CustomRedisLockFactory : ILockServiceFactory
{
	private readonly IDistributedLockFactory redisLock;

	public CustomRedisLockFactory(IDistributedLockFactory redisLock)
	{
		this.redisLock = redisLock;
	}
	public async Task<ILockService> Create(string resource, TimeSpan timeSpan)
	{
		return new DistributedLock(await redisLock.CreateLockAsync(resource, timeSpan));
	}
}

public record RedisEndPoint(string DnsAddress, int Port);
