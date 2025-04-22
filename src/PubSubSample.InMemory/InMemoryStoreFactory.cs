using System;
using Microsoft.Extensions.Configuration;
using PubSubSample.Services;

namespace PubSubSample.InMemory;

public static class InMemoryStoreFactory
{
	public static IStore CreateInMemoryStore(IServiceProvider sp, IConfiguration _)
	{
		return new SimpleInMemoryStore();
	}
}
