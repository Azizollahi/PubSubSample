// Copyright By Hossein Azizollahi All Right Reserved.

namespace PubSubSample.Abstractions.Data;

public enum EventStatus
{
	None= 0,
	Idle=100,
	Pending=200,
	Fail=400,
	Success=600,
	Dead=700,
}
