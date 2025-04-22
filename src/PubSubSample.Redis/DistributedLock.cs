// Copyright By Hossein Azizollahi All Right Reserved.

using System.Threading.Tasks;
using PubSubSample.Services;
using RedLockNet;

namespace PubSubSample.Redis;

sealed class DistributedLock : ILockService
{
	private readonly IRedLock redLock;

	public DistributedLock(IRedLock redLock)
	{
		this.redLock = redLock;
	}

	public Task<bool> Acquire()
	{
		return Task.FromResult(redLock.IsAcquired);
	}

	public async ValueTask DisposeAsync()
	{
		await redLock.DisposeAsync();
	}

	public void Dispose() => redLock.Dispose();
}
