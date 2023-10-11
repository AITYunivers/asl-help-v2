using System;

namespace AslHelp.Common.Results;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
public sealed class ErrorMessageAttribute(
    string message) : Attribute
{
    public string Message { get; } = message;
}
