// Copyright By Hossein Azizollahi All Right Reserved.

using System;
using System.Collections;
using System.Collections.Generic;

namespace PubSubSample.Abstractions.Data;

public class AgException : Exception
{
	public string Detail { get; }
	public int Status { get; protected set; } = 400;
	private readonly Dictionary<string, object> additionalData;
	public sealed override IDictionary Data => additionalData;

	public AgException(string message, string detail = "") : base(message)
	{
		Detail = detail;
		additionalData = new Dictionary<string, object>();
	}

	public AgException(string message, Exception innerException, string detail = "") : base(message, innerException)
	{
		Detail = detail;
		additionalData = new Dictionary<string, object>();
	}

	public AgException AddInfo(string key, object value)
	{
		Data.Add(key, value);
		return this;
	}

	public AgException AddInfo(string key, int value)
	{
		Data.Add(key, value);
		return this;
	}

	public AgException AddInfo(string key, long value)
	{
		Data.Add(key, value);
		return this;
	}

	public AgException AddInfo(string key, float value)
	{
		Data.Add(key, value);
		return this;
	}

	public AgException AddInfo(string key, double value)
	{
		Data.Add(key, value);
		return this;
	}

	public AgException AddInfo(string key, decimal value)
	{
		Data.Add(key, value);
		return this;
	}

	public AgException AddInfo(string key, short value)
	{
		Data.Add(key, value);
		return this;
	}

	public AgException AddInfo(string key, bool value)
	{
		Data.Add(key, value);
		return this;
	}

	public AgException AddInfo(string key, string value)
	{
		Data.Add(key, value);
		return this;
	}
}
