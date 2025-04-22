// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Threading.Tasks;

namespace PubSubSample.Services;

public interface ILockService : IDisposable, IAsyncDisposable
{
	Task<bool> Acquire();
}
