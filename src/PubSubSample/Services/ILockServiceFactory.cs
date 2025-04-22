// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Threading.Tasks;

namespace PubSubSample.Services;

public interface ILockServiceFactory
{
	Task<ILockService> Create(string resource, TimeSpan timeSpan);
}
