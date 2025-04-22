// Copyright By Hossein Azizollahi All Right Reserved.

using System.Threading.Tasks;

namespace PubSubSample.Services;

internal interface IEventConsumer
{
	public Task<bool> Execute();
}
