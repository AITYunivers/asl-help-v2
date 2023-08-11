using System;
using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Common.Results;

public record Result(
    [property: MemberNotNullWhen(false, nameof(Result.Throw))]
    bool IsSuccess,
    Action? Throw = null);

public record Result<T>(
    bool IsSuccess,
    T? Value = default,
    Action? Throw = null) : Result(IsSuccess, Throw);
