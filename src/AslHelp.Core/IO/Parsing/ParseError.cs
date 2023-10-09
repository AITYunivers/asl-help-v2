using AslHelp.Common.Results;

namespace AslHelp.Core.IO.Parsing;

public enum ParseError
{
    [ErrorMessage("The specified embedded resource could not be found.")]
    EmbeddedResourceNotFound,

    [ErrorMessage("The JSON contents are invalid.")]
    InvalidJson,

    [ErrorMessage($"{nameof(NativeFieldParser)} encountered a critical error.")]
    FieldParseException
}
