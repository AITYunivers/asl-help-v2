using System;
using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Common.Results;

public record Result(
    [property: MemberNotNullWhen(false, nameof(Result.Throw))]
    bool IsSuccess,
    Action? Throw = null);

public record Result<T>(
    [property: MemberNotNullWhen(true, nameof(Result<T>.Value))]
    [property: MemberNotNullWhen(false, nameof(Result<T>.Throw))]
    bool IsSuccess,
    T? Value = default,
    Action? Throw = null) : Result(IsSuccess, Throw);
