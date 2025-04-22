// Copyright By Hossein Azizollahi All Right Reserved.

using System.Threading.Tasks;
using PubSubSample.Abstractions.Data;

namespace PubSubSample.Abstractions.Services;

public interface IEventSubscriber<in T> where T : EventBase
{
	Task<Error> Subscribe(T @event);
}
