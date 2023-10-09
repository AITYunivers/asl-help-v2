using System;
using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Common.Results;

public record Result<TError>(
    bool IsSuccess,
    Error<TError>? Error = null)
    where TError : Enum
{
    [MemberNotNullWhen(false, nameof(Result<TError>.Error))]
    public virtual bool IsSuccess { get; } = IsSuccess;

    public Error<TError>? Error { get; } = Error;
}

public record Result<TValue, TError>(
    bool IsSuccess,
    TValue? Value = default,
    Error<TError>? Error = null) : Result<TError>(IsSuccess, Error)
    where TError : Enum
{
    [MemberNotNullWhen(true, nameof(Result<TValue, TError>.Value))]
    public override bool IsSuccess { get; } = IsSuccess;

    public TValue? Value { get; } = Value;
}
