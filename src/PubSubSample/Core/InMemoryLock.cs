// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using PubSubSample.Services;

namespace PubSubSample.Core;

internal class InMemoryLockFactory : ILockServiceFactory
{
	public Task<ILockService> Create(string _, TimeSpan timeSpan)
	{
		ILockService lockService = new InMemoryLock(timeSpan);
		return Task.FromResult(lockService);
	}
}

internal class InMemoryLock : ILockService
{
	private readonly TimeSpan timeSpan;
	private readonly SemaphoreSlim semaphore = new(1, MaximumCountOnSemaphore);
	private const int MaximumCountOnSemaphore = 1;
	public InMemoryLock(TimeSpan timeSpan)
	{
		this.timeSpan = timeSpan;
	}

	public void Dispose()
	{
		if(semaphore.CurrentCount < MaximumCountOnSemaphore)
			semaphore.Release(MaximumCountOnSemaphore - semaphore.CurrentCount);
	}

	public ValueTask DisposeAsync()
	{
		if(semaphore.CurrentCount < MaximumCountOnSemaphore)
			semaphore.Release(MaximumCountOnSemaphore - semaphore.CurrentCount);
		return default;
	}

	public Task<bool> Acquire()
	{
		if(semaphore.CurrentCount <= 0)
			return Task.FromResult(false);
		semaphore.Wait(timeSpan);
		return Task.FromResult(true);
	}
}
