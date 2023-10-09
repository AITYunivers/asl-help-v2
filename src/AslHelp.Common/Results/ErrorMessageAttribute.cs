using System;

namespace AslHelp.Common.Results;

[AttributeUsage(AttributeTargets.Field)]
public class ErrorMessageAttribute(
    string message) : Attribute
{
    public string Message { get; } = message;
}
