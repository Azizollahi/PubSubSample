// Copyright By Hossein Azizollahi All Right Reserved.

using System;

namespace PubSubSample.Abstractions.Data;

public struct Error
{
	public static Error Success(string message="")
	{
		return new Error() { IsFail = false, Message = message };
	}
	public static Error Fail(string message="")
	{
		return new Error() { IsFail = true, Message = message };
	}

	public override string ToString() => Message;
	public override bool Equals(object? obj)
	{
		if (obj is not Error other)
			return false;
		if (IsFail == other.IsFail)
			return true;
		return false;
	}
	public override int GetHashCode() => IsFail.GetHashCode();

	public static bool operator==(Error left, bool right)
	{
		return left.IsFail == right;
	}
	public static bool operator!=(Error left, bool right)
	{
		return left.IsFail != right;
	}
	public static bool operator==(bool left, Error right)
	{
		return left == right.IsFail;
	}
	public static bool operator!=(bool left, Error right)
	{
		return left != right.IsFail;
	}
	public static implicit operator bool(Error error)
	{
		return error.IsFail;
	}
	public static implicit operator string(Error error)
	{
		return error.Message;
	}
	public static implicit operator Tuple<bool, string>(Error error)
	{
		return new Tuple<bool, string>(error.IsFail, error.Message);
	}
	public static implicit operator Error(Tuple<bool, string> error)
	{
		if(error.Item1)
			return Fail(error.Item2);
		return Success(error.Item2);
	}

	public bool IsFail { get; private set; }
	public string Message { get; private set; }
}
