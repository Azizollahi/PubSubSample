// Copyright By Hossein Azizollahi All Right Reserved.

using System.Threading.Tasks;
using PubSubSample.Abstractions.Data;

namespace PubSubSample.Abstractions.Services;

public interface IEventStore
{
	public Task Publish<T>(T @event) where T : EventBase;
}
