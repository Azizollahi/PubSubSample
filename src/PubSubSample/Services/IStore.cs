// Copyright By Hossein Azizollahi All Right Reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using PubSubSample.Abstractions.Data;

namespace PubSubSample.Services;

public interface IStore
{
	public Task<List<T>> Load<T>(string type, int count = 1) where T : EventBase;
	public Task Save<T>(T @event) where T : EventBase;
}
