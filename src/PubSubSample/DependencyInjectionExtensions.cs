// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PubSubSample.Abstractions.Data;
using PubSubSample.Abstractions.Services;
using PubSubSample.Core;
using PubSubSample.Services;

namespace PubSubSample;

public static class DependencyInjectionExtensions
{
	public static void AddEventStore(this IServiceCollection services, IConfiguration configuration, Func<IServiceProvider,
		IConfiguration, IStore> configureStore,
		Func<IServiceProvider, IConfiguration, ILockServiceFactory> configureLockServiceFactory)
	{
		services.AddSingleton<IEventStore, EventStore>();
		services.AddSingleton<IStore>((sp) => configureStore(sp, configuration));
		services.AddSingleton<ILockServiceFactory>((sp) => configureLockServiceFactory(sp, configuration));
	}
	public static void AddEventStore(this IServiceCollection services, IConfiguration configuration, Func<IServiceProvider,
			IConfiguration, IStore> configureStore)
	{
		services.AddSingleton<IEventStore, EventStore>();
		services.AddSingleton<IStore>(sp => configureStore(sp, configuration));
		services.AddSingleton<ILockServiceFactory>(_ => new InMemoryLockFactory());
	}
	public static void AddEventConsumer<T, TSubscriber>(this IServiceCollection services, EventConsumerOptions options)
		where T : EventBase
		where TSubscriber : class, IEventSubscriber<T>
	{
		services.AddSingleton<IEventSubscriber<T>,TSubscriber>();
		services.AddKeyedSingleton<IEventConsumer>(options.Type, (sp, _) =>
			ActivatorUtilities.CreateInstance<EventConsumer<T>>(sp, Options.Create(options)));
		services.AddHostedService<EventConsumerHostedService>(sp =>
			new EventConsumerHostedService(sp.GetRequiredKeyedService<IEventConsumer>(options.Type)));
	}
}
