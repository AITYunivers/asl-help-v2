using System;
using System.Reflection;

namespace AslHelp.Common.Results;

public sealed class Error<TError>(
    TError code,
    string? message = null)
    where TError : Enum
{
    private string? _message = message;

    public string Message
    {
        get
        {
            if (_message is not null)
            {
                return _message;
            }

            string code = Code.ToString();
            _message =
                typeof(TError).GetField(code).GetCustomAttribute<ErrorMessageAttribute>()?.Message is string message
                ? message
                : code;

            return _message;
        }
    }

    public TError Code { get; } = code;

    public static implicit operator Error<TError>(TError code)
    {
        return new(code);
    }
}
